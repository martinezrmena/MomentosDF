using AppCarnesDF.Helpers;
using AppCarnesDF.Helpers.Common;
using AppCarnesDF.Models.PoliticaPrivacidad;
using AppCarnesDF.Services;
using AppCarnesDF.Services.FontSize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppCarnesDF.ViewModels.Configuracion
{
    class PoliticaPrivacidadViewModel : BaseViewModel
    {
        #region Properties
        private readonly FontSizeService FontService = new FontSizeService();

        public LogMessageAttention Message = new LogMessageAttention();

        private WebApiService webApiService = new WebApiService();

        private PoliticaPrivacidadModel model;
        public PoliticaPrivacidadModel Model
        {
            get { return model; }
            set
            {
                model = value;
                if (model.Descripcion.Length >= 3000)
                {
                    model.Descripcion1 = model.Descripcion.Substring(0, 3000);

                    string intermedio = model.Descripcion.Substring(3000);
                    if (intermedio.Length >= 3000)
                    {
                        model.Descripcion2 = intermedio.Substring(0, 3000);
                        model.Descripcion3 = intermedio.Substring(3000);
                    }
                    else
                    {
                        model.Descripcion2 = intermedio;
                    }
                }
                else
                {
                    model.Descripcion1 = model.Descripcion;
                }
                OnPropertyChanged();
            }
        }
        #endregion

        #region Command
        public ICommand BackCommand { get; set; }
        #endregion

        public PoliticaPrivacidadViewModel()
        {
            BackCommand = new Command(async () => await Close());
        }

        #region Methods

        private async Task Close()
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
                await Message.Failed("Estimado usuario ocurrió un error: Código 209.");
            }
        }

        public void Inicializar()
        {
            PoliticaPrivacidadModel TempPolitica;

            Thread actividad = new Thread(new ThreadStart(async () =>
            {
                try
                {
                    TempPolitica = await GetPoliticaPrivacidad();

                    if (TempPolitica != null)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            try
                            {
                                Model = TempPolitica;
                            }
                            catch (Exception ex)
                            {
                                await Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    await Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
                }
            }));

            actividad.Start();
        }

        /// <summary>
        /// Metodo que permite consultar via web service la politica de privacidad
        /// disponibles
        /// </summary>
        /// <returns></returns>
        public async Task<PoliticaPrivacidadModel> GetPoliticaPrivacidad()
        {

            // Definir el token de cancelación.
            CancellationTokenSource source = new CancellationTokenSource(new TimeSpan(0, 0, BrowserView.TimeOutLimit));
            CancellationToken token = source.Token;
            PoliticaPrivacidadModel webAcercade = null;

            try
            {
                webAcercade = await webApiService.GetPoliticaPrivacidad(token);
            }
            catch (Exception)
            {
                await Message.Failed("Estimado usuario, existen problemas de conexión con el servicio web actualmente, intentelo más tarde.");
            }

            return webAcercade;
        }
        #endregion
    }
}
