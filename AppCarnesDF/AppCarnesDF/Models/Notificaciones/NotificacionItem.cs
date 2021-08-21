using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCarnesDF.Models.Notificaciones
{
    public class NotificacionItem
    {
        [PrimaryKey]
        public string Id { get; set; }
        public bool Activated { get; set; }
    }
}
