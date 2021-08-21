using AppCarnesDF.ViewModels;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace AppCarnesDF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AlertMessages : PopupPage
    {
        public AlertMessages(string titulo, 
                             string mensaje,
                             string btnTexto,
                             double font,
                             double fontOptima,
                             double fontCookie)
        {
            InitializeComponent();
            BindingContext = new AlertMessageViewModel(titulo,
                                                       mensaje,
                                                       btnTexto,
                                                       font,
                                                       fontOptima,
                                                       fontCookie);
        }

        public AlertMessages()
        {

        }
    }
}