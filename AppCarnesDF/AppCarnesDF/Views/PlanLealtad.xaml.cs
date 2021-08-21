using Acr.UserDialogs;
using AppCarnesDF.ViewModels.PlanLealtad;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCarnesDF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlanLealtad : ContentPage
    {
        private PlanLealtadViewModel context = new PlanLealtadViewModel();

        public PlanLealtad(bool cerrar)
        {
            InitializeComponent();
            context.Cerrar = cerrar;
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
                if (context.PlanesLealtad == null)
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

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {

                    await context.Close();
                }
                catch (Exception ex)
                {
                    await context.Message.Failed("Estimado usuario ocurrió un error: " + ex.Message);
                }
            });

            return true;

        }

    }
}