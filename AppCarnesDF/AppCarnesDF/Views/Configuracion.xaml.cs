using AppCarnesDF.Models.User;
using AppCarnesDF.ViewModels.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCarnesDF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Configuracion : ContentPage
    {
        public ConfiguracionViewModel context;
        int guardado = 0;

        public Configuracion(UserModel user)
        {
            InitializeComponent();

            try
            {
                context = new ConfiguracionViewModel(user);
                BindingContext = context;
                swtActivarNotificaciones.Toggled -= SwtActivarNotificaciones_Toggled;
                swtActivarNotificaciones.IsToggled = context.tgNotificaciones;
                guardado++;
                swtActivarNotificaciones.Toggled += SwtActivarNotificaciones_Toggled;
            }
            catch (Exception ex)
            {
                Task.Run(async() => 
                {
                    await context.Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
                });
            }
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

        private void SwtActivarNotificaciones_Toggled(object sender, ToggledEventArgs e)
        {
            if (context != null)
            {
                context.tgNotificaciones = swtActivarNotificaciones.IsToggled;

                if (guardado > 0)
                {
                    context.GuardarInit();
                }
            }

        }

        private void ActivarNotificacionCommand(object sender, EventArgs e)
        {
            swtActivarNotificaciones.IsToggled = !swtActivarNotificaciones.IsToggled;
        }
    }
}