using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCarnesDF.Models.Products
{
    public class ProductCategoryModel : TableEntity
    {
        public ProductCategoryModel(string skey, string srow)
        {
            this.PartitionKey = skey;
            this.RowKey = srow;
        }

        public ProductCategoryModel() { }
        public string Category { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string ImagenUrl { get; set; }
    }
}