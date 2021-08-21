using AppCarnesDF.Models.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCarnesDF.Services.User
{
    public class PickerGeneroService
    {
        public static List<Genero> GetGeneros() {

            var generos = new List<Genero>()
            {
                new Genero {Key=1, Value="Masculino" },
                new Genero {Key=2, Value="Femenino" },
                new Genero {Key=3, Value="Prefiero no decir" }
            };

            return generos;
        }
    }
}
