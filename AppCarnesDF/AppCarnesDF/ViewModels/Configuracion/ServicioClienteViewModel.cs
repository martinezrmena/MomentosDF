using Acr.UserDialogs;
using AppCarnesDF.Helpers;
using AppCarnesDF.Helpers.Common;
using AppCarnesDF.Models.User;
using AppCarnesDF.Services;
using AppCarnesDF.Services.FontSize;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppCarnesDF.ViewModels.Configuracion
{
    public class ServicioClienteViewModel: BaseViewModel
    {
        #region Properties
        private CancellationTokenSource source = new CancellationTokenSource(new TimeSpan(0, 1, 0));
        private CancellationToken token;
        public EmailService emailService = new EmailService();
        private Plantillas_Correo Plantillas = new Plantillas_Correo();
        private readonly FontSizeService FontService = new FontSizeService();

        public LogMessageAttention Message = new LogMessageAttention();

        private GeneralUtilities Utilities = new GeneralUtilities();

        private string asunto;
        public string Asunto
        {
            get { return asunto; }
            set
            {
                SetProperty(ref asunto, value);
            }
        }

        private string mensaje;
        public string Mensaje
        {
            get { return mensaje; }
            set
            {
                SetProperty(ref mensaje, value);
            }
        }

        private ValidationString valdation = new ValidationString();
        private WebApiService webApiService = new WebApiService();

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
        #endregion

        #region Commands
        public ICommand ClosingCommand { get; set; }
        public ICommand EnviarCommand { get; set; }
        #endregion

        public ServicioClienteViewModel()
        {
            ClosingCommand = new Command(async ()=> await Close());
            EnviarCommand = new Command(async()=> await Enviar(), () => !IsBusy);
        }

        #region Methods
        private async Task Close()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async Task Enviar()
        {
            try
            {
                IsBusy = true;

                UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);

                if (await ValidForm())
                {
                    WebApiService webApiService = new WebApiService();
                    if (emailService.Parametizaciones == null)
                    {
                        var parametizaciones = await webApiService.GetParametizaciones(token);
                        //parametizaciones.Pass = await valdation.Desencriptar(parametizaciones.Pass);
                        emailService.SetAtributes(parametizaciones);
                    }

                    //pendiente definir a quien enviar el correo
                    if (await webApiService.SendEmailApi(emailService.Parametizaciones.User,
                                                         emailService.Parametizaciones.User,
                                                         Asunto,
                                                         Mensaje,
                                                         emailService.Parametizaciones.Client,
                                                         emailService.Parametizaciones.Port,
                                                         emailService.Parametizaciones.Pass) != 1)
                    {
                        await Message.Failed("Estimado usuario, la operación no pudo ser completada, por favor intente más tarde.");
                    }
                    else
                    {
                        await Message.Successful("Estimado usuario, el envío del correo se realizó correctamente, gracias por utilizar Momentos Don Fernando.");
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }
                }

                IsBusy = false;
            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario por el momento nuestro servicio de servicio al cliente no está disponible, intente más tarde.");
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private async Task<bool> ValidForm()
        {
            if (string.IsNullOrEmpty(Asunto))
            {
                Asunto = string.Empty;
                await Message.Failed("Estimado usuario, el asunto es requerido para poder continuar con la operación actual.");
                return false;
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                Mensaje = string.Empty;
                await Message.Failed("Estimado usuario, el mensaje es requerido para poder continuar con la operación actual.");
                return false;
            }

            if (!await Utilities.VerifyInternetConnection())
            {
                return false;
            }

            return true;
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
                await Message.Failed("Estimado usuario ocurrió un error: Código 209.");
            }
        }
        #endregion
    }
}
