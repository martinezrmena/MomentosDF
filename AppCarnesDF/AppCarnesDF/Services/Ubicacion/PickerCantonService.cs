using AppCarnesDF.Helpers;
using AppCarnesDF.Models.Ubicacion;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppCarnesDF.Services.Ubicacion
{
    public class PickerCantonService
    {
        private readonly WebApiService webApiService = new WebApiService();

        public async Task<List<CantonEntity>> GetCantones()
        {
            CancellationTokenSource source = new CancellationTokenSource(new TimeSpan(0, 1, BrowserView.TimeOutLimit));
            CancellationToken token = source.Token;
            List<CantonEntity> cantonesPicker = new List<CantonEntity>();
            var cantonesModel = await webApiService.GetCantones(token);

            if (cantonesModel != null)
            {
                foreach (var item in cantonesModel)
                {
                    cantonesPicker.Add(new CantonEntity { Key = item.Canton, Value = item.Descripcion, IdProvincia = item.Provincia });
                }
            }

            return cantonesPicker;
        }
    }
}
