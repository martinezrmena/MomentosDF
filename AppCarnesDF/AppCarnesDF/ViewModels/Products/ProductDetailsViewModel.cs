using AppCarnesDF.Helpers.Common;
using AppCarnesDF.Models.Products;
using AppCarnesDF.Services.FontSize;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppCarnesDF.ViewModels.Products
{
    public class ProductDetailsViewModel: BaseViewModel
    {
        #region Properties
        private readonly FontSizeService FontService = new FontSizeService();

        public readonly LogMessageAttention Message = new LogMessageAttention();

        private ProductModel product;

        public ProductModel Product
        {
            get { return product; }
            set
            {
                product = value;
                SetPreparaciones();
                OnPropertyChanged();
            }
        }

        private ObservableCollection<PreparacionModel> preparaciones;

        public ObservableCollection<PreparacionModel> Preparaciones
        {
            get { return preparaciones; }
            set
            {
                preparaciones = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Command
        public ICommand BackCommand { get; set; }

        #endregion
        public ProductDetailsViewModel()
        {
            BackCommand = new Command(async()=> await Close());
        }

        private async Task Close()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
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

        private void SetPreparaciones()
        {
            ObservableCollection<PreparacionModel> values = new ObservableCollection<PreparacionModel>();

            string name;

            if (!string.IsNullOrEmpty(Product.UrlIconosPreparacion))
            {

                foreach (string splitstring in Regex.Split(Product.UrlIconosPreparacion, Simbol._comma))
                {
                    name = Path.GetFileName(splitstring);

                    if (name.Contains("Coccion"))
                    {
                        name = PreparacionModel.Coccion;
                    }
                    else if (name.Contains("Frio"))
                    {
                        name = PreparacionModel.Frio;
                    }
                    else if (name.Contains("Horno"))
                    {
                        name = PreparacionModel.Horno;
                    }
                    else if (name.Contains("Parrilla"))
                    {
                        name = PreparacionModel.Parrilla;
                    }
                    else if (name.Contains("Sarten"))
                    {
                        name = PreparacionModel.Sarten;
                    }

                    values.Add(new PreparacionModel(splitstring, name));
                }
            }

            Preparaciones = values;
        }
    }
}
