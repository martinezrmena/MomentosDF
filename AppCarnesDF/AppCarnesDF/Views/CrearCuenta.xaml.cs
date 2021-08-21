using AppCarnesDF.Helpers.Common;
using AppCarnesDF.Models.Ubicacion;
using AppCarnesDF.Models.User;
using AppCarnesDF.ViewModels.CrearCuenta;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCarnesDF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrearCuenta : ContentPage
    {

        public class informacion
        {
            public const string LeerMas = "Leer Más";
            public const string LeerMenos = "Leer Menos";
            public bool Accordion { get; set; } = false;
        }

        #region Propiedades
        private CreateAccountViewModel context = new CreateAccountViewModel();
        private informacion info = new informacion();
        #endregion

        #region Inicio
        public CrearCuenta(string title,
                           List<ProvinciaEntity> Provincias,
                           List<CantonEntity> Cantones,
                           List<DistritoEntity> Distritos,
                           List<Sucursal> ListSucursal)
        {
            InitializeComponent();
            InitializeViewModel(title, Provincias, Cantones, Distritos, ListSucursal);
            dpFechaNacimiento.MaximumDate = DateTime.Now;
        }

        public CrearCuenta(UserModel user,
                           string title,
                           List<ProvinciaEntity> Provincias,
                           List<CantonEntity> Cantones,
                           List<DistritoEntity> Distritos,
                           List<Sucursal> ListSucursal)
        {
            InitializeComponent();
            BindingContext = context;
            InitializeUpdate(user, title, Provincias, Cantones, Distritos, ListSucursal);
            dpFechaNacimiento.MaximumDate = DateTime.Now;
        }

        public async void InitializeViewModel(string title,
                                              List<ProvinciaEntity> Provincias,
                                              List<CantonEntity> Cantones,
                                              List<DistritoEntity> Distritos,
                                              List<Sucursal> ListSucursal)
        {
            try
            {
                setEventPronvinciaPicker(false);
                context.User = new UserModel(Guid.NewGuid().ToString(),
                                             Guid.NewGuid().ToString());
                BindingContext = context;
                context.Title = title;
                context.InicializarPassword();
                context.SetPickerValues(Provincias, Cantones, Distritos, ListSucursal);
                setEventPronvinciaPicker(true);
            }
            catch (Exception ex)
            {
                await context.Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
            }
        }

        private void setEventPronvinciaPicker(bool active)
        {
            if (!active)
            {
                cmbProvincia.SelectedIndexChanged -= CmbProvincia_SelectedIndexChanged;
                cmbCanton.SelectedIndexChanged -= cmbCanton_SelectedIndexChanged;
            }
            else
            {
                cmbProvincia.SelectedIndexChanged += CmbProvincia_SelectedIndexChanged;
                cmbCanton.SelectedIndexChanged += cmbCanton_SelectedIndexChanged;
            }
        }

        /// <summary>
        /// Metodo encargado de inicializar los parametros en caso
        /// de que se vaya a editar la información de usuario
        /// </summary>
        /// <param name="user">modelo con la información del usuario</param>
        /// <param name="title">identificador de tipo de operación (crear/modificar)</param>
        private async void InitializeUpdate(UserModel user, string title,
                                            List<ProvinciaEntity> Provincias,
                                            List<CantonEntity> Cantones,
                                            List<DistritoEntity> Distritos,
                                            List<Sucursal> ListSucursal)
        {
            try
            {
                context.TempUser = new UserModel(user);
                setEventPronvinciaPicker(false);
                context.User = user;
                context.Imagen = user.PictureURL;
                context.Title = title;
                context.InicializarPassword();
                context.Temp_Password = user.Password;
                user.Password = await context.validationString.Desencriptar(user.Password);
                context.FindSelectedGender();
                context.FindSelectedTipoCedula();
                context.SetPickerValues(Provincias, Cantones, Distritos, ListSucursal);
                if (!string.IsNullOrEmpty(user.Fecha_Nacimiento))
                {
                    dpFechaNacimiento.Placeholder = string.Empty;
                    dpFechaNacimiento.Date = DateTime.ParseExact(user.Fecha_Nacimiento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                setEventPronvinciaPicker(true);
            }
            catch (Exception ex)
            {
                await context.Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
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
        #endregion

        protected override bool OnBackButtonPressed()
        {

            Task.Run(async () =>
            {
                try
                {
                    Device.BeginInvokeOnMainThread(async () => 
                    {
                        await context.Close(false);
                    });
                }
                catch (Exception ex)
                {
                    await context.Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
                }

            });

            return true;

        }

        private async void CmbProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                context.SetCantonesChange();
            }
            catch (Exception ex)
            {
                await context.Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
            }
        }

        private async void cmbCanton_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                context.SetDistritosChange();
            }
            catch (Exception ex)
            {
                await context.Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
            }
        }
    }
}