using AppCarnesDF.Helpers;
using AppCarnesDF.Models.Ubicacion;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppCarnesDF.Services.Ubicacion
{
    public class PickerDistritoService
    {
        private readonly WebApiService webApiService = new WebApiService();

        public async Task<List<DistritoEntity>> GetDistritos()
        {
            CancellationTokenSource source = new CancellationTokenSource(new TimeSpan(0, 1, BrowserView.TimeOutLimit));
            CancellationToken token = source.Token;
            List<DistritoEntity> distritosPicker = new List<DistritoEntity>();
            var distritosModel = await webApiService.GetDistritos(token);

            if (distritosModel != null)
            {
                foreach (var item in distritosModel)
                {
                    distritosPicker.Add(new DistritoEntity { Key = item.Distrito, Value = item.Descripcion, IdCanton = item.Canton });
                }
            }

            return distritosPicker;
        }
    }
}
