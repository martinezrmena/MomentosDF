using Acr.UserDialogs;
using AppCarnesDF.Helpers.Common;
using AppCarnesDF.ViewModels.UpdateFontSizes;
using Rg.Plugins.Popup.Pages;
using System;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace AppCarnesDF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateFontSizes : PopupPage
    {
        private UpdateFontSizesViewModel context = new UpdateFontSizesViewModel();

        public UpdateFontSizes()
        {
            InitializeComponent();

            try
            {
                BindingContext = context;
            }
            catch (Exception ex)
            {
                Task.Run(async () => 
                {
                    await context.messages.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
                });
            }
        }

        public UpdateFontSizes(MainPage view)
        {
            InitializeComponent();

            try
            {
                BindingContext = context;

                context.MainPageView = view;
            }
            catch (Exception ex)
            {
                Task.Run(async()=>
                {
                    await context.messages.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
                });
            }
        }

        protected async override void OnDisappearing()
        {
            base.OnDisappearing();

            try
            {
                UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);

                if (context.MainPageView != null)
                {
                    await context.MainPageView.UpdateFontSize();
                }
            }
            catch (Exception ex)
            {
                await context.messages.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }

        }

    }
}