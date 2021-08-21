using Acr.UserDialogs;
using AppCarnesDF.Helpers;
using AppCarnesDF.Helpers.Common;
using AppCarnesDF.Models;
using AppCarnesDF.Services;
using AppCarnesDF.Services.FontSize;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppCarnesDF.ViewModels.Menu
{
    public class MenuViewModel: BaseViewModel
    {
        #region Properties
        private readonly FontSizeService FontService = new FontSizeService();
        private readonly EmailService emailService = new EmailService();
        public readonly LogMessageAttention Message = new LogMessageAttention();
        #endregion

        public MenuViewModel()
        {

        }

        #region Methods
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
                await Message.Failed("Estimado usuario ocurrió un usuario. Código 209.");
            }
        }

        public async Task ConfirmClosing()
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;

                UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);

                Message.attributes.Message = "¿Está seguro que desea abandonar el sistema?";

                await pantallas.showConfirmationMessage(Message.GetMessageAttributes(), this);

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

        public async Task Closing(bool result)
        {
            try
            {
                if (result)
                {
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
            }
        }
        #endregion



    }
}
