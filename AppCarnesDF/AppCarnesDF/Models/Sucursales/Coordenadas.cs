using System;
using System.Collections.Generic;
using System.Text;

namespace AppCarnesDF.Models.Sucursales
{
    public class Coordenadas
    {
        public double Latitude { get; set; }
        public double Longitud { get; set; }

        public string GoogleURL { get; set; }
        public string WazeURL { get; set; }
    }
}
