using AppCarnesDF.Helpers;
using AppCarnesDF.Models.Ubicacion;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppCarnesDF.Services.Ubicacion
{
    public class PickerProvinciaService
    {

        private readonly WebApiService webApiService = new WebApiService();

        public async Task<List<ProvinciaEntity>> GetProvincias()
        {
            CancellationTokenSource source = new CancellationTokenSource(new TimeSpan(0, 1, BrowserView.TimeOutLimit));
            CancellationToken token = source.Token;
            List<ProvinciaEntity> provinciasPicker = new List<ProvinciaEntity>();
            var provinciasModel = await webApiService.GetProvincias(token);

            if (provinciasModel != null)
            {
                foreach (var item in provinciasModel)
                {
                    provinciasPicker.Add(new ProvinciaEntity { Key = item.Provincia, Value = item.Descripcion });
                }
            }

            return provinciasPicker;
        }

    }
}
