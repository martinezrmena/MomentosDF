using AppCarnesDF.Models.User;
using AppCarnesDF.ViewModels.CrearCuenta;
using AppCarnesDF.ViewModels.Tools;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms.MultiSelectListView;
using Xamarin.Forms.Xaml;

namespace AppCarnesDF.Views.Tools
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MultiSelectPicker : PopupPage
    {
        private MultiSelectPickerViewModel context { get; set; }

        public MultiSelectPicker(ObservableCollection<Sucursal> List, ObservableCollection<Sucursal> SelectedItems, CreateAccountViewModel viewModel)
        {
            InitializeComponent();
            context = new MultiSelectPickerViewModel(List, SelectedItems, viewModel);
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

        private void cvSucursales_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            try
            {
                var sucursal = (SelectableItem)e.SelectedItem;

                context.SucursalSeleccionada = (Sucursal)sucursal.Data;
            }
            catch (Exception ex)
            {

            }
        }
    }
}