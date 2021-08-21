using AppCarnesDF.ViewModels.MainPage;
using Rg.Plugins.Popup.Services;
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
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel context = new MainPageViewModel();

        public MainPage()
        {
            InitializeComponent();
            
            try
            {
                BindingContext = context;
                context.MainPageView = this;
            }
            catch (Exception ex)
            {
                Task.Run(async () =>
                {
                    await context.Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
                });
            }
        }

        public async Task UpdateFontSize() {

            try
            {
                await context.UpdateFontSize();
            }
            catch (Exception ex)
            {
                await context.Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
            }

        }

        /// <summary>
        /// Permite consultar los tamaños de fuentes para el texto de la aplicación
        /// </summary>
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await UpdateFontSize();
        }
    }
}