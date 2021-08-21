using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppCarnesDF.Models.PlanLealtad
{
    public class PlanLealtadModel: TableEntity
    {

        public PlanLealtadModel(string partitionkey, string rowkey)
        {
            this.PartitionKey = partitionkey;
            this.RowKey = rowkey;
            BackgroundColor = Color.White;
        }

        public string Foto { get; set; }
        public string TipoCliente { get; set; }
        public string Descripcion { get; set; }
        public string Beneficios { get; set; }
        public string BonoNivel { get; set; }
        public Color BackgroundColor { get; set; }

        public PlanLealtadModel() {

            BackgroundColor = Color.White;
        }

    }
}
