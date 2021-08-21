using AppCarnesDF.Models.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCarnesDF.Services.User
{
    public class PickerTipoIdentificacionService
    {
        public static List<TipoIdentificacionModel> GetTipoIdentificaciones()
        {

            var tipoidentificaciones = new List<TipoIdentificacionModel>()
            {
                new TipoIdentificacionModel {Key=1, Value="Cédula Nacional" },
                new TipoIdentificacionModel {Key=2, Value="Extranjero (Dimex)" }
            };

            return tipoidentificaciones;
        }
    }
}
