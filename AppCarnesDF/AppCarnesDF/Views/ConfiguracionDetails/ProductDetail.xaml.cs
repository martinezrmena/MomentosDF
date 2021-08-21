using AppCarnesDF.Models.Products;
using AppCarnesDF.ViewModels.Products;
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
    public partial class ProductDetail : ContentPage
    {
        private ProductDetailsViewModel context = new ProductDetailsViewModel();

        public ProductDetail(ProductModel product)
        {
            InitializeComponent();

            context.Product = product;
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