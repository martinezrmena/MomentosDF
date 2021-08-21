using Acr.UserDialogs;
using AppCarnesDF.Helpers;
using AppCarnesDF.Helpers.Common;
using AppCarnesDF.Models;
using AppCarnesDF.Models.Ubicacion;
using AppCarnesDF.Models.User;
using AppCarnesDF.Services;
using AppCarnesDF.Services.FontSize;
using AppCarnesDF.Services.Ubicacion;
using AppCarnesDF.Services.User;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppCarnesDF.ViewModels.CrearCuenta
{
    public class CreateAccountViewModel : BaseViewModel
    {
        #region Properties
        private readonly FontSizeService FontService = new FontSizeService();
        public readonly LogMessageAttention Message = new LogMessageAttention();
        public TableStorageService TableStorage = new TableStorageService();
        private GeneralUtilities Utilities = new GeneralUtilities();
        public MessageReceived MensajeEnviado = new MessageReceived();
        public ValidationString validationString = new ValidationString();
        private PickerSucursalService pickerSucursal = new PickerSucursalService();
        private PickerCantonService pickerCanton = new PickerCantonService();
        private PickerProvinciaService pickerProvincia = new PickerProvinciaService();
        private readonly WebApiService webApiService = new WebApiService();
        CancellationTokenSource source = new CancellationTokenSource(new TimeSpan(0, 1, 0));
        CancellationToken token;

        public MediaFile file { get; set; }

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

        public UserModel TempUser { get; set; }

        private string confirmarpassword;
        public string ConfirmarPassword
        {
            get { return confirmarpassword; }
            set
            {
                confirmarpassword = value;
                OnPropertyChanged();
            }
        }

        private string confirmarnewpassword;
        public string ConfirmarNewPassword
        {
            get { return confirmarnewpassword; }
            set
            {
                confirmarnewpassword = value;
                OnPropertyChanged();
            }
        }

        private string newpassword;
        public string NewPassword
        {
            get { return newpassword; }
            set
            {
                newpassword = value;
                OnPropertyChanged();
            }
        }

        private string currentpassword;
        public string CurrentPassword
        {
            get { return currentpassword; }
            set
            {
                currentpassword = value;
                OnPropertyChanged();
            }
        }

        private bool modificarimagen;
        public bool ModificarImagen
        {
            get { return modificarimagen; }
            set
            {
                modificarimagen = value;
            }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        private bool ischecked1;
        public bool IsChecked1
        {
            get { return ischecked1; }
            set
            {
                ischecked1 = value;
            }
        }

        private bool ischecked2;
        public bool IsChecked2
        {
            get { return ischecked2; }
            set
            {
                ischecked2 = value;
            }
        }

        private bool modifypassword;
        public bool ModifyPassword
        {
            get { return modifypassword; }
            set
            {
                modifypassword = value;
                OnPropertyChanged();
            }
        }

        private bool editarpassword;
        public bool EditarPassword
        {
            get { return editarpassword; }
            set
            {
                editarpassword = value;
                OnPropertyChanged();
            }
        }

        private bool crearpassword;
        public bool CrearPassword
        {
            get { return crearpassword; }
            set
            {
                crearpassword = value;
                OnPropertyChanged();
            }
        }

        private double height;
        public double Height
        {
            get { return height; }
            set
            {
                height = value;
                OnPropertyChanged();
            }
        }

        private string placeholdersucusal;
        public string PlaceholderSucursal
        {
            get { return placeholdersucusal; }
            set
            {
                placeholdersucusal = value;
                OnPropertyChanged();
            }
        }

        private Color placeholdercolor;
        public Color PlaceholderColor
        {
            get { return placeholdercolor; }
            set
            {
                placeholdercolor = value;
                OnPropertyChanged();
            }
        }

        private Color pcfecha;
        public Color PCFecha
        {
            get { return pcfecha; }
            set
            {
                pcfecha = value;
                OnPropertyChanged();
            }
        }

        private ImageSource imagen;
        public ImageSource Imagen
        {
            get { return imagen; }
            set
            {
                imagen = value;
                OnPropertyChanged();
            }
        }

        private string temp_password;
        public string Temp_Password
        {
            get { return temp_password; }
            set
            {
                temp_password = value;
                OnPropertyChanged();
            }
        }

        public List<CantonEntity> ListCanton
        {
            get;
            set;
        }

        public List<DistritoEntity> ListDistrito
        {
            get;
            set;
        }

        private DateTime fecha_nacimiento;
        public DateTime Fecha_Nacimiento
        {
            get { return fecha_nacimiento; }
            set
            {
                fecha_nacimiento = value;
                if (fecha_nacimiento.Date != DateTime.Now.Date)
                {
                    PCFecha = Color.Black;
                    User.Fecha_Nacimiento = Fecha_Nacimiento.ToString("dd/MM/yyyy");
                }
                else
                {
                    PCFecha = Color.Gray;
                }
                OnPropertyChanged();
            }
        }
        #endregion

        #region PickerProperties
        #region Gender
        /// <summary>
        /// Propiedades del Picker para que el usuario establezca su género
        /// </summary>
        public List<Genero> ListGender
        {
            get;
            set;
        }

        private Genero _selectedGender;
        public Genero SelectedGender
        {
            get
            {
                return _selectedGender;
            }
            set
            {
                SetProperty(ref _selectedGender, value);
                User.Genero = _selectedGender.Value;
                Gender = "Género : " + _selectedGender.Value;
            }
        }
        private string _gender;
        public string Gender
        {
            get
            {
                return _gender;
            }
            set
            {
                SetProperty(ref _gender, value);
            }
        }
        #endregion

        #region TipoIdentificacion
        /// <summary>
        /// Propiedades del Picker para que el usuario establezca su género
        /// </summary>
        public List<TipoIdentificacionModel> ListTipoIdentificacion
        {
            get;
            set;
        }

        private TipoIdentificacionModel _selectedTipoIdentificacion;
        public TipoIdentificacionModel SelectedTipoIdentificacion
        {
            get
            {
                return _selectedTipoIdentificacion;
            }
            set
            {
                SetProperty(ref _selectedTipoIdentificacion, value);
                User.Tipo_Cedula = _selectedTipoIdentificacion.Value;
                TipoIdentificacion = "Tipo Identificación: " + _selectedTipoIdentificacion.Value;
            }
        }
        private string _tipoidentificacion;
        public string TipoIdentificacion
        {
            get
            {
                return _tipoidentificacion;
            }
            set
            {
                SetProperty(ref _tipoidentificacion, value);
            }
        }
        #endregion

        #region Sucursal
        /// <summary>
        /// Propiedades del Picker para que el usuario establezca la sucursal
        /// </summary>
        public ObservableCollection<Sucursal> ListSucursal
        {
            get;
            set;
        } = new ObservableCollection<Sucursal>();

        public ObservableCollection<Sucursal> SelectedSucursal
        {
            get;
            set;
        } = new ObservableCollection<Sucursal>();

        private string _sucursal;
        public string Sucursal
        {
            get
            {
                return _sucursal;
            }
            set
            {
                SetProperty(ref _sucursal, value);
            }
        }

        public ICommand ShowSucursalesPicker { get; private set; }
        #endregion

        #region Canton
        /// <summary>
        /// Propiedades del Picker para que el usuario establezca el cantón
        /// </summary>
        private List<CantonEntity> listcantontemp;
        public List<CantonEntity> ListCantonTemp
        {
            get
            {
                return listcantontemp;
            }
            set
            {
                listcantontemp = value;
                OnPropertyChanged();
            }
        }

        private CantonEntity _selectedCanton;
        public CantonEntity SelectedCanton
        {
            get
            {
                return _selectedCanton;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectedCanton, value);
                    User.Canton = _selectedCanton.Value;
                    User.CodigoCanton = value.Key;
                    Canton = "Cantón : " + _selectedCanton.Value;
                }
            }
        }
        private string _canton;
        public string Canton
        {
            get
            {
                return _canton;
            }
            set
            {
                SetProperty(ref _canton, value);
            }
        }
        #endregion

        #region Provincia
        /// <summary>
        /// Propiedades del Picker para que el usuario establezca la provincia
        /// </summary>
        private List<ProvinciaEntity> listprovincia;
        public List<ProvinciaEntity> ListProvincia
        {
            get
            {
                return listprovincia;
            }
            set
            {
                listprovincia = value;
                OnPropertyChanged();
            }
        }

        private ProvinciaEntity _selectedProvincia;
        public ProvinciaEntity SelectedProvincia
        {
            get
            {
                return _selectedProvincia;
            }
            set
            {
                SetProperty(ref _selectedProvincia, value);
                User.Provincia = _selectedProvincia.Value;
                User.CodigoProvincia = value.Key;
                Provincia = "Provincia : " + _selectedProvincia.Value;
            }
        }
        private string _provincia;
        public string Provincia
        {
            get
            {
                return _provincia;
            }
            set
            {
                SetProperty(ref _provincia, value);
            }
        }
        #endregion

        #region Distrito
        /// <summary>
        /// Propiedades del Picker para que el usuario establezca el distrito
        /// </summary>
        private List<DistritoEntity> listdistritotemp;
        public List<DistritoEntity> ListDistritoTemp
        {
            get
            {
                return listdistritotemp;
            }
            set
            {
                listdistritotemp = value;
                OnPropertyChanged();
            }
        }

        private DistritoEntity _selectedDistrito;
        public DistritoEntity SelectedDistrito
        {
            get
            {
                return _selectedDistrito;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectedDistrito, value);
                    User.Distrito = _selectedDistrito.Value;
                    User.CodigoDistrito = value.Key;
                    Distrito = "Distrito : " + _selectedDistrito.Value;
                }
            }
        }
        private string _distrito;
        public string Distrito
        {
            get
            {
                return _distrito;
            }
            set
            {
                SetProperty(ref _distrito, value);
            }
        }
        #endregion
        #endregion

        #region Command
        public ICommand CreateAccountCommand { get; set; }
        public ICommand TakePictureCommand { get; set; }
        public ICommand PickPictureCommand { get; set; }
        public ICommand ClosingCommand { get; set; }
        #endregion

        public CreateAccountViewModel()
        {
            try
            {

                #region SizeFonts
                SizeFonts = FontService.ConsultarFont();
                validationString.SetMessage(Message);
                #endregion

                #region Commands
                //Inicializamos los commands
                CreateAccountCommand = new Command(async () => await ShowConfirmationMessage());
                TakePictureCommand = new Command(async () => await TakePicture());
                PickPictureCommand = new Command(async () => await PickPicture());
                ShowSucursalesPicker = new Command(async () => await ShowSucursales());
                ClosingCommand = new Command(async () => await Close(false));
                #endregion

                #region Pickers
                ListGender = PickerGeneroService.GetGeneros().OrderBy(c => c.Value).ToList();
                ListCantonTemp = new List<CantonEntity>();
                ListDistritoTemp = new List<DistritoEntity>();
                ListTipoIdentificacion = PickerTipoIdentificacionService.GetTipoIdentificaciones().OrderBy(c => c.Value).ToList();
                #endregion

                Fecha_Nacimiento = DateTime.Now;

                token = source.Token;
            }
            catch (Exception ex)
            {
                Task.Run(async () => 
                {
                    await Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
                });
            }
        }

        public void InicializarPassword() 
        {
            if (Title.Equals(ActionValues.ModificarUsuario))
            {
                EditarPassword = true;
                ModifyPassword = false;
            }
            else 
            {
                CrearPassword = true;
            }
        }

        public void SetPickerValues(List<ProvinciaEntity> Provincias,
                                    List<CantonEntity> Cantones,
                                    List<DistritoEntity> Distritos,
                                    List<Sucursal> Sucursales)
        {
            //Inicializar provinicias
            ListProvincia = Provincias;

            if (ListProvincia != null && ListProvincia.Count > 0)
            {
                FindSelectedProvincia();

                //Inicializar Cantones
                ListCanton = Cantones;

                SetCantonesChange();

                if (ListCanton != null && ListCanton.Count >0 )
                {
                    ListDistrito = Distritos;
                    SetDistritosChange();
                }
            }

            //Inicializar Sucursales
            foreach (var item in Sucursales)
            {
                this.ListSucursal.Add(item);
            }

            SetSucursalesValues();

        }

        public void SetCantonesChange()
        {
            ListCantonTemp = ListCanton.Where(x => x.IdProvincia == SelectedProvincia.Key).ToList();

            if (ListCantonTemp != null && ListCantonTemp.Count > 0)
            {
                FindSelectedCanton();
            }
        }

        public void SetDistritosChange()
        {
            ListDistritoTemp = ListDistrito.Where(x => x.IdCanton == SelectedCanton.Key).ToList();

            if (ListDistritoTemp != null && ListDistritoTemp.Count > 0)
            {
                FindSelectedDistrito();
            }
        }

        public async Task Close(bool Updated)
        {
            if (Title.Equals(ActionValues.ModificarUsuario))
            {
                if (!Temp_Password.Equals(User.Password))
                {
                    //Si la contraseña no es igual entonces debe encriptarse
                    TempUser.Password = await validationString.Encriptar(User.Password);
                }

                //Si no se actualizo correctamente entonces revertir
                if (!Updated)
                {
                    //Revertimos los cambios que pudieran producirse
                    User.RevertModel(TempUser);
                }
            }

            await Application.Current.MainPage.Navigation.PopAsync();
        }

        #region Sucursales
        /// <summary>
        /// Metodo que permite cambiar elementos básicos para la sucursales,
        /// de tal manera que el usuario pueda percibir los cambios
        /// </summary>
        public void SetSucursalesValues() {

            if (User != null)
            {
                if (!string.IsNullOrEmpty(User.Sucursal1))
                {
                    PlaceholderColor = Color.Black;
                    PlaceholderSucursal = User.Sucursal1;
                }
                else
                {
                    PlaceholderSucursal = ActionValues.SucursalesNotSelected;
                    PlaceholderColor = Color.Gray;
                }
            }
            else
            {
                PlaceholderSucursal = ActionValues.SucursalesNotSelected;
                PlaceholderColor = Color.Gray;
            }
        }

        /// <summary>
        /// Metodo que sirve para establecer las sucursales que un usuario ha seleccionado
        /// </summary>
        /// <param name="sucursales">La lista de sucursales selecciondas desde el
        /// pop up</param>
        public async Task SetSucursales(ObservableCollection<Sucursal> sucursales)
        {
            try
            {
                SelectedSucursal = sucursales;
                User.Sucursal1 = string.Empty;

                if (sucursales != null && sucursales.Count == 1)
                {
                    var sur = sucursales.FirstOrDefault();
                    User.Sucursal1 = sur.Descripcion_Centro;
                    User.CodigoSucursal1 = sur.Centro.ToString();
                }

                SetSucursalesValues();
            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
            }
        }

        /// <summary>
        /// Metodo que permite mostrar las sucursales para que el
        /// usuario pueda seleccionar dos
        /// </summary>
        /// <returns></returns>
        private async Task ShowSucursales()
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;
                await pantallas.showSucursalesPicker(ListSucursal, SelectedSucursal, this);
                IsBusy = false;
            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// Metodo que permite mostrar el mensaje solicitando la confirmación del usuario
        /// para proceder con la operación actual
        /// </summary>
        /// <returns></returns>
        private async Task ShowConfirmationMessage()
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;

                UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);

                if (await ValidateForm())
                {
                    if (await Utilities.VerifyInternetConnection())
                    {
                        if (string.IsNullOrEmpty(User.Cedula))
                        {
                            Message.attributes.Message = "En sus datos no esta proporcionando la cédula, ¿Está seguro que desea continuar con la operación?";
                        }
                        else 
                        {
                            Message.attributes.Message = "¿Está seguro que desea continuar con la operación?";
                        }

                        await pantallas.showConfirmationMessage(Message.GetMessageAttributes(), this);
                    }
                }

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

        /// <summary>
        /// Metodo que permite al usuario escoger una imagen de su galeria
        /// </summary>
        /// <returns></returns>
        private async Task PickPicture()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;

                    await CrossMedia.Current.Initialize();

                    if (!CrossMedia.Current.IsPickPhotoSupported)
                    {
                        await Message.Failed("Estimado usuario, el dispositivo actual no soporta esta característica.");
                        return;
                    }

                    var mediaOptions = new PickMediaOptions()
                    {
                        PhotoSize = PhotoSize.Large

                    };

                    IsBusy = false;

                    var selectedImage = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

                    UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);

                    if (selectedImage == null)
                        return;

                    Imagen = ImageSource.FromStream(() => selectedImage.GetStream());

                    file = selectedImage;

                    ModificarImagen = true;

                }

            }
            catch (Exception)
            {
                await Message.Failed("Estimado usuario, los permisos requeridos para esta acción no han sido activados.");
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        /// <summary>
        /// Metodo que le permite al usuario tomar una foto utilizando la cámara de su dispositivo
        /// y almacenarla en el mismo
        /// </summary>
        /// <returns></returns>
        private async Task TakePicture()
        {
            try
            {

                if (!IsBusy)
                {
                    IsBusy = true;

                    await CrossMedia.Current.Initialize();

                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                    {
                        await Message.Failed("No posee cámara disponible.");
                        return;
                    }

                    IsBusy = false;

                    var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        Directory = "Momentos Don Fernando",
                        Name = Guid.NewGuid().ToString(),
                        PhotoSize = PhotoSize.Large
                    });

                    UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);

                    if (file == null)
                        return;

                    //await Message.Successful("Ubicación del archivo: " + file.Path);

                    Imagen = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        return stream;
                    });

                    this.file = file;
                    ModificarImagen = true;
                }
            }
            catch (Exception)
            {
                await Message.Failed("Estimado usuario, los permisos requeridos para esta acción no han sido activados.");
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
                DatePickerSize();
                validationString.SetMessage(Message);
                Utilities.Message = Message;
            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario ocurrió un error. Código 209.");
            }
        }

        /// <summary>
        /// Redimemensionar el datetime picker segun el tamaño de fuente
        /// de letra seleccionado
        /// </summary>
        private void DatePickerSize()
        {
            int value = FontService.ConsultarSize();

            switch (value)
            {
                case 3:
                    Height = 60;
                    break;
                case 1:
                    Height = 42;
                    break;
                default:
                    Height = 50;
                    break;
            }
        }

        /// <summary>
        /// Metodo que permite crear una cuenta de usuario
        /// </summary>
        /// <param name="confirmacion">Permite verificar si es permitido crear cuenta
        /// segun la informacion que guarda un usuario</param>
        /// <returns></returns>
        public async Task CrearCuenta(bool confirmacion)
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;

                UserDialogs.Instance.ShowLoading(LogMessageAttention.Loading);

                if (confirmacion)
                {
                    if (file != null)
                    {
                        if (ModificarImagen)
                        {
                            //Validamos si ya existia una imagen
                            if (!string.IsNullOrEmpty(User.PictureName))
                            {
                                await TableStorageService.DeleteImage(User.PictureName);
                            }
                            //Se guarda la imagen en el repositorio en la nube
                            User.PictureURL = await TableStorage.ApplyPropertiesImage(file.Path);

                            //Obtenemos el nombre del archivo
                            User.PictureName = GetImageNameFromPath(file.Path);
                        }
                    }

                    if (Title.Equals(ActionValues.ModificarUsuario))
                    {
                        if (ModifyPassword)
                        {
                            User.Password = NewPassword;
                        }
                    }

                    User.Password = await validationString.Encriptar(User.Password);

                    bool result = false;

                    if (Title.Equals(ActionValues.ModificarUsuario))
                    {
                        result = await webApiService.ActualizarUser(User, token);
                    }
                    else
                    {
                        result = await webApiService.InsertarUser(User, token);
                    }
                    

                    if (result)
                    {
                        await Close(true);
                        await Message.Successful("Sus datos fueron ingresados correctamente.");

                    }
                    else
                    {
                        await Message.Failed("Algo salio mal, por favor revise su información.");
                    }
                }

                IsBusy = false;
            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario el tiempo de espera excedió el permitido, por favor intentelo más tarde.");
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        /// <summary>
        /// Metodo que perimte obtener el nombre de un archivo de su path de direccion
        /// </summary>
        /// <param name="path">string que contiene ubicación del archivo en el dispositivo</param>
        /// <returns></returns>
        private string GetImageNameFromPath(string path)
        {
            return Path.GetFileName(path);
        }

        private async Task<bool> ValidateForm()
        {

            if (!await validationString.ValidateEmail(User.Email))
            {
                User.Email = string.Empty;
                return false;
            }

            //Se modifica nombre por ordenes de Apple ya que en el logue recibe el nombre de cedula y no identificación: Guideline 5.1.1 - Legal - Privacy - Data Collection and Storage
            //Se hace opcional el campo de cedula
            if (!await validationString.ValidateCedula(User.Cedula))
            {
                User.Cedula = string.Empty;
                return false;
            }

            if (string.IsNullOrEmpty(User.Tipo_Cedula))
            {
                User.Tipo_Cedula = string.Empty;
                await Message.Failed("Estimado usuario, el campo del tipo de cédula (Momentos Don Fernando) es requerido para poder continuar con la operación actual.");
                return false;
            }

            if (string.IsNullOrEmpty(User.Nombre))
            {
                User.Nombre = string.Empty;
                await Message.Failed("Estimado usuario, el campo del nombre es requerido para poder continuar con la operación actual.");
                return false;
            }

            if (string.IsNullOrEmpty(User.Apellido))
            {
                User.Apellido = string.Empty;
                await Message.Failed("Estimado usuario, el campo de los apellidos es requerido para poder continuar con la operación actual.");
                return false;
            }

            //Se retira validación por ordenes de Apple: Guideline 5.1.1 - Legal - Privacy - Data Collection and Storage
            //if (string.IsNullOrEmpty(User.Fecha_Nacimiento))

            if (string.IsNullOrEmpty(User.Genero))
            {
                User.Genero = string.Empty;
                await Message.Failed("Estimado usuario, el campo del género es requerido para poder continuar con la operación actual.");
                return false;
            }

            if (! await validationString.VerificarTelefono(User.Telefono))
            {
                User.Telefono = string.Empty;
                return false;
            }

            if (string.IsNullOrEmpty(User.Sucursal1))
            {
                await Message.Failed("Estimado usuario, debe seleccionar la sucursal de su preferencia para poder continuar con la operación actual.");
                return false;
            }


            if (Title.Equals(ActionValues.ModificarUsuario))
            {
                if (ModifyPassword)
                {
                    if (User.Password != CurrentPassword)
                    {
                        CurrentPassword = string.Empty;
                        await Message.Failed("Estimado usuario, la contraseña actual no coincide, por favor verifique sus datos.");
                        return false;
                    }

                    if (!await validationString.ValidatePassword(NewPassword))
                    {
                        return false;
                    }

                    if (NewPassword != ConfirmarNewPassword)
                    {
                        ConfirmarPassword = string.Empty;
                        await Message.Failed("Estimado usuario, las contraseñas proporcionadas no coinciden.");
                        return false;
                    }

                }
            }
            else 
            {
                //Estamos creando al usuario
                if (!await validationString.ValidatePassword(User.Password))
                {
                    return false;
                }

                if (User.Password != ConfirmarPassword)
                {
                    ConfirmarPassword = string.Empty;
                    await Message.Failed("Estimado usuario, las contraseñas proporcionadas no coinciden.");
                    return false;
                }
            }

            if (!await Utilities.VerifyInternetConnection())
            {
                return false;
            }

            //Validamos que no este ingresado el mismo usuario con la misma cédula
            if (Title.Equals(ActionValues.CrearCuenta))
            {
                CancellationTokenSource source = new CancellationTokenSource(new TimeSpan(0, 1, 0));
                CancellationToken token = source.Token;
                UserModel user = null;

                if (!string.IsNullOrEmpty(User.Cedula))
                {
                    user = await TableStorage.GetUserCedulaEmail(User.Cedula, User.Email, token);
                }
                else 
                {
                    user = await TableStorage.GetUserMail(User.Email);
                }

                if (user != null)
                {
                    if ((user.Cedula == User.Cedula) && !string.IsNullOrEmpty(user.Cedula))
                    {
                        await Message.Failed("Estimado usuario, no es posible proceder a guardar sus datos porque ya existe un registro con la cédula proporcionada.");
                    }
                    else if (user.Email == User.Email)
                    {
                        await Message.Failed("Estimado usuario, no es posible proceder a guardar sus datos porque ya existe un registro con el correo electrónico proporcionado.");
                    }
                    else 
                    {
                        await Message.Failed("Estimado usuario, no es posible proceder a guardar sus datos porque ya existe un registro con los datos proporcionados.");
                    }

                    return false;
                }
            }else if (Title.Equals(ActionValues.ModificarUsuario)) 
            {
                List<UserModel> users;

                if (!string.IsNullOrEmpty(User.Cedula))
                {
                    users = await TableStorage.GetAllUserCedulaEmail(User.Cedula, User.Email);
                    var v1 = users?.Count(x => x.Cedula == User.Cedula);

                    //Si existe más de un usuario con esta cédula y la misma no es null
                    if (v1 > 0 && User.Cedula != TempUser?.Cedula)
                    {
                        await Message.Failed("Estimado usuario, no es posible proceder a guardar sus datos porque ya existe un registro con la cédula proporcionada.");
                        return false;
                    }
                }
                else
                {
                    users = await TableStorage.GetAllUserMail(User.Email);
                }

                //Si existe más de un usuario con este email y no es el que inicio el proceso
                if (users?.Where(x=> x.Email == User.Email).Count() > 0 && User.Email != TempUser?.Email)
                {
                    await Message.Failed("Estimado usuario, no es posible proceder a guardar sus datos porque ya existe un registro con el correo electrónico proporcionado.");
                    return false;

                }
            }

            return true;
        }

        /// <summary>
        /// Metodo encargado de marcar el genero seleccionado por el cliente en caso de
        /// ser una modificación de perfil
        /// </summary>
        public void FindSelectedGender()
        {
            if (!string.IsNullOrEmpty(User.Genero))
            {
                if (User.Genero == "Mujer")
                {
                    User.Genero = "Femenino";
                }

                if (User.Genero == "Hombre")
                {
                    User.Genero = "Masculino";
                }

                SelectedGender = ListGender.FirstOrDefault(x => x.Value == User.Genero);
            }
        }

        /// <summary>
        /// Metodo encargado de marcar el tipo de identificación seleccionada por el cliente en caso de
        /// ser una modificación de perfil
        /// </summary>
        public void FindSelectedTipoCedula()
        {
            if (!string.IsNullOrEmpty(User.Tipo_Cedula))
            {
                if (User.Tipo_Cedula == "Física")
                {
                    User.Tipo_Cedula = "Cédula Nacional";
                }

                SelectedTipoIdentificacion = ListTipoIdentificacion.FirstOrDefault(x => x.Value == User.Tipo_Cedula);
            }
        }

        /// <summary>
        /// Metodo encargado de marcar la provincia seleccionada por el cliente en caso de
        /// ser una modificación de perfil
        /// </summary>
        public void FindSelectedProvincia()
        {
            if (!string.IsNullOrEmpty(User.Provincia))
            {
                SelectedProvincia = ListProvincia.FirstOrDefault(x => x.Value == User.Provincia);
            }
            else
            {
                SelectedProvincia = ListProvincia.FirstOrDefault();
            }
        }

        /// <summary>
        /// Metodo encargado de marcar el cantón seleccionada por el cliente en caso de
        /// ser una modificación de perfil
        /// </summary>
        public void FindSelectedCanton()
        {
            if (!string.IsNullOrEmpty(User.Canton))
            {
                var canton = ListCantonTemp.FirstOrDefault(x => x.Value == User.Canton);

                if (canton == null)
                {
                    SelectedCanton = ListCantonTemp.FirstOrDefault();
                }
                else
                {
                    SelectedCanton = canton;
                }
            }
            else
            {
                SelectedCanton = ListCantonTemp.FirstOrDefault();
            }
        }

        /// <summary>
        /// Metodo encargado de marcar el distrito seleccionada por el cliente en caso de
        /// ser una modificación de perfil
        /// </summary>
        public void FindSelectedDistrito()
        {
            if (!string.IsNullOrEmpty(User.Distrito))
            {
                var distrito = ListDistritoTemp.FirstOrDefault(x => x.Value == User.Distrito);

                if (distrito == null)
                {
                    SelectedDistrito = ListDistritoTemp.FirstOrDefault();
                }
                else
                {
                    SelectedDistrito = distrito;
                }
            }
            else
            {
                SelectedDistrito = ListDistritoTemp.FirstOrDefault();
            }
        }

    }
}
