using AppCarnesDF.Helpers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AppCarnesDF.Services
{
    /// <summary>
    /// Clase encargada de contener metodos que permitan realizar acciones
    /// genericas que puedan servir para la aplicación
    /// </summary>
    public class GeneralUtilities
    {
        public LogMessageAttention Message = new LogMessageAttention();

        /// <summary>
        /// Metodo encargado de verificar si el dispositivo posee conexión
        /// disponible a internet
        /// </summary>
        /// <returns>bool que define si se recibe conexión a internet o no</returns>
        public async Task<bool> VerifyInternetConnection()
        {
            var current = Connectivity.NetworkAccess;
            var profiles = Connectivity.ConnectionProfiles;
            bool connection = false;

            if (current == NetworkAccess.Internet || profiles.Contains(ConnectionProfile.WiFi))
            {
                connection = true;
            }

            if (!connection)
            {
                await Message.generalAttention("Estimado usuario, no posee una conexión estable a Internet.");
            }

            return connection;
        }

        /// <summary>
        /// Metodo encargado de verificar si el dispositivo posee conexión
        /// disponible a internet
        /// </summary>
        /// <returns>bool que define si se recibe conexión a internet o no</returns>
        public bool VerifyInternetConnection2()
        {
            var current = Connectivity.NetworkAccess;
            var profiles = Connectivity.ConnectionProfiles;
            bool connection = false;

            if (current == NetworkAccess.Internet || profiles.Contains(ConnectionProfile.WiFi))
            {
                connection = true;
            }

            return connection;
        }

    }
}
