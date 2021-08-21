using AppCarnesDF.Helpers.Common;
using AppCarnesDF.Models.FontSizes;
using AppCarnesDF.Services.FontSize;
using Rg.Plugins.Popup.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppCarnesDF.ViewModels.UpdateFontSizes
{
    public class UpdateFontSizesViewModel: FontSizeModel
    {
        #region Properties

        public LogMessageAttention messages = new LogMessageAttention();

        private readonly FontSizeService servicio = new FontSizeService();

        string labelInit = "La fuente seleccionada es: ";

        public Views.MainPage MainPageView { get; set; }

        private int actualstep;

        public int ActualStep
        {
            get {
                return actualstep;
            }
            set {
                SetProperty(ref actualstep, value);
                UpdateSizeFont();
                OnPropertyChanged();
            }
        }

        private string step;

        public string Step
        {
            get {
                return step;
            }
            set {
                SetProperty(ref step, value);
            }
        }
        #endregion

        #region Commands
        public ICommand BackCommand { get; private set; }
        public ICommand SmallCommand { get; private set; }
        public ICommand MediumCommand { get; private set; }
        public ICommand LargeCommand { get; private set; }
        #endregion

        public UpdateFontSizesViewModel()
        {
            try
            {
                BackCommand = new Command(async () => await BackAction());
                SmallCommand = new Command(async () => await SmallSize());
                MediumCommand = new Command(async () => await MediumSize());
                LargeCommand = new Command(async () => await LargeSize());

                //Consultamos si ya hay algun tipo de datos almacenados en BD
                var modelo = servicio.Consultar().FirstOrDefault();
                if (modelo != null)
                {
                    Font = modelo.Font;
                    Id = modelo.Id;
                }
                ActualStep = Font;
                SizeFonts = FontSizeBD.GetFontSize(ActualStep);

            }
            catch (Exception ex)
            {
                Task.Run(async () =>
                {
                    await messages.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
                });
            }

        }

        private async Task LargeSize()
        {
            ActualStep = 3;
            await GuardarInit();
        }

        private async Task MediumSize()
        {
            ActualStep = 2;
            await GuardarInit();
        }

        private async Task SmallSize()
        {
            ActualStep = 1;
            await GuardarInit();
        }

        private async Task BackAction()
        {
            await PopupNavigation.Instance.PopAsync(true);
        }

        /// <summary>
        /// Metodo que controla el tipo de guardado segun la información desde BD,
        /// puede guardar o modificar
        /// </summary>
        /// <returns></returns>
        private async Task GuardarInit()
        {
            try
            {
                if (IsBusy)
                    return;

                if (!string.IsNullOrEmpty(Id))
                {
                    Modificar();
                }
                else
                {
                    Guardar();
                }

                await messages.Successful("La configuración ha sido guardada con éxito.");
            }
            catch (Exception ex)
            {
                await messages.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
            }
        }

        /// <summary>
        /// Metodo que permite actualizar el tamaño de la fuente en la aplicación
        /// </summary>
        public void UpdateSizeFont()
        {
            try
            {
                Step = labelInit + FontSizeBD.GetFontLabel(ActualStep);
                SizeFonts = FontSizeBD.GetFontSize(ActualStep);
                SizeFontsCookie = FontSizeBD.GetFontSizeCookie(ActualStep);
                SizeFontsOptima = FontSizeBD.GetFontSizeOptima(ActualStep);
                messages.SizeFonts = SizeFonts;
                messages.SizeFontsCookie = SizeFontsCookie;
                messages.SizeFontsOptima = SizeFontsOptima;
            }
            catch (Exception ex)
            {
                Task.Run(async () =>
                {
                    await messages.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
                });
                
            }
            
        }

        private void Guardar()
        {
            IsBusy = true;

            Guid IdFont = Guid.NewGuid();

            Font = ActualStep;
            Id = IdFont.ToString();

            FontSizeModel modelo = new FontSizeModel()
            {
                Font = Font,
                Id = Id
            };

            servicio.Guardar(modelo);

            IsBusy = false;
        }

        private void Modificar()
        {
            IsBusy = true;

            Font = ActualStep;

            FontSizeModel modelo = new FontSizeModel()
            {
                Font = Font,
                Id = Id
            };

            servicio.Modificar(modelo);

            IsBusy = false;
        }

    }

}
