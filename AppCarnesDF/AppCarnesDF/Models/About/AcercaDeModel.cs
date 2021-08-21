using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCarnesDF.Models.About
{
    public class AcercaDeModel: TableEntity
    {
        public AcercaDeModel(string partitionkey, string rowkey)
        {
            this.PartitionKey = partitionkey;
            this.RowKey = rowkey;
        }

        public AcercaDeModel() {}
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Descripcion1 { get; set; }
        public string Descripcion2 { get; set; }
        public string Descripcion3 { get; set; }
        public string Descripcion4 { get; set; }
    }
}
