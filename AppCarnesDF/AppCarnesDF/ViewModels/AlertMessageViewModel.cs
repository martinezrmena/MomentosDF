using AppCarnesDF.Models;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppCarnesDF.ViewModels
{
    /// <summary>
    /// Clase encargada de manejar los mensajes principales que se le presentan al usuario:
    /// advertencia, éxito, error, etc
    /// </summary>
    public class AlertMessageViewModel: MessageAttributes
    {
        public Command CerrarCommand { get; set; }

        public AlertMessageViewModel(string titulo, 
                                     string mensaje,
                                     string btnTexto,
                                     double font,
                                     double fontOptima,
                                     double fontCookie)
        {
            Title = titulo;
            Message = mensaje;
            SizeFonts = font;
            SizeFontsCookie = fontCookie;
            SizeFontsOptima = fontOptima;
            ButtonText = btnTexto;
            CerrarCommand = new Command(async () => await Cerrar());
        }

        private async Task Cerrar()
        {
            try
            {
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {

            }
        }

    }
}
