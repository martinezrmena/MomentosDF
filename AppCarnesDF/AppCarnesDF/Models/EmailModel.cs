using System;
using System.Collections.Generic;
using System.Text;

namespace AppCarnesDF.Models
{
    public class EmailModel
    {
        public string User { get; set; }
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Client { get; set; }
        public int Port { get; set; }
        public string Pass { get; set; }
    }
}
