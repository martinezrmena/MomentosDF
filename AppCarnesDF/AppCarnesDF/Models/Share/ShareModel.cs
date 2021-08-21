using AppCarnesDF.ViewModels;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCarnesDF.Models.Share
{
    public class ShareModel : TableEntity
    {
        public ShareModel(string Partition, string Row)
        {
            this.PartitionKey = Partition;
            this.RowKey = Row;
        }

        public ShareModel() { }

        private string id;
        public string Id
        {
            get { return id; }
            set
            {
                id = value;
            }
        }

        private bool saved;
        public bool Saved
        {
            get { return saved; }
            set
            {
                saved = value;
            }
        }
    }
}
