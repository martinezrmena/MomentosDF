using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCarnesDF.Models.Sucursales
{
    public class SucursalesModel: TableEntity
    {
        public SucursalesModel(string partitionkey, string rowkey)
        {
            this.PartitionKey = partitionkey;
            this.RowKey = rowkey;
        }
        public SucursalesModel(){}

        public string FotoUrl { get; set; }
        public string Nombre { get; set; }
        public string EnlaceGoogleMaps { get; set; }
        public string IconoGoogleMaps { get; set; }
        public string EnlaceWaze { get; set; }
        public string IconoWaze { get; set; }
        public string TelefonoTienda { get; set; }
        public string TelefonoRestaurante { get; set; }
        public string HorarioTienda { get; set; }
        public string HorarioRestaurante { get; set; }
        public string Telefonos { get; set; }
    }
}
