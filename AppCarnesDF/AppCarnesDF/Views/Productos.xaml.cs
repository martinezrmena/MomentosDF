using AppCarnesDF.Helpers.Common;
using AppCarnesDF.ViewModels.Products;
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
    public partial class Productos : ContentPage
    {
        #region Properties
        private ProductsViewModel context = new ProductsViewModel();
        #endregion

        public Productos()
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
                if (context.Products == null)
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

    }
}