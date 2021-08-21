using Acr.UserDialogs;
using AppCarnesDF.Helpers;
using AppCarnesDF.Helpers.Common;
using AppCarnesDF.Models.Notificaciones;
using AppCarnesDF.Models.Share;
using AppCarnesDF.Models.Ubicacion;
using AppCarnesDF.Models.User;
using AppCarnesDF.Services;
using AppCarnesDF.Services.CompartirCodigo;
using AppCarnesDF.Services.FontSize;
using AppCarnesDF.Services.Notificaciones;
using AppCarnesDF.Services.Ubicacion;
using AppCarnesDF.Services.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppCarnesDF.ViewModels.Configuracion
{
    public class ConfiguracionViewModel : BaseViewModel
    {
        #region Properties
        private readonly FontSizeService FontService = new FontSizeService();

        private readonly GeneralUtilities Utilities = new GeneralUtilities();

        public ValidationString validationString = new ValidationString();

        public LogMessageAttention Message = new LogMessageAttention();

        private NotificacionesService servicio = new NotificacionesService();
        private ShareService servicioSh = new ShareService();
        private PickerCantonService pickerCanton = new PickerCantonService();
        private PickerDistritoService pickerDistrito = new PickerDistritoService();
        private PickerProvinciaService pickerProvincia = new PickerProvinciaService();
        private PickerSucursalService pickerSucursal = new PickerSucursalService();
        private List<ProvinciaEntity> Provincias { set; get; }
        private List<CantonEntity> Cantones { set; get; }

        private List<DistritoEntity> Distritos { set; get; }
        public List<Sucursal> ListSucursal { set; get; }
        public bool Modify { set; get; } = false;

        private bool tgnotificaciones;
        public bool tgNotificaciones
        {
            get { return tgnotificaciones; }
            set
            {
                tgnotificaciones = value;
            }
        }

        private bool compartir;
        public bool Compartir
        {
            get { return compartir; }
            set
            {
                compartir = value;
            }
        }

        private string id;
        public string ID
        {
            get { return id; }
            set
            {

                id = value;
            }
        }

        private UserModel usuario;

        public UserModel UserAutenticated
        {
            get { return usuario; }
            set
            {
                usuario = value;
                OnPropertyChanged();
            }
        }

        private string username;

        public string UserName
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged();
            }
        }

        #region Commands
        public ICommand EditarPerfilCommand { get; private set; }
        public ICommand CompartirCodigoCommand { get; private set; }
        public ICommand ServicioClienteCommand { get; private set; }
        public ICommand SucursalesCommand { get; private set; }
        public ICommand AcercaDeCommand { get; private set; }
        public ICommand PoliticaPrivacidadCommand { get; private set; }
        public ICommand PlanLealtadCommand { get; private set; }
        public ICommand ClosingCommand { get; private set; }
        public ICommand ClosingSession { get; private set; }
        #endregion

        #endregion

        public ConfiguracionViewModel(UserModel user)
        {

            UserAutenticated = user;

            UserName = user.Nombre + " " + user.Apellido;

            #region Commands
            EditarPerfilCommand = new Command(EditarPerfil);
            CompartirCodigoCommand = new Command(async () => await CompartirCodigo());
            ServicioClienteCommand = new Command(async () => await ServicioCliente());
            SucursalesCommand = new Command(async () => await Sucursales());
            AcercaDeCommand = new Command(async () => await AcercaDe());
            PoliticaPrivacidadCommand = new Command(async () => await PoliticaPrivacidad());
            PlanLealtadCommand = new Command(async () => await PlanLealtad());
            ClosingCommand = new Command(async () => await Closing());
            ClosingSession = new Command(async () => await ConfirmClosing());
            #endregion

            //Consultamos si ya hay algun tipo de datos almacenados en BD
            #region Notificaciones
            var modelo = servicio.Consultar().FirstOrDefault();
            if (modelo != null)
            {
                Modify = true;
                tgNotificaciones = modelo.Activated;
                ID = modelo.Id;
            }
            else
            {
                tgNotificaciones = true;
                GuardarInit();
            }
            #endregion

            IniciarCompartir();
        }

        public void IniciarCompartir() 
        {
            var model = servicioSh.Consultar().Where(x => x.Id == UserAutenticated.Cedula).FirstOrDefault();
            if (model != null)
            {
                Compartir = model.Saved;
            }
            else
            {
                Task.Run(async () =>
                {
                    await ConsultarWebShare();
                });
            }
        }

        /// <summary>
        /// Metodo que permite actualizar la bd local en caso de que se posean datos via web
        /// </summary>
        /// <returns></returns>
        private async Task ConsultarWebShare() 
        {
            try
            {
                UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);
                ShareModel modelshare = null;

                if (Utilities.VerifyInternetConnection2())
                {
                    modelshare = await servicioSh.GetShare(UserAutenticated.Cedula);
                }

                if (modelshare != null)
                {
                    Compartir = modelshare.Saved;
                    servicioSh.Guardar(modelshare);
                }
                else
                {
                    Compartir = false;
                }
            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario, ocurrio un error: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        public async Task ConfirmClosing()
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;

                UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);

                Message.attributes.Message = "¿Está seguro que desea cerrar la sesión?";

                await pantallas.showConfirmationMessage(Message.GetMessageAttributes(), this);

                IsBusy = false;

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

        public  async Task CloseSession(bool result) 
        {
            if (result)
            {
                await Message.generalAttention("Estimado usuario, muchas gracias por utilizar Momentos Don Fernando, le esperamos para compartir más momentos juntos.");
                await Closing();
                await Closing();
            }
        }

        public async void GuardarInit()
        {
            try
            {
                if (IsBusy)
                    return;

                //Es necesario desactivar/reactivar las notificaciones de IOS
                var ConfiguracionNotificaciones = DependencyService.Get<IHandleLocalNotification>();
                if (tgNotificaciones)
                {
                    ConfiguracionNotificaciones.ReceiveNotification();
                }
                else 
                {
                    ConfiguracionNotificaciones.NotReceiveNotification();
                }


                if (!string.IsNullOrEmpty(ID))
                {
                    Modificar();
                    await Message.Successful("La configuración ha sido guardada con éxito.");
                }
                else
                {
                    Guardar();
                }
            }
            catch (Exception ex)
            {
                await Message.Failed("Ocurrio el siguiente error: " + ex.Message);
            }

        }

        private async Task Closing()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        #region Redireccionamientos
        private void EditarPerfil()
        {
            if (!IsBusy)
            {
                Thread actividad = new Thread(new ThreadStart(async () =>
                {
                    try
                    {
                        IsBusy = true;
                        UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);

                        if (await Utilities.VerifyInternetConnection())
                        {
                            ///Eliminar las provincias y cantones desde el view model de create account
                            if (Provincias == null)
                            {
                                Provincias = pickerProvincia.GetProvincias().Result.OrderBy(c => c.Value).ToList();
                            }

                            if (Cantones == null)
                            {
                                Cantones = pickerCanton.GetCantones().Result.OrderBy(c => c.Value).ToList();
                            }

                            if (Distritos == null)
                            {
                                Distritos = pickerDistrito.GetDistritos().Result.OrderBy(c => c.Value).ToList();
                            }

                            if (ListSucursal == null)
                            {
                                ListSucursal = pickerSucursal.GetSucursales().Result.OrderBy(c => c.Descripcion_Centro).ToList();
                            }

                            if (Provincias != null && Cantones != null && Distritos != null && ListSucursal != null)
                            {
                                Device.BeginInvokeOnMainThread(async() =>
                                {
                                    await Closing();
                                });
                                
                                pantallas.showCreateAccount(UserAutenticated, Message, Provincias, Cantones, Distritos, ListSucursal);
                            }
                            else
                            {
                                await Message.Failed("Estimado usuario, ocurrió un error, por favor verifique su conexión a internet.");
                            }
                        }

                        IsBusy = false;

                    }
                    catch (Exception ex)
                    {
                        await Message.Failed("Estimado usuario ocurrió un error durante el proceso, por favor intente más tarde.");
                    }
                    finally
                    {
                        UserDialogs.Instance.HideLoading();
                    }

                }));

                actividad.Start();
            }
        }

        private async Task CompartirCodigo()
        {
            try
            {
                if (!IsBusy)
                {
                    UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);
                    IsBusy = true;

                    //Si no se ha compartido el codigo
                    if (!Compartir)
                    {
                        if (await Utilities.VerifyInternetConnection())
                        {
                            await Closing();
                            await pantallas.showCompartirCodigo(UserAutenticated, this);
                        }
                    }
                    else 
                    {
                        await Message.generalAttention("Estimado usuario, la acción para compartir código ha sido generada con anterioridad, la misma puede efectuarse una única vez. Gracias por compartir Momentos Don Fernando.");
                    }

                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario ocurrió un error durante el proceso, por favor intentelo más tarde.");
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }

        }

        private async Task ServicioCliente()
        {
            try
            {
                if (!IsBusy)
                {
                    UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);
                    IsBusy = true;
                    if (await validationString.ValidateEmail(UserAutenticated.Email))
                    {
                        await Closing();
                        await pantallas.showServicioCliente(UserAutenticated);
                    }
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

        private async Task Sucursales()
        {
            try
            {
                if (!IsBusy)
                {
                    UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);
                    IsBusy = true;
                    if (await Utilities.VerifyInternetConnection())
                    {
                        await Closing();
                        await pantallas.showSucursales();
                    }
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

        private async Task AcercaDe()
        {
            try
            {
                if (!IsBusy)
                {
                    UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);
                    IsBusy = true;
                    if (await Utilities.VerifyInternetConnection())
                    {
                        await Closing();
                        await pantallas.showAcercaDe();
                    }
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

        private async Task PoliticaPrivacidad()
        {
            try
            {
                if (!IsBusy)
                {
                    UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);
                    IsBusy = true;
                    if (await Utilities.VerifyInternetConnection())
                    {
                        await Closing();
                        await pantallas.showPoliticaPrivacidad();
                    }
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

        private async Task PlanLealtad()
        {
            try
            {
                if (!IsBusy)
                {
                    UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);
                    IsBusy = true;
                    if (await Utilities.VerifyInternetConnection())
                    {
                        await pantallas.showPlanLealtad(true);
                    }
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
                Utilities.Message = Message;
                validationString.SetMessage(Message);
            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario ocurrió un error. Código 209.");
            }
        }

        public async Task ShowSucursales()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);
                await pantallas.showSucursal();
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

        private void Guardar()
        {
            IsBusy = true;

            Guid IdFont = Guid.NewGuid();

            ID = IdFont.ToString();

            NotificacionModel modelo = new NotificacionModel()
            {
                Activated = tgNotificaciones,
                Id = ID
            };

            servicio.Guardar(modelo);

            IsBusy = false;
        }

        private void Modificar()
        {
            IsBusy = true;

            NotificacionModel modelo = new NotificacionModel()
            {
                Activated = tgNotificaciones,
                Id = ID
            };

            servicio.Modificar(modelo);

            IsBusy = false;
        }

    }
}
