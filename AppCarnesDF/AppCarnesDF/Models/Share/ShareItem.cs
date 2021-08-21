using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCarnesDF.Models.Share
{
    public class ShareItem
    {
        [PrimaryKey]
        public string Id { get; set; }
        public bool Saved { get; set; }
    }
}
