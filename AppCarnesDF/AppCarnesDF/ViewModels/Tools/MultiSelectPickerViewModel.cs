using Acr.UserDialogs;
using AppCarnesDF.Helpers.Common;
using AppCarnesDF.Models.User;
using AppCarnesDF.Services.FontSize;
using AppCarnesDF.Services.User;
using AppCarnesDF.ViewModels.CrearCuenta;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.MultiSelectListView;

namespace AppCarnesDF.ViewModels.Tools
{
    public class MultiSelectPickerViewModel: BaseViewModel
    {
        #region Properties
        /// <summary>
        /// Propiedades del Picker para que el usuario establezca la sucursal
        /// </summary>

        private readonly FontSizeService FontService = new FontSizeService();
        public readonly LogMessageAttention Message = new LogMessageAttention();
        public CreateAccountViewModel ViewModelaAccount { get; set; }

        private MultiSelectObservableCollection<Sucursal> listsucursal;
        public MultiSelectObservableCollection<Sucursal> ListSucursal
        {
            get
            {
                return listsucursal;
            }
            set
            {

                listsucursal = value;
                OnPropertyChanged();
            }
        }

        private Sucursal sucursal;
        public Sucursal SucursalSeleccionada
        {
            get
            {
                return sucursal;
            }
            set
            {

                sucursal = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands
        public ICommand AceptarCommand { get; private set; }
        public ICommand CancelarCommand { get; private set; }
        #endregion

        public MultiSelectPickerViewModel(ObservableCollection<Sucursal> List,
                                          ObservableCollection<Sucursal> SelectedItems,
                                          CreateAccountViewModel viewModel)
        {
            try
            {

                ListSucursal = new MultiSelectObservableCollection<Sucursal>();

                foreach (var sucursales in List)
                {
                    ListSucursal.Add(sucursales);
                }
                AceptarCommand = new Command(async () => await AceptarEvent());
                CancelarCommand = new Command(async () => await CancelarEvent());

                //if (SelectedItems.Count > 0 && SelectedItems != null)
                //{
                //    foreach (var seleccionadas in SelectedItems)
                //    {
                //        ListSucursal.IsSelected(seleccionadas);
                //    }
                //}

                ViewModelaAccount = viewModel;
            }
            catch (Exception ex)
            {
                Task.Run(async () =>
                {
                    await Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
                });
            }

        }

        /// <summary>
        /// Metodo que sirve para actualizar el tamaño de la fuente.
        /// Debe consultarse el tamaño por la base de datos.
        /// </summary>
        public async Task UpdateFontSize()
        {
            try
            {
                SizeFonts = FontService.ConsultarFont();
                SizeFontsCookie = FontService.ConsultarFontCookie();
                SizeFontsOptima = FontService.ConsultarFontOptima();
                Message.SizeFonts = SizeFonts;
                Message.SizeFontsCookie = SizeFontsCookie;
                Message.SizeFontsOptima = SizeFontsOptima;
            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario ocurrió un error. Código 209.");
            }
        }

        private async Task CancelarEvent()
        {
            await PopupNavigation.Instance.PopAsync(true);
        }

        private async Task AceptarEvent()
        {
            try
            {
                //Es necesario enviar por MessageCenter los elementos de la lista seleccionada
                if (IsBusy)
                    return;

                UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);
                IsBusy = true;

                if (SucursalSeleccionada != null)
                {
                    if (ViewModelaAccount!= null)
                    {
                        ObservableCollection<Sucursal> sucursales = new ObservableCollection<Sucursal>();

                        sucursales.Add(SucursalSeleccionada);

                        await ViewModelaAccount.SetSucursales(sucursales);
                    }

                    IsBusy = false;
                    await PopupNavigation.Instance.PopAsync(true);
                }
                else
                {
                    IsBusy = false;
                    await Message.Failed("Estimado usuario, debe seleccionar una sucursal para proceder con la operación actual.");
                }
            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }
    }
}
