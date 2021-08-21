using AppCarnesDF.Helpers;
using AppCarnesDF.Helpers.Common;
using AppCarnesDF.Models.Promotion;
using AppCarnesDF.Services;
using AppCarnesDF.Services.FontSize;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using Xamarin.Essentials;
using System.Globalization;

namespace AppCarnesDF.ViewModels.Promotion
{
    public class PromotionViewModel : BaseViewModel
    {
        #region Properties
        private readonly FontSizeService FontService = new FontSizeService();

        private readonly WebApiService webApiService = new WebApiService();

        public int LimitValues { get; set; } = 5;

        public readonly LogMessageAttention Message = new LogMessageAttention();

        public readonly BrowserView browserView = new BrowserView();

        private GeneralUtilities Utilities = new GeneralUtilities();

        private ObservableCollection<PromotionModel> promotions;

        public ObservableCollection<PromotionModel> Promotions
        {
            get { return promotions; }
            set
            {
                promotions = value;
                OnPropertyChanged();
            }
        }

        private PromotionModel _selectedItem;

        public PromotionModel SelectedItem
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

        public PromotionViewModel()
        {
        }

        public void Inicializar()
        {
            ObservableCollection<PromotionModel> TempPromotions = new ObservableCollection<PromotionModel>();

            Thread actividad = new Thread(new ThreadStart(async () =>
            {
                try
                {
                    List<PromotionModel> promotionsd = new List<PromotionModel>();

                    if (await Utilities.VerifyInternetConnection())
                    {
                        promotionsd = await GetPromotions();
                    }

                    if (promotionsd != null)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            try
                            {
                                foreach (var item in promotionsd)
                                {
                                    //Verificamos que solo se agreguen 5
                                    if (TempPromotions.Count != LimitValues)
                                    {
                                        //Que la fecha de finalizacion no haya pasado
                                        var Finalizo = DateTime.ParseExact(item.Fecha_Finalizacion, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                        var valuedate = DateTime.Compare(Finalizo.Date, DateTime.Now.Date);

                                        if (valuedate >= 0)
                                        {
                                            //if > 0 date1 is later than date2
                                            //if = 0 date1 equals date2
                                            TempPromotions.Add(item);
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                Promotions = TempPromotions;
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
        public async Task<List<PromotionModel>> GetPromotions() {

            // Definir el token de cancelación.
            CancellationTokenSource source = new CancellationTokenSource(new TimeSpan(0, 1, BrowserView.TimeOutLimit));
            CancellationToken token = source.Token;
            List<PromotionModel> webPromotions = null;

            try
            {
                webPromotions = await webApiService.GetPromotions(token);
            }
            catch (Exception)
            {
                await Message.Failed("Estimado usuario, existen problemas de conexión con el servicio web actualmente, intentelo más tarde.");
            }

            return webPromotions;
        }

        /// <summary>
        /// Metodo que permite abrir una página en el navegador apuntado al vinculo
        /// de la imagen seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public async void RedireccionaEnlace(PromotionModel promotion)
        {
            try
            {
                if (promotion != null)
                {
                    if (!string.IsNullOrEmpty(promotion.Enlace))
                    {
                        await browserView.OpenBrowser(new Uri(promotion.Enlace));
                    }
                }
            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
            }
        }

    }
}
