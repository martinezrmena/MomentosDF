using AppCarnesDF.Helpers.Azure;
using AppCarnesDF.Models.User;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using AppCarnesDF.Helpers;

namespace AppCarnesDF.Services.User
{
    public class TableStorageService
    {
        #region Properties
        static CloudStorageAccount cloudStorageAccount;
        static CloudTableClient tableClient;
        static CloudTable UsersTable;
        public MediaFile file { get; set; }
        private CloudBlobClient cloudBlobClient;
        private CloudBlobContainer cloudBlobContainer;
        #endregion


        public TableStorageService()
        {
            cloudStorageAccount = CloudStorageAccount.Parse(AzureConstants.StorageAccountConnectionString);
            cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            cloudBlobContainer = cloudBlobClient.GetContainerReference("userimages");
        }

        /// <summary>
        /// Metodo incial que permite establecer conexión con el Azure Table
        /// correspondiente al metodo
        /// </summary>
        /// <returns></returns>
        private static async Task ConnectToTable()
        {
            cloudStorageAccount = CloudStorageAccount.Parse(AzureConstants.StorageAccountConnectionString);
            tableClient = cloudStorageAccount.CreateCloudTableClient();
            UsersTable = tableClient.GetTableReference(AzureConstants.UserTableName);
            await UsersTable.CreateIfNotExistsAsync();
        }

        /// <summary>
        /// Permite obtener una lista de todos los usuarios
        /// </summary>
        /// <returns></returns>
        public static async Task<List<UserModel>> GetUsers()
        {
            await ConnectToTable();

            TableContinuationToken continuationToken = null;
            var users = new List<UserModel>();

            try
            {
                do
                {
                    var result = await UsersTable.ExecuteQuerySegmentedAsync(new TableQuery<UserModel>(), continuationToken);
                    continuationToken = result.ContinuationToken;

                    if (result.Results.Count > 0)
                        users.AddRange(result.Results);
                } while (continuationToken != null);
            }
            catch (Exception ex)
            {

            }

            return users;
        }

        /// <summary>
        /// Permite obtener un usuario especifico por cedula y contraseña
        /// </summary>
        /// <param name="cedula">El identificador del modelo</param>
        /// <param name="password">un string cifrado</param>
        /// <returns>Un modelo de tipo UserModel</returns>
        public static async Task<UserModel> GetUser(string email, string password, CancellationToken token)
        {
            try
            {
                await ConnectToTable();

                // Definir el token de cancelación.
                TableContinuationToken continuationToken = null;
                var users = new List<UserModel>();

                // Create the table query.
                var condition = TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("Email", QueryComparisons.Equal, email),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("Password", QueryComparisons.Equal, password));

                var query = new TableQuery<UserModel>().Where(condition);


                do
                {
                    var result = await UsersTable.ExecuteQuerySegmentedAsync(query, continuationToken);
                    continuationToken = result.ContinuationToken;

                    if (result.Results.Count > 0)
                        users.AddRange(result.Results);
                } while (continuationToken != null && users.Count == 0);

                return users.FirstOrDefault();
            }
            catch (Exception)
            {
            }

            return null;
        }


        /// <summary>
        /// Permite obtener un usuario especifico por correo
        /// </summary>
        /// <param name="correo">El identificador del modelo</param>
        /// <returns>Un modelo de tipo UserModel</returns>
        public async Task<UserModel> GetUserMail(string correo)
        {
            try
            {
                try
                {
                    await ConnectToTable();

                    // Definir el token de cancelación.
                    TableContinuationToken continuationToken = null;
                    var users = new List<UserModel>();

                    // Create the table query.
                    var condition = TableQuery.GenerateFilterCondition("Email", QueryComparisons.Equal, correo);
                    var query = new TableQuery<UserModel>().Where(condition);

                    do
                    {
                        var result = await UsersTable.ExecuteQuerySegmentedAsync(query, continuationToken);
                        continuationToken = result.ContinuationToken;

                        if (result.Results.Count > 0)
                            users.AddRange(result.Results);
                    } while (continuationToken != null && users.Count == 0);

                    return users.FirstOrDefault();
                }
                catch (Exception)
                {
                }

                return null;
            }
            catch (Exception ex)
            {
            }

            return null;
        }

        /// <summary>
        /// Permite obtener un usuario especifico por correo
        /// </summary>
        /// <param name="correo">El identificador del modelo</param>
        /// <returns>Un modelo de tipo UserModel</returns>
        public async Task<List<UserModel>> GetAllUserMail(string correo)
        {
            try
            {
                try
                {
                    await ConnectToTable();

                    // Definir el token de cancelación.
                    TableContinuationToken continuationToken = null;
                    var users = new List<UserModel>();

                    // Create the table query.
                    var condition = TableQuery.GenerateFilterCondition("Email", QueryComparisons.Equal, correo);
                    var query = new TableQuery<UserModel>().Where(condition);

                    do
                    {
                        var result = await UsersTable.ExecuteQuerySegmentedAsync(query, continuationToken);
                        continuationToken = result.ContinuationToken;

                        if (result.Results.Count > 0)
                            users.AddRange(result.Results);
                    } while (continuationToken != null);

                    return users;
                }
                catch (Exception)
                {
                }

                return null;
            }
            catch (Exception ex)
            {
            }

            return null;
        }

