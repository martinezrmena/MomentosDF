using AppCarnesDF.Helpers;
using AppCarnesDF.Models.User;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppCarnesDF.Services.User
{
    public class PickerSucursalService
    {
        private readonly WebApiService webApiService = new WebApiService();

        public async Task<List<Sucursal>> GetSucursales()
        {
            CancellationTokenSource source = new CancellationTokenSource(new TimeSpan(0, 3, BrowserView.TimeOutLimit));
            CancellationToken token = source.Token;
            var sucursales = await webApiService.GetSucursales(token);

            return sucursales;
        }
    }
}
