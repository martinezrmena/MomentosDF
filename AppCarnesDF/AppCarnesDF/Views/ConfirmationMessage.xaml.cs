using AppCarnesDF.Models;
using AppCarnesDF.ViewModels;
using AppCarnesDF.ViewModels.Configuracion;
using AppCarnesDF.ViewModels.CrearCuenta;
using AppCarnesDF.ViewModels.Menu;
using AppCarnesDF.ViewModels.Users;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCarnesDF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfirmationMessage : PopupPage
    {
        public ConfirmationMessage(MessageAttributes attributes, RecoverPasswordViewModel viewModelRecoverPassword)
        {
            InitializeComponent();

            BindingContext = new ConfirmationMessageViewModel(attributes, viewModelRecoverPassword);
        }

        public ConfirmationMessage(MessageAttributes attributes, CompartirCodigoViewModel viewModelCompartir, int value)
        {
            InitializeComponent();

            BindingContext = new ConfirmationMessageViewModel(attributes, viewModelCompartir, value);
        }

        public ConfirmationMessage(MessageAttributes attributes, ConfiguracionViewModel viewModelConfiguracion)
        {
            InitializeComponent();

            BindingContext = new ConfirmationMessageViewModel(attributes, viewModelConfiguracion);
        }

        public ConfirmationMessage(MessageAttributes attributes, CreateAccountViewModel viewModelAccount)
        {
            InitializeComponent();

            BindingContext = new ConfirmationMessageViewModel(attributes, viewModelAccount);
        }

        public ConfirmationMessage(MessageAttributes attributes, MenuViewModel viewModelMenu)
        {
            InitializeComponent();

            BindingContext = new ConfirmationMessageViewModel(attributes, viewModelMenu);
        }
    }
}