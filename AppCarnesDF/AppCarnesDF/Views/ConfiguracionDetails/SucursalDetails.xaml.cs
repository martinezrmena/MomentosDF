using AppCarnesDF.Models.Sucursales;
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
    public partial class SucursalDetails : ContentPage
    {
        #region Properties
        public SucursalDetailsViewModel context = new SucursalDetailsViewModel();
        #endregion

        public SucursalDetails(SucursalesModel sucursalesDetails)
        {
            InitializeComponent();
            try
            {
                BindingContext = context;
                context.SucursalDetails = sucursalesDetails;
                context.ManageRestaurant();
                context.ManageTienda();
                context.FillPhoneNumberList();
            }
            catch (Exception ex)
            {
                Task.Run(async()=>
                {
                    await context.Message.Failed("Estimado usuario, ocurrio un error:" + ex.Message);
                });
            }
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

        private async void cvSucursales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                PhoneNumber value = (PhoneNumber)e.CurrentSelection.FirstOrDefault();

                await context.PhoneCall(value.Number);
            }
            catch (Exception ex)
            {
                await context.Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
            }
        }
    }
}