using System;
using System.Collections.Generic;
using System.Text;

namespace AppCarnesDF.Services
{
    //Interfaz encargada de manipular el path de dirección
    //donde se almacerá la base de datos en los dispostivos
    public interface IFileHelper
    {
        string GetLocalFilePath(string filename);
    }
}
