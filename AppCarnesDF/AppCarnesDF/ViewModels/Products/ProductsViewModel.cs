using Acr.UserDialogs;
using AppCarnesDF.Helpers;
using AppCarnesDF.Helpers.Common;
using AppCarnesDF.Models.Products;
using AppCarnesDF.Services;
using AppCarnesDF.Services.FontSize;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppCarnesDF.ViewModels.Products
{
    public class ProductsViewModel: BaseViewModel
    {
        #region Properties
        private readonly FontSizeService FontService = new FontSizeService();

        public int LimitValues { get; set; } = 5;

        private readonly WebApiService webApiService = new WebApiService();

        public readonly LogMessageAttention Message = new LogMessageAttention();

        private GeneralUtilities Utilities = new GeneralUtilities();

        private ObservableCollection<ProductModel> products;

        public ObservableCollection<ProductModel> Products
        {
            get { return products; }
            set
            {
                products = value;
                OnPropertyChanged();
            }
        }

        private ProductModel _selectedItem;

        public ProductModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                if (_selectedItem != null)
                {
                    RedireccionaEnlace(_selectedItem);
                }
            }
        }
        #endregion

        public ProductsViewModel(){}

        public void Inicializar()
        {
            ObservableCollection<ProductModel> TempProducts = new ObservableCollection<ProductModel>();

            Thread actividad = new Thread(new ThreadStart(async () =>
            {
                try
                {
                    List<ProductModel> productsd = new List<ProductModel>();

                    if (await Utilities.VerifyInternetConnection())
                    {
                        productsd = await GetProducts();
                    }

                    if (productsd != null)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            try
                            {
                                foreach (var item in productsd)
                                {
                                    //Verificamos que solo se agreguen 5
                                    if (TempProducts.Count != LimitValues)
                                    {
                                        //Que la fecha de finalizacion no haya pasado
                                        var Finalizo = DateTime.ParseExact(item.Fecha_Finalizacion, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                        var valuedate = DateTime.Compare(Finalizo.Date, DateTime.Now.Date);

                                        if (valuedate >= 0)
                                        {
                                            //if > 0 date1 is later than date2
                                            //if = 0 date1 equals date2
                                            TempProducts.Add(item);
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                Products = TempProducts;
                            }
                            catch (Exception ex)
                            {
                                await Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    await Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
                }
            }));

            actividad.Start();
        }

        /// <summary>
        /// Metodo que sirve para actualizar el tamaño de la fuente.
        /// Debe consultarse el tamaño por la base de datos.
        /// </summary>
        public async Task UpdateFontSize()
        {
            try
            {
                SizeFonts = FontService.ConsultarFont();
                SizeFontsCookie = FontService.ConsultarFontCookie();
                SizeFontsOptima = FontService.ConsultarFontOptima();
                Message.SizeFonts = SizeFonts;
                Message.SizeFontsCookie = SizeFontsCookie;
                Message.SizeFontsOptima = SizeFontsOptima;
            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario ocurrió un error. Código 209.");
            }
        }

        /// <summary>
        /// Metodo que permite consultar via web service las promociones que se encuentran
        /// disponibles
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductModel>> GetProducts()
        {

            // Definir el token de cancelación.
            CancellationTokenSource source = new CancellationTokenSource(new TimeSpan(0, 1, BrowserView.TimeOutLimit));
            CancellationToken token = source.Token;

            List<ProductModel> webProducts = await webApiService.GetProducts(token);

            return webProducts;
        }

        /// <summary>
        /// Metodo que permite abrir una página en el navegador apuntado al vinculo
        /// de la imagen seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public async void RedireccionaEnlace(ProductModel product)
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);
                    if (product != null)
                    {
                        ///Abrir pantalla con productos de detalle
                        await pantallas.showProductDetail(product);
                    }
                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }
    }
}
