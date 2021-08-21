using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCarnesDF.Models.Products
{
    public class ProductModel : TableEntity
    {
        public ProductModel(string skey, string srow)
        {
            this.PartitionKey = skey;
            this.RowKey = srow;
        }

        public ProductModel() { }
        public string Titulo { get; set; }
        public string Fecha_Publicacion { get; set; }
        public string Fecha_Finalizacion { get; set; }
        public string ImagenUrl { get; set; }
        public string Descripcion_Resumen { get; set; }
        public string Descripcion_Detalle { get; set; }
        public string Preparacion { get; set; }
        public string UrlIconosPreparacion { get; set; }
    }
}
