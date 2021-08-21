using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCarnesDF.Models.Permissions
{
    public class AllowRegisterItem
    {
        [PrimaryKey]
        public string Id { get; set; }
        public bool Activated { get; set; }
    }
}
