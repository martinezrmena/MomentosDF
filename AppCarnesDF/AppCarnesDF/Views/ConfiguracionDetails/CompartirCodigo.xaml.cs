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
    public partial class CompartirCodigo : ContentPage
    {
        private CompartirCodigoViewModel context = new CompartirCodigoViewModel();

        public CompartirCodigo(UserModel user, ConfiguracionViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = context;
            context.User = user;
            context.ConfiguracionViewModel = viewModel;
        }

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