        /// <summary>
        /// Permite obtener un usuario especifico por cedula o por correo
        /// </summary>
        /// <param name="cedula">El identificador del modelo</param>
        /// <param name="email">El email del modelo</param>
        /// <returns>Un modelo de tipo UserModel</returns>
        public async Task<UserModel> GetUserCedulaEmail(string cedula, string email, CancellationToken token)
        {
            try
            {
                await ConnectToTable();

                // Definir el token de cancelación.
                TableContinuationToken continuationToken = null;
                var users = new List<UserModel>();

                // Create the table query.
                var condition = TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("Cedula", QueryComparisons.Equal, cedula),
                    TableOperators.Or,
                    TableQuery.GenerateFilterCondition("Email", QueryComparisons.Equal, email));

                var query = new TableQuery<UserModel>().Where(condition);


                do
                {
                    var result = await UsersTable.ExecuteQuerySegmentedAsync(query, continuationToken);
                    continuationToken = result.ContinuationToken;

                    if (result.Results.Count > 0)
                        users.AddRange(result.Results);
                } while (continuationToken != null && users.Count == 0);

                return users.FirstOrDefault();
            }
            catch (Exception ex)
            {
            }

            return null;
        }

        /// <summary>
        /// Permite obtener un usuario especifico por cedula o por correo
        /// </summary>
        /// <param name="cedula">El identificador del modelo</param>
        /// <param name="email">El email del modelo</param>
        /// <returns>Un modelo de tipo UserModel</returns>
        public async Task<List<UserModel>> GetAllUserCedulaEmail(string cedula, string email)
        {
            try
            {
                await ConnectToTable();

                // Definir el token de cancelación.
                TableContinuationToken continuationToken = null;
                var users = new List<UserModel>();

                // Create the table query.
                var condition = TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("Cedula", QueryComparisons.Equal, cedula),
                    TableOperators.Or,
                    TableQuery.GenerateFilterCondition("Email", QueryComparisons.Equal, email));

                var query = new TableQuery<UserModel>().Where(condition);


                do
                {
                    var result = await UsersTable.ExecuteQuerySegmentedAsync(query, continuationToken);
                    continuationToken = result.ContinuationToken;

                    if (result.Results.Count > 0)
                        users.AddRange(result.Results);
                } while (continuationToken != null);

                return users;
            }
            catch (Exception ex)
            {
            }

            return null;
        }

        /// <summary>
        /// Metodo que permite actualizar o insertar un usuario en el Azure Table
        /// </summary>
        /// <param name="user">Modelo de tipo usuario</param>
        /// <returns>Un bool que indica si la operación fue exitosa</returns>
        public static async Task<bool> SaveUser(UserModel user)
        {
            try
            {
                await ConnectToTable();
                user.Timestamp = new DateTimeOffset();
                var operation = TableOperation.InsertOrMerge(user);
                var upsert = await UsersTable.ExecuteAsync(operation);
                return (upsert.HttpStatusCode == 204);
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        /// <summary>
        /// Metodo que permita eliminar un usuario
        /// </summary>
        /// <param name="user">Modelo de tipo usaurio</param>
        /// <returns>Un bool que indica si la operación fue exitosa</returns>
        public static async Task<bool> DeleteUser(UserModel user)
        {
            try
            {
                await ConnectToTable();
                var operation = TableOperation.Delete(user);
                var delete = await UsersTable.ExecuteAsync(operation);
                return (delete.HttpStatusCode == 204);
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        #region Guardar Imagen
        /// <summary>
        /// Permite insertar una imagen en el blob correspondiente
        /// </summary>
        /// <param name="_path">string que permite guardar la imagen por la dirección de la misma
        /// en el dispositivo</param>
        /// <returns>La url correspondiente a la imagen para almacenarla en el Azure Table</returns>
        public async Task<string> ApplyPropertiesImage(string _path)
        {
            string filePath = _path;
            string fileName = Path.GetFileName(filePath);
            await cloudBlobContainer.CreateIfNotExistsAsync();

            await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });
            var blockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
            return await UploadImage(blockBlob, filePath);
        }

        private async Task<string> UploadImage(CloudBlockBlob blob, string filePath)
        {
            string blobUrl = string.Empty;

            using (var fileStream = File.OpenRead(filePath))
            {
                await blob.UploadFromStreamAsync(fileStream);
                blobUrl = blob.Uri.AbsoluteUri;
            }

            return blobUrl;
        }

        #endregion


        /// <summary>
        /// Permite eleminar una imagen en caso de que el usuario la modifique
        /// </summary>
        /// <param name="name">Nombre de la imagen</param>
        /// <returns></returns>
        public static async Task DeleteImage(string name)
        {
            var _containerName = "userimages";
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(AzureConstants.StorageAccountConnectionString);
            CloudBlobClient _blobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer _cloudBlobContainer = _blobClient.GetContainerReference(_containerName);
            CloudBlockBlob _blockBlob = _cloudBlobContainer.GetBlockBlobReference(name);

            if (_blockBlob.ExistsAsync().Result)
            {
                await _blockBlob.DeleteAsync();
            }
            
        }

        #region No Utilizados
        private async void DownloadImage(string name)
        {
            string filePath = file.Path;
            string fileName = Path.GetFileName(filePath);
            var blockBlob = cloudBlobContainer.GetBlockBlobReference(name);
            await DownloadImage(blockBlob, filePath);
        }

        private static async Task DownloadImage(CloudBlockBlob blob, string filePath)
        {
            if (blob.ExistsAsync().Result)
            {
                await blob.DownloadToFileAsync(filePath, FileMode.CreateNew);
            }
        }
        #endregion

    }
}
