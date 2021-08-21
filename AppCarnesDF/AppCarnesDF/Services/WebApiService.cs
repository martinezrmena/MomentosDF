using AppCarnesDF.Helpers;
using AppCarnesDF.Models;
using AppCarnesDF.Models.About;
using AppCarnesDF.Models.ActividadReciente;
using AppCarnesDF.Models.PlanLealtad;
using AppCarnesDF.Models.PoliticaPrivacidad;
using AppCarnesDF.Models.Products;
using AppCarnesDF.Models.Promotion;
using AppCarnesDF.Models.Sucursales;
using AppCarnesDF.Models.Ubicacion;
using AppCarnesDF.Models.User;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppCarnesDF.Services
{
    /// <summary>
    /// Para tener más información sobre lo que sucede, necesitamos una excepción personalizada.
    /// </summary>
    public class ApiException : Exception
    {
        public int StatusCode { get; set; }

        public string Content { get; set; }
    }

    /// <summary>
    /// Clase encargada de manejar todos los recursos provistos por la web api que
    /// alimenta a la aplicación
    /// </summary>
    public class WebApiService
    {
        #region Properties
#pragma warning disable S2223 // Non-constant static fields should not be visible
#pragma warning disable S1104 // Fields should not have public accessibility
#pragma warning disable S1075 // URIs should not be hardcoded
        public static string ApiUrl = "https://momentosdonfernandoapi.azurewebsites.net/api/";
#pragma warning restore S1075 // URIs should not be hardcoded
        public static string UserConsulta = "Usuarios/ConsultarUsuario";
        public static string UsersInserta = "Usuarios/Registra";
        public static string UsersActualiza = "Usuarios/Actualiza";
        public static string Promotion = "Promociones/Consulta";
        public static string Productos = "ProductosMes/Consulta";
        public static string Sucursales = "Sucursales/ConsultarSucursales";
        public static string SucursalesData = "Sucursales/ConsultarDatosSucursal";
        public static string Cantones = "Cantones/ConsultarCantones";
        public static string Distritos = "Distritos/ConsultarDistritos";
        public static string Provincias = "Provincias/ConsultarProvincias";
        public static string Imagenes = "AzureBlobStorage/SubirImagenApp";
        public static string AcercaDe = "AcercaDe/Consulta";
        public static string PoliticaPrivacidad = "PoliticaPrivacidad/Consulta";
        public static string PlanLealtad = "PlanLealtad/Consulta";
        public static string RecuperarPassword = "Usuarios/ConsultarPorCorreo";
        public static string SendEmail = "Usuarios/SendEmail";
        public static string ActividadReciente = "Puntos/Consulta";
        public static string Parametizaciones = "Parametrizacion/Consulta";
#pragma warning restore S1104 // Fields should not have public accessibility
#pragma warning restore S2223 // Non-constant static fields should not be visible
        #endregion

        #region Not Used
        public async Task<UserModel> GetUser(string Id, string Cedula)
        {

            string url = string.Format(ApiUrl + UserConsulta + "/{0}/{1}", Id, Cedula);

            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(url);
            var user = JsonConvert.DeserializeObject<UserModel>(response);

            return user;
        }

        public async Task<List<PromotionModel>> GetPromotions()
        {

            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(ApiUrl + Promotion);
            var promotions = JsonConvert.DeserializeObject<List<PromotionModel>>(response);

            return promotions;
        }
        #endregion

        #region Generic
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Objeto de tipo generico</typeparam>
        /// <param name="stream"></param>
        /// <returns>Objeto generico</returns>
        private static T DeserializeJsonFromStream<T>(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
                return default(T);

            using (var sr = new StreamReader(stream))
            using (var jtr = new JsonTextReader(sr))
            {
                var js = new JsonSerializer();
                var searchResult = js.Deserialize<T>(jtr);
                return searchResult;
            }
        }

        /// <summary>
        /// En caso de errores necesitamos verificar la información provista
        /// </summary>
        /// <param name="stream">Cadena provista</param>
        /// <returns>string que contiene información relativa a la excepción</returns>
        private static async Task<string> StreamToStringAsync(Stream stream)
        {
            string content = null;

            if (stream != null)
                using (var sr = new StreamReader(stream))
                    content = await sr.ReadToEndAsync();

            return content;
        }
        #endregion

        #region Imagenes
        public async Task<string> InsertImage(Stream streamFile, string _sFileName, string _sContainerName, CancellationToken cancellationToken)
        {
            string Url = ApiUrl + Imagenes;

            StreamReader reader = new StreamReader(streamFile);
            string text = reader.ReadToEnd();

            var httpClient = new HttpClient();
            JObject jsonData = new JObject
            {
                    { "streamFile", text},
                    { "_sFileName", _sFileName },
                    { "_sContainerName", _sContainerName }
            };

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.PostAsJsonAsync(Url, jsonData).Result;

            var stream = await response.Content.ReadAsStreamAsync();
            var content = await StreamToStringAsync(stream);


            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException
                {
                    StatusCode = (int)response.StatusCode,
                    Content = content
                };
            }

            return content;
        }


        #endregion

        #region User

        /// <summary>
        /// Metodo que permite devolver el usuario que corresponda
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<UserModel> GetUser(CancellationToken cancellationToken, string Email, string Password)
        {
            string Url = ApiUrl + UserConsulta;

            JObject jsonData = new JObject
            {
                    { "Email", Email},
                    { "Password", Password }
            };

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //Esperamos la respuesta
            var response = await client.PostAsJsonAsync(Url, jsonData, cancellationToken);
            //Recibimos el modelo
            var model = await response.Content.ReadAsAsync<UserModel>();
            //Obtenemos el contenido de la respuesta
            var stream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
                return model;

            if (!response.IsSuccessStatusCode)
            {
                var content = await StreamToStringAsync(stream);

                throw new ApiException
                {
                    StatusCode = (int)response.StatusCode,
                    Content = content
                };
            }

            return null;
        }

        /// <summary>
        /// Metodo utilizado para insertar un usuario por medio de la web api
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> InsertarUser(UserModel user, CancellationToken cancellationToken)
        {
            string Url = ApiUrl + UsersInserta;

            var httpClient = new HttpClient();

            var Json = JsonConvert.SerializeObject(user);
            HttpContent httpContent = new StringContent(Json);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/Json");
            httpClient.Timeout = new TimeSpan(0, 1, 30);
            var response = await httpClient.PostAsync(Url, httpContent);

            var stream = await response.Content.ReadAsStreamAsync();

            if (!response.IsSuccessStatusCode)
            {
                var content = await StreamToStringAsync(stream);

                throw new ApiException
                {
                    StatusCode = (int)response.StatusCode,
                    Content = content
                };
            }

            return true;
        }

        /// <summary>
        /// Metodo que sirve para actualizar al usuario
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> ActualizarUser(UserModel user, CancellationToken cancellationToken, bool fuePorpuntos = false)
        {
            string Url = ApiUrl + UsersActualiza;

            var httpClient = new HttpClient();

            var Json = JsonConvert.SerializeObject(user);
            HttpContent httpContent = new StringContent(Json);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/Json");
            httpClient.Timeout = new TimeSpan(0, 1, 30);
            if (fuePorpuntos)
            {
                httpClient.DefaultRequestHeaders.Add("PorPuntos", "Si");
            }

            var response = await httpClient.PostAsync(Url, httpContent);

            var stream = await response.Content.ReadAsStreamAsync();

            if (!response.IsSuccessStatusCode)
            {
                var content = await StreamToStringAsync(stream);

                throw new ApiException
                {
                    StatusCode = (int)response.StatusCode,
                    Content = content
                };
            }

            return true;
        }

        public async Task<bool> AddUser(UserModel user, CancellationToken cancellationToken)
        {
            string Url = ApiUrl + UsersInserta;

            var httpClient = new HttpClient();
            var Json = JsonConvert.SerializeObject(user);
            HttpContent httpContent = new StringContent(Json);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/Json");
            httpClient.Timeout = new TimeSpan(0, 1, 30);
            var response = await httpClient.PostAsync(Url, httpContent, cancellationToken);

            var stream = await response.Content.ReadAsStreamAsync();

            if (!response.IsSuccessStatusCode)
            {
                var content = await StreamToStringAsync(stream);

                throw new ApiException
                {
                    StatusCode = (int)response.StatusCode,
                    Content = content
                };
            }

            var res = await response.Content.ReadAsAsync<bool>();
            return res;
        }

        #endregion

        #region Ubicacion
        public async Task<List<DistritoModel>> GetDistritos(CancellationToken cancellationToken)
        {
            string Url = ApiUrl + Distritos;

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, Url))
            using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                var stream = await response.Content.ReadAsStreamAsync();

                if (response.IsSuccessStatusCode)
                    return DeserializeJsonFromStream<List<DistritoModel>>(stream);

                var content = await StreamToStringAsync(stream);

                throw new ApiException
                {
                    StatusCode = (int)response.StatusCode,
                    Content = content
                };
            }
        }

        public async Task<List<CantonModel>> GetCantones(CancellationToken cancellationToken)
        {
            string Url = ApiUrl + Cantones;

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, Url))
            using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                var stream = await response.Content.ReadAsStreamAsync();

                if (response.IsSuccessStatusCode)
                    return DeserializeJsonFromStream<List<CantonModel>>(stream);

                var content = await StreamToStringAsync(stream);

                throw new ApiException
                {
                    StatusCode = (int)response.StatusCode,
                    Content = content
                };
            }
        }

        public async Task<List<ProvinciaModel>> GetProvincias(CancellationToken cancellationToken)
        {
            string Url = ApiUrl + Provincias;

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, Url))
            using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                var stream = await response.Content.ReadAsStreamAsync();

                if (response.IsSuccessStatusCode)
                    return DeserializeJsonFromStream<List<ProvinciaModel>>(stream);

                var content = await StreamToStringAsync(stream);

                throw new ApiException
                {
                    StatusCode = (int)response.StatusCode,
                    Content = content
                };
            }
        }
        #endregion

        #region Promociones
        public async Task<List<PromotionModel>> GetPromotions(CancellationToken cancellationToken)
        {
            string Url = ApiUrl + Promotion;

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, Url))
            using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                var stream = await response.Content.ReadAsStreamAsync();

                if (response.IsSuccessStatusCode)
                    return DeserializeJsonFromStream<List<PromotionModel>>(stream);

                var content = await StreamToStringAsync(stream);

                throw new ApiException
                {
                    StatusCode = (int)response.StatusCode,
                    Content = content
                };
            }
        }
        #endregion

        #region Sucursales
        public async Task<List<Sucursal>> GetSucursales(CancellationToken cancellationToken)
        {
            string Url = ApiUrl + Sucursales;

            HttpClient client = new HttpClient();
            client.Timeout = new TimeSpan(0, 3, 0);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(Url).Result;

            var stream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
                return DeserializeJsonFromStream<List<Sucursal>>(stream);

            var content = await StreamToStringAsync(stream);

            throw new ApiException
            {
                StatusCode = (int)response.StatusCode,
                Content = content
            };
        }

        public async Task<List<SucursalesModel>> GetSucursalesData(CancellationToken cancellationToken)
        {
            string Url = ApiUrl + SucursalesData;

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, Url))
            using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                var stream = await response.Content.ReadAsStreamAsync();

                if (response.IsSuccessStatusCode)
                    return DeserializeJsonFromStream<List<SucursalesModel>>(stream);

                var content = await StreamToStringAsync(stream);

                throw new ApiException
                {
                    StatusCode = (int)response.StatusCode,
                    Content = content
                };
            }
        }
        #endregion

        #region Products
        public async Task<List<ProductModel>> GetProducts(CancellationToken cancellationToken)
        {
            string Url = ApiUrl + Productos;

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, Url))
            using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                var stream = await response.Content.ReadAsStreamAsync();

                if (response.IsSuccessStatusCode)
                    return DeserializeJsonFromStream<List<ProductModel>>(stream);

                var content = await StreamToStringAsync(stream);

                throw new ApiException
                {
                    StatusCode = (int)response.StatusCode,
                    Content = content
                };
            }
        }
        #endregion

        #region Acerca De
        public async Task<AcercaDeModel> GetAcercaDe(CancellationToken cancellationToken)
        {
            string Url = ApiUrl + AcercaDe;

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, Url))
            using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                var stream = await response.Content.ReadAsStreamAsync();

                if (response.IsSuccessStatusCode)
                    return DeserializeJsonFromStream<AcercaDeModel>(stream);

                var content = await StreamToStringAsync(stream);

                throw new ApiException
                {
                    StatusCode = (int)response.StatusCode,
                    Content = content
                };
            }
        }
        #endregion

        #region Politicas de Privacidad
        public async Task<PoliticaPrivacidadModel> GetPoliticaPrivacidad(CancellationToken cancellationToken)
        {
            string Url = ApiUrl + PoliticaPrivacidad;

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, Url))
            using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                var stream = await response.Content.ReadAsStreamAsync();

                if (response.IsSuccessStatusCode)
                    return DeserializeJsonFromStream<PoliticaPrivacidadModel>(stream);

                var content = await StreamToStringAsync(stream);

                throw new ApiException
                {
                    StatusCode = (int)response.StatusCode,
                    Content = content
                };
            }
        }
        #endregion

        #region Plan Lealtad
        public async Task<List<PlanLealtadModel>> GetPlanesLealtad(CancellationToken cancellationToken)
        {
            string Url = ApiUrl + PlanLealtad;

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, Url))
            using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                var stream = await response.Content.ReadAsStreamAsync();

                if (response.IsSuccessStatusCode)
                    return DeserializeJsonFromStream<List<PlanLealtadModel>>(stream);

                var content = await StreamToStringAsync(stream);

                throw new ApiException
                {
                    StatusCode = (int)response.StatusCode,
                    Content = content
                };
            }
        }
        #endregion

        #region Recuperar Password
        public async Task<bool> RecuperarPasswordMethod(string _password, string _email, CancellationToken cancellationToken)
        {
            string Url = ApiUrl + RecuperarPassword;
            JObject jsonData = new JObject
            {
                    { "_password", _password},
                    { "_email", _email }
            };

            HttpClient client = new HttpClient();
            client.Timeout = new TimeSpan(0,3,0);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.PostAsJsonAsync(Url, jsonData);

            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadAsAsync<bool>();
                return res;
            }

            var stream = await response.Content.ReadAsStreamAsync();

            var content = await StreamToStringAsync(stream);

            throw new ApiException
            {
                StatusCode = (int)response.StatusCode,
                Content = content
            };
        }
        #endregion

        #region Actividad Reciente
        public async Task<List<ActividadRecienteModel>> GetActividadReciente(string cedula, CancellationToken cancellationToken)
        {
            string Url = ApiUrl + ActividadReciente;

            JObject jsonData = new JObject
            {
                    { "Cedula", cedula}
            };

            HttpClient client = new HttpClient();
            client.Timeout = new TimeSpan(0, 3, 0);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.PostAsJsonAsync(Url, jsonData).Result;

            var stream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
                return DeserializeJsonFromStream<List<ActividadRecienteModel>>(stream);

            var content = await StreamToStringAsync(stream);

            throw new ApiException
            {
                StatusCode = (int)response.StatusCode,
                Content = content
            };
        }
        #endregion

        #region Parametizaciones
        public async Task<ParametizacionesModel> GetParametizaciones(CancellationToken cancellationToken)
        {
            string Url = ApiUrl + Parametizaciones;

            HttpClient client = new HttpClient();
            client.Timeout = new TimeSpan(0, 3, 0);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(Url).Result;

            var stream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
                return DeserializeJsonFromStream<ParametizacionesModel>(stream);

            var content = await StreamToStringAsync(stream);

            throw new ApiException
            {
                StatusCode = (int)response.StatusCode,
                Content = content
            };
        }
        #endregion

        #region Email
        public async Task<int> SendEmailApi(string User, string Recipient, 
                                          string Subject, string Body, 
                                          string Client, int Port, 
                                          string Pass)
        {
            string Url = ApiUrl + SendEmail;

            EmailModel emailModel = new EmailModel();
            emailModel.User = User;
            emailModel.Recipient = Recipient;
            emailModel.Subject = Subject;
            emailModel.Body = Body;
            emailModel.Client = Client;
            emailModel.Port = Port;
            emailModel.Pass = Pass;

            HttpClient client = new HttpClient();
            client.Timeout = new TimeSpan(0, 3, 0);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.PostAsJsonAsync(Url, emailModel);

            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadAsAsync<int>();
                return res;
            }

            var stream = await response.Content.ReadAsStreamAsync();

            var content = await StreamToStringAsync(stream);

            throw new ApiException
            {
                StatusCode = (int)response.StatusCode,
                Content = content
            };
        }
        #endregion
    }
}
