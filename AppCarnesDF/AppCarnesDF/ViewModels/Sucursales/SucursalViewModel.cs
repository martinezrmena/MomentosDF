using Acr.UserDialogs;
using AppCarnesDF.Helpers;
using AppCarnesDF.Helpers.Common;
using AppCarnesDF.Models.Sucursales;
using AppCarnesDF.Services;
using AppCarnesDF.Services.FontSize;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppCarnesDF.ViewModels.Sucursales
{
    public class SucursalViewModel : BaseViewModel
    {
        #region Fields
        private ObservableCollection<object> _Items;
        private ObservableCollection<object> _ItemsFiltered;
        private ObservableCollection<object> _ItemsUnfiltered;
        private string _searchText;
        #endregion

        #region Properties
        private readonly FontSizeService FontService = new FontSizeService();

        private readonly WebApiService webApiService = new WebApiService();

        public readonly LogMessageAttention Message = new LogMessageAttention();

        private SucursalesModel _selectedItem;

        public SucursalesModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (value != null)
                {
                    _selectedItem = value;

                    Task.Run(() => 
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await ItemSelectedChanged();
                        });
                    });
                }
            }
        }
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged("SearchText");
            }
        }
        public ObservableCollection<object> Items
        {
            get { return _Items; }
            set
            {
                _Items = value;
                OnPropertyChanged("Items");
            }
        }
        #endregion

        #region Commands
        public ICommand SearchCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        #endregion

        public SucursalViewModel()
        {
            Items = new ObservableCollection<object>();

            Thread hilo = new Thread(()=> 
            {
                GetItems();
            });

            hilo.Start();

            SearchCommand = new Command(PerformSearch);
            BackCommand = new Command(BackAction);
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

        #region Methods
        public void PerformSearch()
        {
            if (string.IsNullOrWhiteSpace(this._searchText))
                Items = _ItemsUnfiltered;
            else
            {
                _ItemsFiltered = new ObservableCollection<object>(_ItemsUnfiltered.Where(i =>
               (i is SucursalesModel && (((SucursalesModel)i).Nombre.ToLower().Contains(_searchText.ToLower())))));
                Items = _ItemsFiltered;
            }
        }

        private async void GetItems()
        {
            // Definir el token de cancelación.
            CancellationTokenSource source = new CancellationTokenSource(new TimeSpan(0, 1, BrowserView.TimeOutLimit));
            CancellationToken token = source.Token;
            List<SucursalesModel> data = null;

            try
            {
                _ItemsUnfiltered = _Items;

                data = await webApiService.GetSucursalesData(token);

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        _ItemsUnfiltered.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                await Message.Failed("Estimado usuario, existen problemas de conexión con el servicio web actualmente, intentelo más tarde.");
            }
        }

        private async void BackAction()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        /// <summary>
        /// Permite mostrar los detalles de las sucursales
        /// </summary>
        private async Task ItemSelectedChanged()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);
                    SelectedItem.HorarioRestaurante = SelectedItem.HorarioRestaurante != null ? SelectedItem.HorarioRestaurante.Replace(Simbol._comma, Environment.NewLine) : string.Empty;
                    SelectedItem.HorarioTienda = SelectedItem.HorarioTienda != null ? SelectedItem.HorarioTienda.Replace(Simbol._comma, Environment.NewLine): string.Empty;
                    await pantallas.showSucursalesDetails(SelectedItem);
                    IsBusy = false;
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
        #endregion

    }
}
