using AppCarnesDF.ViewModels.Sucursales;
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
    public partial class Sucursales : ContentPage
    {
        private SucursalViewModel context = new SucursalViewModel();
        public Sucursales()
        {
            InitializeComponent();
            BindingContext = context;
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