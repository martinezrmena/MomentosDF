using Acr.UserDialogs;
using AppCarnesDF.Helpers.Common;
using AppCarnesDF.Models.ActividadReciente;
using AppCarnesDF.Models.User;
using AppCarnesDF.Services;
using AppCarnesDF.Services.FontSize;
using AppCarnesDF.Services.User;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppCarnesDF.ViewModels.Users
{
    public class UserViewModel : BaseViewModel
    {
        #region Properties
        public ObservableCollection<UserModel> Users { get; set; }

        private readonly WebApiService webApiService = new WebApiService();

        private GeneralUtilities Utilities = new GeneralUtilities();

        private UserService servicio = new UserService();

        private FontSizeService FontService = new FontSizeService();

        public LogMessageAttention Message = new LogMessageAttention();

        private PickerSucursalService pickerSucursal = new PickerSucursalService();

        UserModel modelo;
        private UserModel usuario;

        public UserModel UserAutenticated
        {
            get { return usuario; }
            set
            {
                usuario = value;
                OnPropertyChanged();
            }
        }

        private string nombreUser;

        public string NombreUser
        {
            get { return nombreUser; }
            set
            {
                nombreUser = value;
                OnPropertyChanged();
            }
        }

        public Command GuardarCommand { get; set; }
        public Command ModificarCommand { get; set; }
        public Command EliminarCommand { get; set; }
        public Command LimpiarCommand { get; set; }
        public Command ActividadRecienteCommand { get; set; }

        public Stream ProfileImage { get; set; }

        private List<ActividadRecienteModel> actividades;

        public List<ActividadRecienteModel> Actividades
        {
            get { return actividades; }
            set
            {
                actividades = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public UserViewModel()
        {
            Users = servicio.Consultar();
            SizeFonts = FontService.ConsultarFont();
            GuardarCommand = new Command(() => Guardar(), () => !IsBusy);
            ModificarCommand = new Command(() => Modificar(), () => !IsBusy);
            EliminarCommand = new Command(() => Eliminar(), () => !IsBusy);
            LimpiarCommand = new Command(Limpiar, () => !IsBusy);
            ActividadRecienteCommand = new Command(async()=> await ActividadReciente());
            Task.Run(async () => 
            {
                await UpdateFontSize();
            });
        }

        private async Task ActividadReciente()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;

                    UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);

                    List<ActividadRecienteModel> TempActividades = new List<ActividadRecienteModel>();

                    List<ActividadRecienteModel> actividadesd = new List<ActividadRecienteModel>();

                    if (await Utilities.VerifyInternetConnection())
                    {
                        actividadesd = await GetActividades();
                        await Task.Delay(1000);
                    }

                    List<Sucursal> ListSucursal = await pickerSucursal.GetSucursales();
                    await Task.Delay(1000);
                    ListSucursal = ListSucursal.OrderBy(c => c.Descripcion_Centro).ToList();

                    if (actividadesd != null)
                    {
                        foreach (var item in actividadesd)
                        {
                            if (ListSucursal != null)
                            {
                                item.Sucursal = ListSucursal.Where(x => x.Centro.ToString() == item.Centro).FirstOrDefault().Descripcion_Centro;
                            }
                            if (!string.IsNullOrEmpty(item.Fecha_Mov))
                            {
                                item.Fecha_Identificador = DateTime.Parse(item.Fecha_Mov);
                                item.Fecha_Mov = item.Fecha_Identificador.ToString("MMM d");
                                TempActividades.Add(item);
                            }
                        }

                        Actividades = TempActividades.OrderByDescending(c => c.Fecha_Identificador).Take(10).ToList();

                    }

                    await pantallas.showActividadReciente(UserAutenticated, this, Actividades);

                    IsBusy = false;

                }
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
                if (UserAutenticated?.Nombre?.Length > 7)
                {
                    NombreUser = UserAutenticated?.Nombre?.Substring(0, 8);
                }
                else 
                {
                    NombreUser = UserAutenticated?.Nombre;
                }
            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario ocurrió un error. Código 209.");
            }
        }

        /// <summary>
        /// Metodo que permite consultar via web service las promociones que se encuentran
        /// disponibles
        /// </summary>
        /// <returns></returns>
        public async Task<List<ActividadRecienteModel>> GetActividades()
        {

            // Definir el token de cancelación.
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            List<ActividadRecienteModel> webActividades = await webApiService.GetActividadReciente(UserAutenticated.Cedula, token);

            return webActividades;
        }

        private void Guardar()
        {
            IsBusy = true;

            //modelo = new UserModel()
            //{
            //    Nombre = Nombre,
            //    Apellido = Apellido
            //};

            //servicio.Guardar(modelo);

            IsBusy = false;
        }

        private void Modificar()
        {
            IsBusy = true;

            //modelo = new UserModel()
            //{
            //    Nombre = Nombre,
            //    Apellido = Apellido,
            //    Id = Id
            //};

            servicio.Modificar(modelo);

            IsBusy = false;
        }

        private void Eliminar()
        {
            IsBusy = true;

            //servicio.Eliminar(Id);

            IsBusy = false;
        }

        private void Limpiar()
        {
            //Nombre = string.Empty;
            //Apellido = string.Empty;
            //Email = string.Empty;
            //Id = string.Empty;
        }
    }
}