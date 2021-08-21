using Acr.UserDialogs;
using AppCarnesDF.Helpers;
using AppCarnesDF.Helpers.Common;
using AppCarnesDF.Models.User;
using AppCarnesDF.Services;
using AppCarnesDF.Services.FontSize;
using AppCarnesDF.Services.User;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppCarnesDF.ViewModels.Login
{
    public class LoginViewModel : BaseViewModel
    {
        #region Properties
        private readonly FontSizeService FontService = new FontSizeService();
        private readonly EmailService emailService = new EmailService();
        public readonly LogMessageAttention Message = new LogMessageAttention();
        private readonly Plantillas_Correo plantilla_Correo = new Plantillas_Correo();
        private readonly UserValidation userValidation = new UserValidation();
        private readonly WebApiService webApiService = new WebApiService();
        UserModel user = new UserModel();
        public string texthelpPassword = "La contraseña debe contener entre 6 y 50 caracteres, un numero, caracteres especiales y letras.";
        private ValidationString validationString = new ValidationString();

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands
        public ICommand ConocerMasCommand { get; private set; }
        public ICommand ForgotPasswordCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        #endregion

        public LoginViewModel()
        {
            ConocerMasCommand = new Command(async () => await AccessPlanLealtad());
            ForgotPasswordCommand = new Command(async () => await AccessForgotPassword());
            BackCommand = new Command(async ()=> await Close());
            validationString.SetMessage(Message);
        }

        private async Task Close()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
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
                validationString.SetMessage(Message);
            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario ocurrió un error. Código 209.");
            }
        }

        #region Access
        /// <summary>
        /// Metodo principal que permite acceder al menu prinicipal de la aplicación
        /// </summary>
        /// <param name="TipoAutenticacion"></param>
        /// <param name="user"></param>
        public async Task AccessMainMenu(string TipoAutenticacion, UserModel user)
        {
            try
            {
                await pantallas.showMainMenu(TipoAutenticacion, user);
            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
            }
        }

        /// <summary>
        /// Metodo que permite acceder a la pantalla informativa de plan de lealtad
        /// </summary>
        private async Task AccessPlanLealtad()
        {
            try
            {
                if (!IsBusy)
                {
                    UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);
                    IsBusy = true;
                    await pantallas.showPlanLealtad(false);
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

        private async Task AccessForgotPassword()
        {
            try
            {
                if (!IsBusy)
                {
                    UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);
                    IsBusy = true;
                    await pantallas.showForgotPassword();
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

        #region Web
        /// <summary>
        /// Permite extraer los parametros desde el web service
        /// </summary>
        public async Task GetUserFromService()
        {
            try
            {
                if (!IsBusy)
                {
                    // Definir el token de cancelación.
                    CancellationTokenSource source = new CancellationTokenSource(new TimeSpan(0, 1, 0));
                    CancellationToken token = source.Token;

                    IsBusy = true;
                    //encriptar contraseña
                    string encriptpassword = await validationString.Encriptar(Password);

                    //OPERACIONES de validacion para extraer al usuario desde web service
                    user = await webApiService.GetUser(token, Username, encriptpassword);

                    //user = await TableStorageService.GetUser(Username, encriptpassword, token);

                    if (user != null)
                    {
                        //Cerramos el login
                        await Application.Current.MainPage.Navigation.PopAsync();
                        //Acedemos al menu
                        await AccessMainMenu(TipoAutenticacion.NormalAutentication, user);

                        Thread actividad = new Thread(new ThreadStart(async () =>
                        {
                            //Validamos si la información del usuario esta completa
                            if (!await userValidation.ValidateUserModel(user))
                            {
                                //Si no esta completa
                                await Message.Failed("Estimado usuario, su información no está completa, por favor modifique sus datos en la opción 'Editar Perfil'.");
                            }
                        }));

                        actividad.Start();
                    }
                    else
                    {
                        //Si el usuario no existe
                        await Message.Failed("Estimado usuario, los datos proporcionados no existen, por favor cree su cuenta o comuníquese con el personal de la sucursal más cercana de Carnes Don Fernando.");
                    }

                    IsBusy = false;
                }
                
            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
            }
        }

        /// <summary>
        /// Metodo en encargado de enviar un email
        /// </summary>
        /// <param name="subject">Asunto</param>
        /// <param name="body">El contenido del email</param>
        /// <param name="recipient">el correo a quien se enviará</param>
        /// <returns></returns>
        public async Task SendEmail()
        {
            try
            {
                string body = plantilla_Correo.Mail_ChangePassword(Username);

                if (!string.IsNullOrEmpty(Username))
                {
                    if (await emailService.SendEmail("Recuperar contraseña", body, Username) > 0)
                    {
                        await Message.Successful("Se envio un correo a:" + Username + " con la información de contraseña temporal.");
                    }
                }
                else
                {
                    await Message.Failed("Necesita especificar un usuario para proporcionar una contraseña temporal");
                }
            }

            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
            }
        }

        #endregion

        internal async Task<bool> ValidateForm()
        {
            if (!await validationString.ValidateEmail(Username))
            {
                return false;
            }

            if (! await validationString.ValidatePassword(Password))
            {
                return false;
            }

            return true;
        }

    }
}
