using Acr.UserDialogs;
using AppCarnesDF.Helpers.Common;
using AppCarnesDF.Models.Share;
using AppCarnesDF.Models.User;
using AppCarnesDF.Services;
using AppCarnesDF.Services.CompartirCodigo;
using AppCarnesDF.Services.FontSize;
using AppCarnesDF.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppCarnesDF.ViewModels.Configuracion
{
    public class CompartirCodigoViewModel: BaseViewModel
    {
        #region Properties
        private readonly FontSizeService FontService = new FontSizeService();
        private readonly WebApiService webApiService = new WebApiService();
        public LogMessageAttention Message = new LogMessageAttention();
        private CancellationTokenSource source = new CancellationTokenSource(new TimeSpan(0, 3, 0));
        private CancellationToken token;
        private ShareService servicio = new ShareService();

        private string codigo;
        public string Codigo
        {
            get { return codigo; }
            set
            {
                SetProperty(ref codigo, value);
            }
        }

        private UserModel user;
        public UserModel User
        {
            get { return user; }
            set
            {
                SetProperty(ref user, value);
            }
        }

        private ShareModel model;
        public ShareModel Model
        {
            get { return model; }
            set
            {
                SetProperty(ref model, value);
            }
        }

        public ConfiguracionViewModel ConfiguracionViewModel { get; set; }

        private readonly GeneralUtilities Utilities = new GeneralUtilities();
        #endregion

        #region Command
        public ICommand CompartirCodigoCommand { get; set; }
        public ICommand CopiarCodigoCommand { get; set; }
        public ICommand ClosingCommand { get; private set; }
        #endregion

        public CompartirCodigoViewModel()
        {
            CompartirCodigoCommand = new Command(async () => await ConfirmInvitarAmigos());
            CopiarCodigoCommand = new Command(async () => await ConfirmCompartirCodigo());
            ClosingCommand = new Command(async () => await Cerrar());
            Codigo = RandomPasswordEx.PasswordGenerator(6 , false);
            token = source.Token;
        }

        private async Task Cerrar()
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
            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario ocurrió un error. Código 209.");
            }
        }

        public async Task ConfirmCompartirCodigo()
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;

                UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);

                Message.attributes.Message = "¿Está seguro que desea copiar el código para compartir? Este es un proceso que solo se puede realizar una vez, al realizarlo recibirá una acreditación especial.";

                await pantallas.showConfirmationMessage(Message.GetMessageAttributes(), this, 1);

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

        public async Task ConfirmInvitarAmigos()
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;

                UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);

                Message.attributes.Message = "¿Está seguro que desea compartir la aplicación? Este es un proceso que solo se puede realizar una vez, al realizarlo recibirá una acreditación especial.";

                await pantallas.showConfirmationMessage(Message.GetMessageAttributes(), this, 0);

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

        public async Task CompartirCodigo()
        {

            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    if (await Utilities.VerifyInternetConnection())
                    {
                        await Share.RequestAsync(new ShareTextRequest
                        {
                            Text = Codigo,
                            Title = "Compartir código Momentos Don Fernando"
                        });

                        await Finalize();
                    }

                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
            }
            
        }

        public async Task CopiarCodigo()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    if (await Utilities.VerifyInternetConnection())
                    {
                        await Finalize();
                    }
                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
            }
        }

        private async Task Finalize() 
        {
            try
            {
                int valor = 0;
                int.TryParse(User.Puntos, out valor);
                valor += 100;
                User.Puntos = valor.ToString();
                model = new ShareModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

                await Clipboard.SetTextAsync(Codigo);

                if (await webApiService.ActualizarUser(User, token,true))
                {
                    //Necesario guardar en bd el resultado para evitar más inserciones
                    //Ademas se guardara en la nube como respaldo
                    model.Saved = true;
                    model.Id = User.Cedula;
                    await servicio.SaveShare(model);
                    servicio.Guardar(model);
                    if (ConfiguracionViewModel != null)
                    {
                        ConfiguracionViewModel.Compartir = true;
                    }
                    await Message.Successful("Estimado usuario, el código ha sido copiado a su portapapeles, puede compartirlo en cualquier aplicación, muchas gracias por compartir Momentos Don Fernando.");
                }
                else
                {
                    valor -= 100;
                    User.Puntos = valor.ToString();
                    await Message.Failed("Estimado usuario, ocurrio un error durante el proceso, por favor intentelo más tarde.");
                }

                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario ocurrio un error: " + ex.Message);
            }
        }
    }
}
