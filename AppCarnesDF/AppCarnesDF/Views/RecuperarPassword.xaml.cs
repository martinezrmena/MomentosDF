using AppCarnesDF.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCarnesDF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecuperarPassword : ContentPage
    {

        private RecoverPasswordViewModel context = new RecoverPasswordViewModel();

        public RecuperarPassword()
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
                if (context != null)
                {
                    await context.UpdateFontSize();
                }
            }
            catch (Exception ex)
            {
                if (context != null)
                {
                    await context.Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
                }
            }
        }
    }
}