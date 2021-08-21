using Acr.UserDialogs;
using AppCarnesDF.Models;
using AppCarnesDF.Services.FontSize;
using AppCarnesDF.Views;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;

namespace AppCarnesDF.Helpers.Common
{
    /// <summary>
    /// Metodo encargado del manejo de mensajes desplegados en la aplicacion
    /// </summary>
    public class LogMessageAttention
    {
        /// <summary>
        /// Mensajes genericos tipo plantilla para utilizarse en toda la aplicacion
        /// </summary>
        #region Mensajes Plantillas
        public const string Loading = "Procesando";
        public const string NavBarAutentication = "Autenticación";
        public const string WithoutAutentication = "El proceso de autenticación no fue completado.";
        #endregion

        public ConvertFontSizeBD FontSizeBD = new ConvertFontSizeBD();

        public MessageAttributes attributes = new MessageAttributes();

        public double SizeFonts { get; set; }
        public double SizeFontsCookie { get; set; }
        public double SizeFontsOptima { get; set; }

        public LogMessageAttention()
        {
            SetMessageAttributes();
        }

        /// <summary>
        /// Mensajes utilizados en Display Alert
        /// </summary>
        #region Mensajes Alert
        private const string v_attention = "Atención: ";
        private const string v_exito = "Éxito: ";
        private const string v_error = "Error: ";
        private const string v_aceptar = "Aceptar";
        private static string v_cancelar = "Cancelar";
        private static string v_bienvenido = "Bienvenid@:";
        #endregion

        /// <summary>
        /// Mensaje de bienvenida
        /// </summary>
        /// <param name="pmessage"></param>
        /// <returns></returns>
        public async Task welcomeMessage(string pmessage)
        {
            string _string = string.Empty;

            _string += pmessage;
            _string += Simbol._point;

            await PopupNavigation.Instance.PushAsync(new AlertMessages(v_bienvenido, pmessage, v_aceptar, SizeFonts, SizeFontsOptima, SizeFontsCookie));
        }

        /// <summary>
        /// Mensajes de atención para el usuario
        /// </summary>
        /// <param name="pmessage">Mensaje</param>
        /// <returns></returns>
        public async Task generalAttention(string pmessage)
        {
            string _string = string.Empty;

            _string += pmessage;
            _string += Simbol._point;

            await PopupNavigation.Instance.PushAsync(new AlertMessages(v_attention, pmessage, v_aceptar, SizeFonts, SizeFontsOptima, SizeFontsCookie));
        }

        /// <summary>
        /// Mensajes de tipo positivos indicando algun exito para el usuario
        /// </summary>
        /// <param name="pmessage">Mensaje</param>
        /// <returns></returns>
        public async Task Successful(string pmessage)
        {
            string _string = string.Empty;

            _string += pmessage;
            _string += Simbol._point;

            await PopupNavigation.Instance.PushAsync(new AlertMessages(v_exito, pmessage, v_aceptar, SizeFonts, SizeFontsOptima, SizeFontsCookie));
        }

        /// <summary>
        /// Mensajes en caso de error
        /// </summary>
        /// <param name="pmessage">Mensaje</param>
        /// <returns></returns>
        public async Task Failed(string pmessage)
        {
            string _string = string.Empty;

            _string += pmessage;
            _string += Simbol._point;

            await PopupNavigation.Instance.PushAsync(new AlertMessages(v_error, pmessage, v_aceptar, SizeFonts, SizeFontsOptima, SizeFontsCookie));
        }

        public void SetMessageAttributes()
        {
            attributes.Title = v_attention;
            attributes.Message = "¿Está seguro que desea continuar con la operación?";
            attributes.CancelButtonText = v_cancelar;
            attributes.ButtonText = v_aceptar;
        }

        public MessageAttributes GetMessageAttributes()
        {
            attributes.SizeFonts = SizeFonts;
            attributes.SizeFontsCookie = SizeFontsCookie;
            attributes.SizeFontsOptima = SizeFontsOptima;
            return attributes;
        }
    }
}
