using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCarnesDF.Models
{
    public class ParametizacionesModel: TableEntity
    {
        public ParametizacionesModel(string partitionkey, string rowkey)
        {
            this.PartitionKey = partitionkey;
            this.RowKey = rowkey;
        }
        public ParametizacionesModel() { }

        public string User { get; set; }
        public string Pass { get; set; }
        public string Client { get; set; }
        public int Port { get; set; }
    }
}
