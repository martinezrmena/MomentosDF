using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCarnesDF.Models.PoliticaPrivacidad
{
    public class PoliticaPrivacidadModel : TableEntity
    {
        public PoliticaPrivacidadModel(string partitionkey, string rowkey)
        {
            this.PartitionKey = partitionkey;
            this.RowKey = rowkey;
        }

        public PoliticaPrivacidadModel() { }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Descripcion1 { get; set; }
        public string Descripcion2 { get; set; }
        public string Descripcion3 { get; set; }
        public string Descripcion4 { get; set; }
    }
}
