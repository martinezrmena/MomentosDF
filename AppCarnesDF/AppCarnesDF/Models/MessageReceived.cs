using System;
using System.Collections.Generic;
using System.Text;

namespace AppCarnesDF.Models
{
    public class MessageReceived
    {
        public bool Respuesta { get; set; }

        public MessageReceived()
        {
            Respuesta = false;
        }
    }
}
