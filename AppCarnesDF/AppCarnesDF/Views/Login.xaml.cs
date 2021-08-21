using Acr.UserDialogs;
using AppCarnesDF.Helpers;
using AppCarnesDF.Helpers.Common;
using AppCarnesDF.Services;
using AppCarnesDF.ViewModels.Login;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCarnesDF.Views
{
    /// <summary>
    /// Clase encargada del manejo de logue de los usuarios utilizando los distintos
    /// tipos de autenticación: Google, Facebook, Cuenta de la empresa
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        #region Variables
        public string _tipoAutenticacion { get; set; }
        private LoginViewModel context = new LoginViewModel();
        public GeneralUtilities Utilities = new GeneralUtilities();
        #endregion

        public Login()
        {
            InitializeComponent();
            BindingContext = context;
        }

        /// <summary>
        /// Permite consultar los tamaños de fuentes para el texto de la aplicación
        /// </summary>
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                await context.UpdateFontSize();
                Utilities.Message = context.Message;
            }
            catch (Exception ex)
            {
                await context.Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
            }

        }

        /// <summary>
        /// Metodo para validación principal para determinar cual es el metodo de autenticacion
        /// que definio el usuario
        /// </summary>
        private async Task GetTipoAutentication()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);

                if (await Utilities.VerifyInternetConnection())
                {
                    switch (_tipoAutenticacion)
                    {
                        case TipoAutenticacion.NormalAutentication:
                            await context.GetUserFromService();
                            break;
                        default:
                            await context.Message.generalAttention("El usuario no se ha autenticado");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                await context.Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        #region Normal Autentication
        /// <summary>
        /// Autenticacion desde servidores propios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            try
            {
                UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);

                if (await context.ValidateForm())
                {
                    if (await Utilities.VerifyInternetConnection())
                    {
                        await context.GetUserFromService();
                    }
                }
            }
            catch (Exception ex)
            {
                await context.Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }
        #endregion

    }
}