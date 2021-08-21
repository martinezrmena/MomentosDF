using System;
using System.Collections.Generic;
using System.Text;

namespace AppCarnesDF.Models.Products
{
    public class PreparacionModel
    {
        public PreparacionModel(string url, string name)
        {
            ImagenURL = url;
            ImagenName = name;
        }

        public PreparacionModel()
        {

        }

        public string ImagenURL { get; set; }
        public string ImagenName { get; set; }


        public const string Coccion = "Cocción Liquida";
        public const string Frio = "Frío";
        public const string Horno = "Horno";
        public const string Parrilla = "Parrilla";
        public const string Sarten = "Sartén";
    }
}
