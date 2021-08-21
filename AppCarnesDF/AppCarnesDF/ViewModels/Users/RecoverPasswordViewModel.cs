using Acr.UserDialogs;
using AppCarnesDF.Helpers;
using AppCarnesDF.Helpers.Common;
using AppCarnesDF.Services;
using AppCarnesDF.Services.FontSize;
using AppCarnesDF.Services.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppCarnesDF.ViewModels.Users
{

    public class RandomPasswordEx 
    {
        /// <summary>
        /// Random declaration must be done outside the method to actually generate random numbers
        /// </summary>
        private static readonly Random Random = new Random();

        /// <summary>
        /// Generate passwords
        /// </summary>
        /// <param name="passwordLength"></param>
        /// <param name="strongPassword"> </param>
        /// <returns></returns>
        public static string PasswordGenerator(int passwordLength, bool strongPassword)
        {
            int seed = Random.Next(1, int.MaxValue);
            //const string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            const string specialCharacters = "!#$%&'()*+,-.:;<=>?[]_";

            var chars = new char[passwordLength];
            var rd = new Random(seed);

            for (var i = 0; i < passwordLength; i++)
            {
                // If we are to use special characters
                if (strongPassword && i % Random.Next(3, passwordLength) == 0)
                {
                    chars[i] = specialCharacters[rd.Next(0, specialCharacters.Length)];
                }
                else
                {
                    chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
                }
            }

            return new string(chars);
        }

        public static string PasswordGeneratorNumbers(int passwordLength)
        {
            int seed = Random.Next(1, int.MaxValue);
            const string allowedChars = "0123456789";

            var chars = new char[passwordLength];
            var rd = new Random(seed);

            for (var i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        public static string PasswordGeneratorSpecialCharacter(int passwordLength, bool strongPassword)
        {
            int seed = Random.Next(1, int.MaxValue);
            //const string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            const string allowedChars = "!#$%&'()*+,-.:;<=>?[]_";

            var chars = new char[passwordLength];
            var rd = new Random(seed);

            for (var i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        public static string GenerateConfidentialPassword(int passwordLength, bool strongPassword) 
        {
            string password = string.Empty;

            password += PasswordGenerator(passwordLength, strongPassword);
            password += PasswordGeneratorSpecialCharacter(2, strongPassword);
            password += PasswordGeneratorNumbers(2);

            return password;
        }
    }

    public class RecoverPasswordViewModel : BaseViewModel
    {

        #region Properties
        private FontSizeService FontService = new FontSizeService();
        public LogMessageAttention Message = new LogMessageAttention();
        public string Email { get; set; }
        public EmailService emailService = new EmailService();
        public string NewPassword { get; set; }
        private Plantillas_Correo Plantillas = new Plantillas_Correo();
        private ValidationString valdation = new ValidationString();
        private WebApiService webApiService = new WebApiService();
        private CancellationTokenSource source = new CancellationTokenSource(new TimeSpan(0, 1, 0));
        private CancellationToken token;
        private GeneralUtilities Utilities = new GeneralUtilities();
        public TableStorageService tableservice = new TableStorageService();
        #endregion

        #region Commands
        public ICommand SendEmailCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        #endregion

        public RecoverPasswordViewModel()
        {
            SendEmailCommand = new Command(async () => await ConfirmSendEmail(), () => !IsBusy);
            CancelarCommand = new Command(async () => await Cancelar(), () => !IsBusy);
            NewPassword = RandomPasswordEx.GenerateConfidentialPassword(8, true);
            token = source.Token;
        }

        private async Task Cancelar()
        {
            try
            {
                IsBusy = true;
                await pantallas.CerrarPantalla();
                IsBusy = false;
            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
            }
        }

        private async Task ConfirmSendEmail() 
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;

                UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);

                Message.attributes.Message = "¿Está seguro de que desea continuar con la operación?";

                //Validamos que el email contenga el formato correcto
                if (await valdation.ValidateEmail(Email))
                {
                    await pantallas.showConfirmationMessage(Message.GetMessageAttributes(), this);
                }

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

        public async Task SendEmail()
        {
            try
            {
                IsBusy = true;
                UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);

                //Validamos conexion a internet
                if (await Utilities.VerifyInternetConnection())
                {
                    if (emailService.Parametizaciones == null)
                    {
                        var parametizaciones = await webApiService.GetParametizaciones(token);
                        await Task.Delay(1000);
                        //parametizaciones.Pass = await valdation.Desencriptar(parametizaciones.Pass);
                        emailService.SetAtributes(parametizaciones);
                    }

                    //verificamos que la contraseña cumpla con la validacion
                    if (!valdation.ValidatePasswordSimple(NewPassword))
                    {
                        NewPassword += RandomPasswordEx.PasswordGenerator(8, true);
                    }

                    string securepassword = NewPassword;
                    securepassword = await valdation.Encriptar(securepassword);

                    //validamos que el correo lo posea algun usuario
                    //si lo posee entonces la web api actualizara la contraseña cifrada
                    var response = await webApiService.RecuperarPasswordMethod(securepassword, Email, token);
                    await Task.Delay(1000);

                    if (response)
                    {
                        if (await webApiService.SendEmailApi(emailService.Parametizaciones.User,
                                                             Email,
                                                             Plantillas_Correo.TitleRecoverPassword,
                                                             NewPassword,
                                                             emailService.Parametizaciones.Client,
                                                             emailService.Parametizaciones.Port,
                                                             emailService.Parametizaciones.Pass) != 1)
                        {
                            await Message.Failed("Estimado usuario, la operación no pudo ser completada, por favor intente más tarde.");
                        }
                        else
                        {
                            await Message.Successful("Estimado usuario, el envio de la contraseña se realizo correctamente.");

                            await Application.Current.MainPage.Navigation.PopAsync();
                        }
                    }
                    else
                    {
                        await Message.Failed("Estimado usuario, la operación no pudo ser completada, verifique su correo o comuniquese con la sucursal más cercana de Carnes Don Fernando.");
                    }

                }

                IsBusy = false;
            }
            catch (Exception)
            {
                await Message.Failed("Estimado usuario por el momento nuestro servicio al cliente no está disponible, intente más tarde.");
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
                valdation.SetMessage(Message);
            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario ocurrió un error. Código 209.");
            }
        }

    }
}