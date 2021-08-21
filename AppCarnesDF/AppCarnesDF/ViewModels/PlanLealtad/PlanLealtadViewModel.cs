using AppCarnesDF.Helpers.Common;
using AppCarnesDF.Models.PlanLealtad;
using AppCarnesDF.Services.FontSize;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using System.Windows.Input;
using System.Threading;
using System.Collections.Generic;
using AppCarnesDF.Helpers;
using AppCarnesDF.Services;
using Acr.UserDialogs;

namespace AppCarnesDF.ViewModels.PlanLealtad
{
    public class PlanLealtadViewModel: BaseViewModel 
    {
        #region Properties
        private readonly FontSizeService FontService = new FontSizeService();
        public readonly LogMessageAttention Message = new LogMessageAttention();
        private WebApiService webApiService = new WebApiService();

        private ObservableCollection<PlanLealtadModel> planeslealtad;

        public ObservableCollection<PlanLealtadModel> PlanesLealtad
        {
            get { return planeslealtad; }
            set
            {

                planeslealtad = value;

                OnPropertyChanged();
            }
        }

        private PlanLealtadModel _selectedItem;

        public PlanLealtadModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != null)
                    _selectedItem.BackgroundColor = Color.FromHex(Colors.WhiteColor);

                _selectedItem = value;

                if (_selectedItem != null)
                    _selectedItem.BackgroundColor = Color.FromHex(Colors.Secondary);

                ChangeColors();

                OnPropertyChanged();

            }
        }

        private bool _cerrar;

        public bool Cerrar
        {
            get { return _cerrar; }
            set
            {
                _cerrar = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand ClosingCommand { get; set; }
        #endregion

        public PlanLealtadViewModel()
        {
            ClosingCommand = new Command(async()=> await Close(), ()=> !IsBusy);
        }

        #region Methods
        public async Task Close()
        {
            IsBusy = true;
            if (Cerrar)
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            await Application.Current.MainPage.Navigation.PopAsync();
            IsBusy = false;
        }

        private void ChangeColors()
        {
            ObservableCollection<PlanLealtadModel> valores = new ObservableCollection<PlanLealtadModel>();

            foreach (var item in PlanesLealtad)
            {
                if (item.TipoCliente == SelectedItem.TipoCliente)
                {
                    item.BackgroundColor = Color.FromHex(Colors.Secondary);
                }

                valores.Add(item);
            }

            PlanesLealtad = valores;

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

        public void Inicializar()
        {
            ObservableCollection<PlanLealtadModel> TempPlanesLealtad = new ObservableCollection<PlanLealtadModel>();

            Thread actividad = new Thread(new ThreadStart(async () =>
            {
                try
                {
                    Device.BeginInvokeOnMainThread(()=> UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading));

                    List<PlanLealtadModel> planesd = new List<PlanLealtadModel>();

                    planesd = await GetPlanes();

                    if (planesd != null)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            try
                            {
                                foreach (var item in planesd)
                                {
                                    TempPlanesLealtad.Add(item);
                                }

                                PlanesLealtad = TempPlanesLealtad;
                                if (PlanesLealtad.Count > 0)
                                {
                                    SelectedItem = PlanesLealtad.FirstOrDefault();
                                }
                            }
                            catch (Exception ex)
                            {
                                await Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    await Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
                }
                finally
                {
                    Device.BeginInvokeOnMainThread(() => UserDialogs.Instance.HideLoading());
                }
            }));

            actividad.Start();
        }

        /// <summary>
        /// Metodo que permite consultar via web service las promociones que se encuentran
        /// disponibles
        /// </summary>
        /// <returns></returns>
        public async Task<List<PlanLealtadModel>> GetPlanes()
        {

            // Definir el token de cancelación.
            CancellationTokenSource source = new CancellationTokenSource(new TimeSpan(0, 1, BrowserView.TimeOutLimit));
            CancellationToken token = source.Token;
            List<PlanLealtadModel> webPlanesLealtad = null;

            try
            {
                webPlanesLealtad = await webApiService.GetPlanesLealtad(token);
            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario, existen problemas de conexión con el servicio web actualmente, intentelo más tarde.");
            }

            return webPlanesLealtad;
        }
        #endregion

    }
}
