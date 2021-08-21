using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCarnesDF.Models.Promotion
{
    public class PromotionModel : TableEntity
    {
        public PromotionModel(string skey, string srow)
        {
            this.PartitionKey = skey;
            this.RowKey = srow;

        }

        public PromotionModel(){}
        public string Titulo { get; set; }
        public string ImagenUrl { get; set; }
        public string Enlace { get; set; }
        public string Fecha_Publicacion { get; set; }
        public string Fecha_Finalizacion { get; set; }
    }
}
