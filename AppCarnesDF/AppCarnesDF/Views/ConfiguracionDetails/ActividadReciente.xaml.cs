using AppCarnesDF.Models.ActividadReciente;
using AppCarnesDF.Models.User;
using AppCarnesDF.ViewModels.ActividadReciente;
using AppCarnesDF.ViewModels.Users;
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
    public partial class ActividadReciente : ContentPage
    {
        private ActividadRecienteViewModel context;        

        public ActividadReciente(UserModel user, UserViewModel viewModel, List<ActividadRecienteModel> Actividades)
        {
            InitializeComponent();
            context = new ActividadRecienteViewModel(user, viewModel);
            BindingContext = context;
            context.Inicializar(Actividades);
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