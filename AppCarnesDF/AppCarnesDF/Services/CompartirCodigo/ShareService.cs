using AppCarnesDF.Helpers.Azure;
using AppCarnesDF.Models.Share;
using AppCarnesDF.Services.Notificaciones;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCarnesDF.Services.CompartirCodigo
{
    public class ShareService
    {
        #region Properties
        public ObservableCollection<ShareModel> n_config { get; set; }
        public ShareDataBase db { get; set; }

        static CloudStorageAccount cloudStorageAccount;
        static CloudTableClient tableCompartir;
        static CloudTable CompartirTable;
        #endregion

        #region Share
        /// <summary>
        /// Metodo incial que permite establecer conexión con el Azure Table
        /// correspondiente al metodo, si la tabla no existe el la crea
        /// </summary>
        /// <returns></returns>
        private static async Task ConnectToTable()
        {
            cloudStorageAccount = CloudStorageAccount.Parse(AzureConstants.StorageAccountConnectionString);
            tableCompartir = cloudStorageAccount.CreateCloudTableClient();
            CompartirTable = tableCompartir.GetTableReference(AzureConstants.CompartirTableName);
            await CompartirTable.CreateIfNotExistsAsync();
        }

        /// Metodo que permite insertar la configuracion del share en el Azure Table
        /// </summary>
        /// <param name="movimiento">Modelo de tipo usuario</param>
        /// <returns>Un bool que indica si la operación fue exitosa</returns>
        public async Task<bool> SaveShare(ShareModel share)
        {
            try
            {
                await ConnectToTable();
                share.Timestamp = new DateTimeOffset();
                var operation = TableOperation.InsertOrMerge(share);
                var upsert = await CompartirTable.ExecuteAsync(operation);
                return (upsert.HttpStatusCode == 204);
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        /// <summary>
        /// Permite obtener el compartir de un cliente
        /// </summary>
        /// <param name="idusuario">la cedula del usuario</param>
        /// <returns></returns>
        public async Task<ShareModel> GetShare(string idusuario)
        {
            try
            {
                await ConnectToTable();

                TableContinuationToken continuationToken = null;

                // Create the table query.
                var condition = TableQuery.GenerateFilterCondition("Id", QueryComparisons.Equal, idusuario);

                var query = new TableQuery<ShareModel>().Where(condition);
                var lst = await CompartirTable.ExecuteQuerySegmentedAsync(query, continuationToken);

                var share = new ShareModel();

                share = lst.Results.FirstOrDefault();

                return share;
            }
            catch (Exception)
            {
            }

            return null;
        }
        #endregion

        public ShareService()
        {
            if (db == null)
            {
                db = new ShareDataBase();
            };

            if (n_config == null)
            {
                n_config = new ObservableCollection<ShareModel>();
                TraerLista();
            }
        }

        public ObservableCollection<ShareModel> Consultar()
        {
            return n_config;
        }

        public bool ConsultarConfig()
        {
            TraerLista();
            var modelo = n_config.FirstOrDefault();
            bool value = modelo == null ? true : modelo.Saved;

            return value;
        }

        private ShareModel ConvertirDataBaseAModelo(ShareItem data)
        {
            ShareModel modelo = new ShareModel()
            {
                Id = data.Id,
                Saved = data.Saved
            };

            return modelo;
        }

        private ShareItem ConvertirModeloADataBase(ShareModel model)
        {
            ShareItem data = new ShareItem()
            {
                Id = model.Id,
                Saved = model.Saved
            };

            return data;
        }

        public void TraerLista()
        {
            n_config.Clear();
            List<ShareItem> Lista = db.GetItemsAsync();
            foreach (var item in Lista)
            {
                n_config.Add(ConvertirDataBaseAModelo(item));
            }
        }

        public int Guardar(ShareModel modelo)
        {
            var Data = ConvertirModeloADataBase(modelo);
            int resultados = db.SaveItemAsync(Data);
            TraerLista();
            return resultados;
        }

        public int Modificar(ShareModel modelo)
        {
            var Data = ConvertirModeloADataBase(modelo);
            int resultados = db.UpdateItemAsync(Data);
            TraerLista();
            return resultados;
        }

        public int Eliminar(string IdConfig)
        {
            var Data = db.GetItemAsync(IdConfig);
            int Resultados = db.DeleteItemAsync(Data);
            TraerLista();
            return Resultados;
        }
    }
}
