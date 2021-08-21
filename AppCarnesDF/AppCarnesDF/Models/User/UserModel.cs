using AppCarnesDF.ViewModels;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace AppCarnesDF.Models.User
{
    public class UserModel: TableEntity, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public UserModel(string rowkey, string partition)
        {
            this.RowKey = rowkey;
            this.PartitionKey = partition;
            this.Puntos = string.Empty;
        }

        public UserModel()
        {
            this.Puntos = string.Empty;
        }

        public UserModel(UserModel temp)
        {
            RevertModel(temp);
        }

        public void RevertModel(UserModel temp)
        {
            this.RowKey = temp.RowKey;
            this.PartitionKey = temp.PartitionKey;
            this.Cedula = temp.Cedula;
            this.Password = temp.Password;
            this.Tipo_Cedula = temp.Tipo_Cedula;
            this.Nombre = temp.Nombre;
            this.Apellido = temp.Apellido;
            this.Email = temp.Email;
            this.Fecha_Nacimiento = temp.Fecha_Nacimiento;
            this.Genero = temp.Genero;
            this.Telefono = temp.Telefono;
            this.Provincia = temp.Provincia;
            this.Canton = temp.Canton;
            this.Distrito = temp.Distrito;
            this.Direccion_Exacta = temp.Direccion_Exacta;
            this.Sucursal1 = temp.Sucursal1;
            this.Codigo_Invitacion = temp.Codigo_Invitacion;
            this.PictureURL = temp.PictureURL;
            this.PictureName = temp.PictureName;
            this.Puntos = temp.Puntos;
            this.CodigoCanton = temp.CodigoCanton;
            this.CodigoProvincia = temp.CodigoProvincia;
            this.CodigoDistrito = temp.CodigoDistrito;
            this.CodigoSucursal1 = temp.CodigoSucursal1;
            this.CheckedPolitica = temp.CheckedPolitica;
            this.CheckedMensaje = temp.CheckedMensaje;
        }


        protected virtual void OnPropertyChanged([CallerMemberName]string propertyname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        private string cedula;
        public string Cedula
        {
            get { return cedula; }
            set
            {
                cedula = value;
                OnPropertyChanged();
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }

        private string tipo_cedula;
        public string Tipo_Cedula
        {
            get { return tipo_cedula; }
            set
            {
                tipo_cedula = value;
                OnPropertyChanged();
            }
        }

        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set
            {
                nombre = value;
                OnPropertyChanged();
            }
        }

        private string apellido;
        public string Apellido
        {
            get { return apellido; }
            set
            {
                apellido = value;
                OnPropertyChanged();
            }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }

        private string fecha_nacimiento;
        public string Fecha_Nacimiento
        {
            get { return fecha_nacimiento; }
            set
            {
                fecha_nacimiento = value;
                OnPropertyChanged();
            }
        }

        private string genero;
        public string Genero
        {
            get { return genero; }
            set
            {
                genero = value;
                OnPropertyChanged();
            }
        }

        private string telefono;
        public string Telefono
        {
            get { return telefono; }
            set
            {
                telefono = value;
                OnPropertyChanged();
            }
        }

        private string codigoprovincia;
        public string CodigoProvincia
        {
            get { return codigoprovincia; }
            set
            {
                codigoprovincia = value;
                OnPropertyChanged();
            }
        }

        private string provincia;
        public string Provincia
        {
            get { return provincia; }
            set
            {
                provincia = value;
                OnPropertyChanged();
            }
        }

        private string codigocanton;
        public string CodigoCanton
        {
            get { return codigocanton; }
            set
            {
                codigocanton = value;
                OnPropertyChanged();
            }
        }

        private string canton;
        public string Canton
        {
            get { return canton; }
            set
            {
                canton = value;
                OnPropertyChanged();
            }
        }

        private string codigodistrito;
        public string CodigoDistrito
        {
            get { return codigodistrito; }
            set
            {
                codigodistrito = value;
                OnPropertyChanged();
            }
        }

        private string distrito;
        public string Distrito
        {
            get { return distrito; }
            set
            {
                distrito = value;
                OnPropertyChanged();
            }
        }

        private string direccion_exacta;
        public string Direccion_Exacta
        {
            get { return direccion_exacta; }
            set
            {
                direccion_exacta = value;
                OnPropertyChanged();
            }
        }

        private string codigosucursal1;
        public string CodigoSucursal1
        {
            get { return codigosucursal1; }
            set
            {
                codigosucursal1 = value;
                OnPropertyChanged();
            }
        }

        private string sucursal1;
        public string Sucursal1
        {
            get { return sucursal1; }
            set
            {
                sucursal1 = value;
                OnPropertyChanged();
            }
        }

        private string codigo_invitacion;
        public string Codigo_Invitacion
        {
            get { return codigo_invitacion; }
            set
            {
                codigo_invitacion = value;
                OnPropertyChanged();
            }
        }

        private string pictureurl;

        public string PictureURL
        {
            get { return pictureurl; }
            set
            {
                pictureurl = value;
                OnPropertyChanged();
            }
        }

        private string picturename;

        public string PictureName
        {
            get { return picturename; }
            set
            {
                picturename = value;
                OnPropertyChanged();
            }
        }

        private string puntos;

        public string Puntos
        {
            get { return puntos; }
            set
            {
                puntos = value;
                OnPropertyChanged();
            }
        }

        public string Tipo_Transaccion { get; set; }

        public bool CheckedPolitica { get; set; }

        public bool CheckedMensaje { get; set; }
    }
}