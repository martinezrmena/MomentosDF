using Acr.UserDialogs;
using AppCarnesDF.Helpers.Common;
using AppCarnesDF.Models.Ubicacion;
using AppCarnesDF.Models.User;
using AppCarnesDF.Services;
using AppCarnesDF.Services.FontSize;
using AppCarnesDF.Services.Permissions;
using AppCarnesDF.Services.Ubicacion;
using AppCarnesDF.Services.User;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppCarnesDF.ViewModels.MainPage
{
    public class MainPageViewModel: BaseViewModel
    {
        #region Properties
        private readonly FontSizeService FontService = new FontSizeService();

        public readonly LogMessageAttention Message = new LogMessageAttention();

        public Views.MainPage MainPageView { get; set; }

        private PickerCantonService pickerCanton = new PickerCantonService();
        private PickerProvinciaService pickerProvincia = new PickerProvinciaService();
        private PickerDistritoService pickerDistrito = new PickerDistritoService();
        private PickerSucursalService pickerSucursal = new PickerSucursalService();

        private List<DistritoEntity> Distritos { set; get; } 
        private List<ProvinciaEntity> Provincias { set; get; }
        private List<CantonEntity> Cantones { set; get; }
        public List<Sucursal> ListSucursal { set; get; }

        private readonly GeneralUtilities Utilities = new GeneralUtilities();
        #endregion

        #region Commands
        public ICommand IniciarSesionCommand { get; set; }

        public ICommand CrearCuentaCommand { get; set; }

        public ICommand ConocerMasCommand { get; set; }

        public ICommand HerramientasCommand { get; set; }
        #endregion

        public MainPageViewModel()
        {
            SizeFonts = FontService.ConsultarFont();
            IniciarSesionCommand = new Command(async() => await IniciarSesion());
            CrearCuentaCommand = new Command(CrearCuenta);
            ConocerMasCommand = new Command(async () => await ConocerMas());
            HerramientasCommand = new Command(async () => await Herramientas());
        }

        private async Task Herramientas()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);
                    await pantallas.showUpdateFontSize(MainPageView);
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

        private async Task ConocerMas()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);
                    if (await Utilities.VerifyInternetConnection())
                    {
                        await pantallas.showPlanLealtad(false);
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

        private async void CrearCuenta()
        {
            if (!IsBusy)
            {

                //AllowRegisterDataBase allowRegisterDataBase = new AllowRegisterDataBase();

                //var Values = allowRegisterDataBase.GetItemsAsync();
                //bool Acceso = Values == null ? true : Values.Any() ? Values.FirstOrDefault().Activated : true;

                Thread actividad = new Thread(new ThreadStart(async () =>
                {
                    try
                    {
                        IsBusy = true;
                        UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);

                        if (await Utilities.VerifyInternetConnection())
                        {
                            ///Eliminar las provincias y cantones desde el view model de create account
                            if (Provincias == null || Provincias.Count == 0)
                            {
                                Provincias = pickerProvincia.GetProvincias().Result.OrderBy(c => c.Value).ToList();
                            }

                            if (Cantones == null || Cantones.Count == 0)
                            {
                                Cantones = pickerCanton.GetCantones().Result.OrderBy(c => c.Value).ToList();
                            }

                            if (Distritos == null || Distritos.Count == 0)
                            {
                                Distritos = pickerDistrito.GetDistritos().Result.OrderBy(c => c.Value).ToList();
                            }

                            if (ListSucursal == null || ListSucursal.Count == 0)
                            {
                                ListSucursal = pickerSucursal.GetSucursales().Result.OrderBy(c => c.Descripcion_Centro).ToList();
                            }

                            if (Provincias != null && Cantones != null && Distritos != null && ListSucursal != null)
                            {
                                if (Provincias.Count > 0 && Cantones.Count > 0 && Distritos.Count > 0 && ListSucursal.Count > 0)
                                {
                                    pantallas.showCreateAccount(Message, Provincias, Cantones, Distritos, ListSucursal);
                                }
                            }
                            else
                            {
                                await Message.Failed("Estimado usuario, ocurrio un error, por favor verifique su conexión a internet.");
                            }
                        }

                        IsBusy = false;

                    }
                    catch (Exception ex)
                    {
                        await Message.Failed("Estimado usuario, ocurrio un error : " + ex.Message);
                    }
                    finally
                    {
                        UserDialogs.Instance.HideLoading();
                    }

                }));

                actividad.Start();
                //if (Acceso)
                //{

                //}
                //else
                //{
                //    await Message.Failed("Estimado usuario no posee permisos para acceder a esta funcionalidad. ");
                //}
            }
        }

        private async Task IniciarSesion()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);
                    await pantallas.showLogin();
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
            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario ocurrió un error. Código 209.");
            }
        }

    }
}
