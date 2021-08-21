using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCarnesDF.Models.FontSizes
{
    public class FontSizeItem
    {
        [PrimaryKey]
        public string Id { get; set; }
        public int Font { get; set; }

    }
}
