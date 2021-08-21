using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCarnesDF.Models.User
{
    public class UserItem
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string Cedula { get; set; }
        public string Tipo_Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Fecha_Nacimiento { get; set; }
        public string Genero { get; set; }
        public string Telefono { get; set; }
        public string Provincia { get; set; }
        public string Canton { get; set; }
        public string Direccion_Exacta { get; set; }
        public string Sucursal { get; set; }
        public string Codigo_Invitacion { get; set; }

        public string Picture { get; set; }
    }
}