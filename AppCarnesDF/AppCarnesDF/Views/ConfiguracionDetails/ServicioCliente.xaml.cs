using AppCarnesDF.Models.User;
using AppCarnesDF.ViewModels.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCarnesDF.Views.ConfiguracionDetails
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServicioCliente : ContentPage
    {
        public ServicioClienteViewModel context = new ServicioClienteViewModel();

        public ServicioCliente(UserModel userModel)
        {
            InitializeComponent();
            context.UserAutenticated = userModel;
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
                await context.UpdateFontSize();
            }
            catch (Exception ex)
            {
                await context.Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
            }

        }
    }
}