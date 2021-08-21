using Acr.UserDialogs;
using AppCarnesDF.Helpers.Common;
using AppCarnesDF.Models.ActividadReciente;
using AppCarnesDF.Models.User;
using AppCarnesDF.Services;
using AppCarnesDF.Services.FontSize;
using AppCarnesDF.Services.User;
using AppCarnesDF.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppCarnesDF.ViewModels.ActividadReciente
{
    public class ActividadRecienteViewModel : BaseViewModel
    {
        #region Properties
        private readonly FontSizeService FontService = new FontSizeService();

        private readonly WebApiService webApiService = new WebApiService();

        public readonly LogMessageAttention Message = new LogMessageAttention();

        private GeneralUtilities Utilities = new GeneralUtilities();

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

        private ActividadRecienteModel _selectedItem;

        public ActividadRecienteModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
            }
        }

        private UserModel _user;
        public UserModel User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }

        private UserViewModel _uservm;
        public UserViewModel UserVM
        {
            get { return _uservm; }
            set
            {
                _uservm = value;
            }
        }

        public List<Sucursal> ListSucursal { set; get; }
        private PickerSucursalService pickerSucursal = new PickerSucursalService();

        private bool _listviewvisible;
        public bool ListViewVisible
        {
            get { return _listviewvisible; }
            set
            {
                _listviewvisible = value;
                OnPropertyChanged();
            }
        }

        private bool _labelvisible;
        public bool LabelVisible
        {
            get { return _labelvisible; }
            set
            {
                _labelvisible = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand BackCommand { get; set; }
        #endregion

        public ActividadRecienteViewModel(UserModel userModel, UserViewModel viewModel)
        {
            User = userModel;
            UserVM = viewModel;

            if (UserVM.Actividades != null)
            {
                Actividades = UserVM.Actividades;
            }

            LabelVisible = false;

            ListViewVisible = false;

            BackCommand = new Command(BackAction);
        }

        #region Methods
        public async void Inicializar(List<ActividadRecienteModel> TempActividades)
        {
            try
            {
                UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);

                if (TempActividades != null)
                {
                    Actividades = TempActividades.OrderByDescending(c => c.Fecha_Identificador).Take(10).ToList();

                    if (Actividades.Count > 0)
                    {
                        ListViewVisible = true;
                    }
                }
                else
                {
                    LabelVisible = true;
                }

                UserDialogs.Instance.HideLoading();
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
            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario ocurrión un error. Código: 209.");
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

            List<ActividadRecienteModel> webActividades = await webApiService.GetActividadReciente(User.Cedula ,token);

            return webActividades;
        }

        private async void BackAction()
        {
            UserVM.Actividades = Actividades;
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        #endregion
    }
}
