using System;
using System.Collections.Generic;
using System.Text;

namespace AppCarnesDF.Models.Sucursales
{
    public class PhoneNumber
    {
        public PhoneNumber()
        {

        }

        public PhoneNumber(string n)
        {
            Number = n;
        }
        public string Number { get; set; }
    }
}
