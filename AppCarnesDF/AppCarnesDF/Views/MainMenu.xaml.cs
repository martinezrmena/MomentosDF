using AppCarnesDF.Helpers.Common;
using AppCarnesDF.Models.User;
using System;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;
using System.Linq;
using System.Threading.Tasks;
using AppCarnesDF.Helpers;
using AppCarnesDF.Views.ConfiguracionDetails;
using AppCarnesDF.ViewModels.Menu;

namespace AppCarnesDF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenu : Xamarin.Forms.TabbedPage
    {
        #region Properties
        public string tipoAutenticacion { get; set; }
        private UserModel UserAutenticated;
        private CarnesDF pantallas = new CarnesDF();
        private MenuViewModel context = new MenuViewModel();
        #endregion

        public MainMenu(string TP, UserModel userModel)
        {
            InitializeComponent();
            tipoAutenticacion = TP;
            UserAutenticated = userModel;
            BindingContext = context;

            //Establecemos la barra del tabbed page en la parte inferior para la plataforma Android
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
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
            }
            catch (Exception ex)
            {
                await context.Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
            }

        }

        protected override bool OnBackButtonPressed()
        {

            Task.Run(async () =>
            {
                try
                {
                    await context.ConfirmClosing();
                }
                catch (Exception ex)
                {
                    await context.Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
                }
                
            });

            return true;

        }

        /// <summary>
        /// Metodo encargado de establecer los parametros en la pantalla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void General_Appearing(object sender, EventArgs e)
        {
            try
            {
                if (UserAutenticated != null)
                {
                    viewGeneral.ModifyUserInformation(UserAutenticated);
                }
            }
            catch (Exception ex)
            {
                await context.Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
            }
            
        }

        protected async override void OnCurrentPageChanged()
        {
            try
            {
                //Metodo utilizado para montar nueva pagina que tome el
                //posicionamiento absoluto en la pantalla
                if (CurrentPage.Title == "Configuracion")
                {
                    await pantallas.showConfiguration(UserAutenticated)
                        .ContinueWith((t) =>
                        {
                        //Solo el hilo principal puede modificar
                        //la vista(jerarquia)
                        Device.BeginInvokeOnMainThread(() =>
                            {
                            //Regresamos a la pantalla principal
                            CurrentPage = Children[0];
                            });
                        }).ConfigureAwait(true);
                    return;
                }
            }
            catch (Exception ex)
            {
                await context.Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
            }

            base.OnCurrentPageChanged();

        }
    }
}