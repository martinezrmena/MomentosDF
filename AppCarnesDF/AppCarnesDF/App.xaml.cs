using AppCarnesDF.Helpers.Common;
using AppCarnesDF.Services.FontSize;
using AppCarnesDF.Views;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppCarnesDF
{
    public partial class App : Application
    {
        #region
        private LogMessageAttention Message = new LogMessageAttention();
        private FontSizeService servicio = new FontSizeService();
        #endregion

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage( new MainPage());

            //Global settings for Checkboxes
            Plugin.InputKit.Shared.Controls.CheckBox.GlobalSetting.BorderColor = Color.Red;
        }

        protected async override void OnStart()
        {
            // Handle when your app starts
            await MensajeInicial();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        /// <summary>
        /// Mensaje que se despliega al iniciar la aplicación
        /// </summary>
        /// <returns></returns>
        private async Task MensajeInicial()
        {
            Message.SizeFonts = servicio.ConsultarFont();
            Message.SizeFontsCookie = servicio.ConsultarFontCookie();
            Message.SizeFontsOptima = servicio.ConsultarFontOptima();

            await Message.welcomeMessage("Estimado usuario, sea bienvenido a Carnes Don Fernando, los especialistas en carnes.");
        }
    }
}
