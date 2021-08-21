using Acr.UserDialogs;
using AppCarnesDF.Helpers.Common;
using AppCarnesDF.ViewModels.Promotion;
using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCarnesDF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Promociones : ContentPage
    {
        private  readonly PromotionViewModel context = new PromotionViewModel();

        public Promociones()
        {
            InitializeComponent();
            BindingContext = context;
        }

        /// <summary>
        /// Permite consultar los tamaños de fuentes para el texto de la aplicación
        /// </summary>
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                if (context.Promotions == null)
                {
                    context.Inicializar();
                }
                await context.UpdateFontSize();
            }
            catch (Exception ex)
            {
                await context.Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
            }
        }
    }
}