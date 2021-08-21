using Acr.UserDialogs;
using AppCarnesDF.Helpers;
using AppCarnesDF.Helpers.Common;
using AppCarnesDF.Models.Sucursales;
using AppCarnesDF.Services;
using AppCarnesDF.Services.FontSize;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppCarnesDF.ViewModels.Sucursales
{
    public class SucursalDetailsViewModel : BaseViewModel
    {
        #region Properties
        private readonly FontSizeService FontService = new FontSizeService();
        public readonly LogMessageAttention Message = new LogMessageAttention();
        public readonly BrowserView browserView = new BrowserView();
        private GeneralUtilities GeneralUtilities = new GeneralUtilities();
        public Coordenadas coordenadas { get; set; }

        private SucursalesModel sucursaldetails;
        public SucursalesModel SucursalDetails
        {
            get { return sucursaldetails; }
            set
            {
                SetProperty(ref sucursaldetails, value);
            }
        }

        private ObservableCollection<PhoneNumber> telefonoslist;

        public ObservableCollection<PhoneNumber> TelefonosList
        {
            get { return telefonoslist; }
            set
            {
                telefonoslist = value;
                OnPropertyChanged();
            }
        }

        private bool phonevisible;
        public bool PhoneVisible
        {
            get { return phonevisible; }
            set
            {
                SetProperty(ref phonevisible, value);
            }
        }

        private bool restaurantevisible;
        public bool RestauranteVisible
        {
            get { return restaurantevisible; }
            set
            {
                SetProperty(ref restaurantevisible, value);
            }
        }

        private bool tiendavisible;
        public bool TiendaVisible
        {
            get { return tiendavisible; }
            set
            {
                SetProperty(ref tiendavisible, value);
            }
        }

        #endregion

        #region Commands
        public ICommand GoogleMapsCommand { get; set; }
        public ICommand WazeMapsCommand { get; set; }
        public ICommand BackCommand { get; private set; }
        #endregion

        public SucursalDetailsViewModel()
        {
            TelefonosList = new ObservableCollection<PhoneNumber>();
            GoogleMapsCommand = new Command(async()=> await GoogleMapsView());
            WazeMapsCommand = new Command(async() => await WazeMapsView());
            BackCommand = new Command(BackAction);
        }
        #region Methods

        public void ManageRestaurant() 
        {
            if (string.IsNullOrEmpty(SucursalDetails.HorarioRestaurante))
            {
                RestauranteVisible = false;
            }
            else 
            {
                RestauranteVisible = true;
            }
        }

        public void ManageTienda() 
        {
            if (string.IsNullOrEmpty(SucursalDetails.HorarioTienda))
            {
                TiendaVisible = false;
            }
            else
            {
                TiendaVisible = true;
            }
        }

        public void FillPhoneNumberList()
        {
            foreach (string splitstring in Regex.Split(SucursalDetails.Telefonos, Simbol._comma))
            {
                if (!string.IsNullOrEmpty(splitstring))
                {
                    TelefonosList.Add(new PhoneNumber(splitstring));
                }
            }

            if (TelefonosList.Count > 0)
            {
                PhoneVisible = true;
            }
            else
            {
                PhoneVisible = false;
            }
        }

        private async void BackAction()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        /// <summary>
        /// Metodo que permite abrir una pestaña del navegador para mostrar
        /// la ubicación en la aplicación google maps
        /// </summary>
        /// <returns></returns>
        private async Task GoogleMapsView()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);
                if (await GeneralUtilities.VerifyInternetConnection())
                {
                    var request = string.Format(SucursalDetails.EnlaceGoogleMaps);
                    await browserView.OpenBrowser(new Uri(request));
                }
            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario en este momento no es posible visualizar la locación del establecimiento.");
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        /// <summary>
        /// Metodo que permite abrir una pestaña del navegador para mostrar
        /// la ubicación en la aplicación waze
        /// </summary>
        /// <returns></returns>
        private async Task WazeMapsView()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);
                if (await GeneralUtilities.VerifyInternetConnection())
                {
                    var request = string.Format(SucursalDetails.EnlaceWaze);
                    await browserView.OpenBrowser(new Uri(request));
                }
            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario en este momento no es posible visualizar la locación del establecimiento.");
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
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

        public async Task PhoneCall(string number)
        {
            try
            {
                UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);
                PhoneDialer.Open(number);
            }
            catch (ArgumentNullException anEx)
            {
                // Number was null or white space
                await Message.Failed("El número proporcionado estaba en blanco.");
            }
            catch (FeatureNotSupportedException ex)
            {
                // Phone Dialer is not supported on this device.
                await Message.Failed("La marcación automatica no es una caracteristica soportada por el dispositivo actual. " + ex.Message);
            }
            catch (Exception ex)
            {
                // Other error has occurred.
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
