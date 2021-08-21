using AppCarnesDF.Helpers.Common;
using AppCarnesDF.Models;
using AppCarnesDF.Models.ActividadReciente;
using AppCarnesDF.Models.Products;
using AppCarnesDF.Models.Sucursales;
using AppCarnesDF.Models.Ubicacion;
using AppCarnesDF.Models.User;
using AppCarnesDF.Services;
using AppCarnesDF.ViewModels.Configuracion;
using AppCarnesDF.ViewModels.CrearCuenta;
using AppCarnesDF.ViewModels.Menu;
using AppCarnesDF.ViewModels.Users;
using AppCarnesDF.Views;
using AppCarnesDF.Views.ConfiguracionDetails;
using AppCarnesDF.Views.Tools;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Enums;
using Rg.Plugins.Popup.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppCarnesDF.Helpers
{
    /// <summary>
    /// Clase encargada de manejar todos los redireccionamientos de pantallas
    /// </summary>
    public class CarnesDF
    {
        public async Task showMainMenu(string TipoAutenticacion, UserModel userAutenticated)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new MainMenu(TipoAutenticacion, userAutenticated));
        }

        #region ConfirmMessage
        public async Task showConfirmationMessage(MessageAttributes attributes, RecoverPasswordViewModel viewModel)
        {
            await PopupNavigation.Instance.PushAsync(new ConfirmationMessage(attributes, viewModel));
        }

        public async Task showConfirmationMessage(MessageAttributes attributes, CompartirCodigoViewModel viewModel, int value)
        {
            await PopupNavigation.Instance.PushAsync(new ConfirmationMessage(attributes, viewModel, value));
        }

        public async Task showConfirmationMessage(MessageAttributes attributes, ConfiguracionViewModel viewModel)
        {
            await PopupNavigation.Instance.PushAsync(new ConfirmationMessage(attributes, viewModel));
        }

        public async Task showConfirmationMessage(MessageAttributes attributes, CreateAccountViewModel viewModel)
        {
            await PopupNavigation.Instance.PushAsync(new ConfirmationMessage(attributes, viewModel));
        }

        public async Task showConfirmationMessage(MessageAttributes attributes, MenuViewModel viewModel)
        {
            await PopupNavigation.Instance.PushAsync(new ConfirmationMessage(attributes, viewModel));
        }
        #endregion

        public void showCreateAccount(LogMessageAttention Message, 
                                            List<ProvinciaEntity> Provincias,
                                            List<CantonEntity> Cantones,
                                            List<DistritoEntity> Distritos,
                                            List<Sucursal> ListSucursal)
        {
            Device.BeginInvokeOnMainThread(async () => {
                await Application.Current.MainPage.Navigation.PushAsync(
                            new CrearCuenta(ActionValues.CrearCuenta,
                                            Provincias,
                                            Cantones,
                                            Distritos,
                                            ListSucursal));
            });
        }

        public void showCreateAccount(UserModel user,
                                      LogMessageAttention Message,
                                      List<ProvinciaEntity> Provincias,
                                      List<CantonEntity> Cantones,
                                      List<DistritoEntity> Distritos,
                                      List<Sucursal> ListSucursal)
        {
            Device.BeginInvokeOnMainThread(async () => {
                await Application.Current.MainPage.Navigation.PushAsync(
                            new CrearCuenta(user,
                                            ActionValues.ModificarUsuario,
                                            Provincias,
                                            Cantones,
                                            Distritos,
                                            ListSucursal));
            });
        }

        public async Task showLogin()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new Login());
        }

        public async Task showConfiguration(UserModel user)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new Configuracion(user));
        }

        public async Task showActividadReciente(UserModel user, UserViewModel viewModel, List<ActividadRecienteModel> Actividades)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ActividadReciente(user, viewModel, Actividades));
        }

        public async Task showUpdateFontSize()
        {
            var animationPopup = new UpdateFontSizes();

            var scaleAnimation = new ScaleAnimation
            {
                PositionIn = MoveAnimationOptions.Right,
                PositionOut = MoveAnimationOptions.Right,
                ScaleIn = 1.2,
                ScaleOut = 0.8,
                DurationIn = 1000,
                DurationOut = 1000,
                EasingIn = Easing.CubicIn,
                EasingOut = Easing.CubicOut,
                HasBackgroundAnimation = false
            };

            animationPopup.Animation = scaleAnimation;
            await PopupNavigation.Instance.PushAsync(animationPopup);
        }

        public async Task showUpdateFontSize(MainPage mainPage)
        {
            var animationPopup = new UpdateFontSizes(mainPage);

            var scaleAnimation = new ScaleAnimation
            {
                PositionIn = MoveAnimationOptions.Right,
                PositionOut = MoveAnimationOptions.Right,
                ScaleIn = 1.2,
                ScaleOut = 0.8,
                DurationIn = 1000,
                DurationOut = 1000,
                EasingIn = Easing.CubicIn,
                EasingOut = Easing.CubicOut,
                HasBackgroundAnimation = false
            };

            animationPopup.Animation = scaleAnimation;
            await PopupNavigation.Instance.PushAsync(animationPopup);
        }

        public async Task showSucursalesPicker(ObservableCollection<Sucursal> List, ObservableCollection<Sucursal> SelectedItems, CreateAccountViewModel viewModel)
        {
            await PopupNavigation.Instance.PushAsync(new MultiSelectPicker(List, SelectedItems, viewModel));
        }

        #region Configuracion Details

        public async Task showProductDetail(ProductModel producto)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ProductDetail(producto));
        }

        public async Task showCompartirCodigo(UserModel user, ConfiguracionViewModel viewModel)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new CompartirCodigo(user, viewModel));
        }

        public async Task showServicioCliente(UserModel userModel)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ServicioCliente(userModel));
        }

        public async Task showSucursales()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new Sucursales());
        }

        public async Task showSucursalesDetails(SucursalesModel sucursalesDetails)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new SucursalDetails(sucursalesDetails));
        }

        public async Task showAcercaDe()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new About());
        }

        public async Task showPoliticaPrivacidad()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new PoliticaPrivacidad());
        }

        public async Task showPlanLealtad(bool cerrar)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new PlanLealtad(cerrar));
        }

        public async Task showForgotPassword()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new RecuperarPassword());
        }

        public async Task showSucursal()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new Sucursales());
        }
        #endregion

        public async Task CerrarPantalla()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
