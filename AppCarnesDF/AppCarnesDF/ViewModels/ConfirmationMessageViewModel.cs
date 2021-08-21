using AppCarnesDF.Models;
using AppCarnesDF.ViewModels.Configuracion;
using AppCarnesDF.ViewModels.CrearCuenta;
using AppCarnesDF.ViewModels.Menu;
using AppCarnesDF.ViewModels.Users;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppCarnesDF.ViewModels
{
    /// <summary>
    /// Clase encargada de manejar el envio de confirmación por parte del usuario
    /// </summary>
    public class ConfirmationMessageViewModel : MessageAttributes
    {
        #region Properties

        public MessageReceived MensajeEnviado = new MessageReceived();

        public CreateAccountViewModel AccountViewModel { get; set; }

        public MenuViewModel MenuViewModel { get; set; }

        public ConfiguracionViewModel ConfiguracionViewModel { get; set; }

        public CompartirCodigoViewModel CompartirCodigoViewModel { get; set; }

        public int Value { get; set; }

        public RecoverPasswordViewModel RecoverPasswordViewModel { get; set; }
        #endregion

        #region Commands
        public Command AceptarCommand { get; set; }

        public Command CancelarCommand { get; set; }
        #endregion

        #region Constructores
        public ConfirmationMessageViewModel(MessageAttributes attributes, RecoverPasswordViewModel viewModelRecoverPassword)
        {
            Iniciar(attributes);
            RecoverPasswordViewModel = viewModelRecoverPassword;
        }

        public ConfirmationMessageViewModel(MessageAttributes attributes, CompartirCodigoViewModel viewModelCompartirCodigo, int value)
        {
            Iniciar(attributes);
            CompartirCodigoViewModel = viewModelCompartirCodigo;
            Value = value;
        }

        public ConfirmationMessageViewModel(MessageAttributes attributes, ConfiguracionViewModel viewModelConfiguracion)
        {
            Iniciar(attributes);
            ConfiguracionViewModel = viewModelConfiguracion;
        }

        public ConfirmationMessageViewModel(MessageAttributes attributes, CreateAccountViewModel viewModelAccount)
        {
            Iniciar(attributes);
            AccountViewModel = viewModelAccount;
        }

        public ConfirmationMessageViewModel(MessageAttributes attributes, MenuViewModel viewModelMenu)
        {
            Iniciar(attributes);
            MenuViewModel = viewModelMenu;
        }
        #endregion

        public void Iniciar(MessageAttributes attributes)
        {
            Title = attributes.Title;
            Message = attributes.Message;
            SizeFontsCookie = attributes.SizeFontsCookie;
            SizeFontsOptima = attributes.SizeFontsOptima;
            SizeFonts = attributes.SizeFonts;
            CancelButtonText = attributes.CancelButtonText;
            ButtonText = attributes.ButtonText;
            AceptarCommand = new Command(async () => await AceptarAction());
            CancelarCommand = new Command(async () => await CancelarAction());
            IsBusy = false;
        }

        private async Task CancelarAction()
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;

                await PopupNavigation.Instance.PopAsync();

                IsBusy = false;
            }
            catch (Exception ex)
            {

            }
        }

        private async Task AceptarAction()
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;

                MensajeEnviado.Respuesta = true;

                await PopupNavigation.Instance.PopAsync();

                if (AccountViewModel != null)
                {
                    await AccountViewModel.CrearCuenta(true);
                }
                else if (MenuViewModel != null)
                {
                    await MenuViewModel.Closing(true);
                }
                else if (ConfiguracionViewModel != null)
                {
                    await ConfiguracionViewModel.CloseSession(true);
                }
                else if (CompartirCodigoViewModel != null)
                {
                    if (Value > 0)
                    {
                        await CompartirCodigoViewModel.CopiarCodigo();
                    }
                    else
                    {
                        await CompartirCodigoViewModel.CompartirCodigo();
                    }
                }
                else if (RecoverPasswordViewModel != null) 
                {
                    await RecoverPasswordViewModel.SendEmail();
                }

                IsBusy = false;
            }
            catch (Exception ex)
            {

            }
            

        }
    }
}