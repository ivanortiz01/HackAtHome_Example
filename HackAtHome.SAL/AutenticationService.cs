using HackAtHome.Entities;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HackAtHome.SAL
{
    public class AutenticationService
    {


        /// <summary>
        /// Realiza la autenticación al servicio Web API.
        /// </summary>
        /// <param name="studentEmail">Correo del usuario</param>
        /// <param name="studentPassword">Password del usuario</param>
        /// <returns>Objeto ResultInfo con los datos del usuario y un token de autenticación.</returns>
        public async Task<ResultInfo> AutenticateAsync(string studentEmail, string studentPassword)
        {
            ResultInfo Result = null;
            // Dirección base de la Web API
            string WebAPIBaseAddress = "https://ticapacitacion.com/hackathome/";
            // ID del diplomado.
            string EventID = "xamarin30";

            string RequestUri = "api/evidence/Authenticate";
            // El servicio requiere un objeto UserInfo con los datos del usuario y evento.
            UserInfo User = new UserInfo
            {
                Email = studentEmail,
                Password = studentPassword,
                EventID = EventID
            };
            // Utilizamos el objeto System.Net.Http.HttpClient para consumir el servicio REST
            // Debe instalarse el paquete NuGet System.Net.Http
            using (var Client = new HttpClient())
            {
                // Establecemos la dirección base del servicio REST
                Client.BaseAddress = new Uri(WebAPIBaseAddress);
                // Limpiamos encabezados de la petición.
                Client.DefaultRequestHeaders.Accept.Clear();

                // Indicamos al servicio que envie los datos en formato JSON.
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    // Serializamos a formato JSON el objeto a enviar.
                    // Debe instalarse el paquete NuGet Newtonsoft.Json.
                    var JSONUserInfo = JsonConvert.SerializeObject(User);
                    // Hacemos una petición POST al servicio enviando el objeto JSON
                    HttpResponseMessage Response = await Client.PostAsync(RequestUri, new StringContent(JSONUserInfo.ToString(), Encoding.UTF8, "application/json"));
                    // Leemos el resultado devuelto.
                    var ResultWebAPI = await Response.Content.ReadAsStringAsync();
                    // Deserializamos el resultado JSON obtenido
                    Result = JsonConvert.DeserializeObject<ResultInfo>(ResultWebAPI);
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
            }
            return Result;
        }

    }
}
