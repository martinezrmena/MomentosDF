using AppCarnesDF.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace AppCarnesDF.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public ConvertFontSizeBD FontSizeBD = new ConvertFontSizeBD();

        public event PropertyChangedEventHandler PropertyChanged;

        public CarnesDF pantallas = new CarnesDF();

        private double sizefonts;
        public double SizeFonts
        {
            get { return sizefonts; }
            set
            {
                SetProperty(ref sizefonts, value);
            }
        }

        private double sizefontscookie;
        public double SizeFontsCookie
        {
            get { return sizefontscookie; }
            set
            {
                SetProperty(ref sizefontscookie, value);
            }
        }

        private double sizefontsoptima;
        public double SizeFontsOptima
        {
            get { return sizefontsoptima; }
            set
            {
                SetProperty(ref sizefontsoptima, value);
            }
        }

        private bool isbusy = false;
        public bool IsBusy
        {
            get { return isbusy; }
            set
            {
                SetProperty(ref isbusy, value);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        /// <summary>
        /// Metodo que permite enviar información a un control cuando se produce un cambio en alguna propiedad,
        /// siempre y cuando utilice un binding
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="backfield"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetProperty<T>(ref T backfield, T value, [CallerMemberName]string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backfield, value))
            {
                return false;
            }
            backfield = value;
            OnPropertyChanged(propertyName);
            return true;
        }

    }
}