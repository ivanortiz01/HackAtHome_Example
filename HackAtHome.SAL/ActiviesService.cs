using HackAtHome.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HackAtHome.SAL
{
    public class ActiviesService
    {
        // Dirección base de la Web API
        private static string WebAPIBaseAddress = "https://ticapacitacion.com/hackathome/";
        // ID del diplomado
        private static string EventID = "xamarin30";

        /// <summary>
        /// Obtiene la lista de evidencias.
        /// </summary>
        /// <param name="token">Token de autenticación del usuario.</param>
        /// <returns>Una lista con las evidencias.</returns>
        public async Task<List<Evidence>> GetEvidencesAsync(string token)
        {
            List<Evidence> Evidences = null;

            // Dirección del servicio REST
            string URI = $"{WebAPIBaseAddress}api/evidence/getevidences?token={token}";

            using (var Client = new HttpClient())
            {
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    // Realizamos una petición GET
                    var Response =
                            await Client.GetAsync(URI);

                    if (Response.StatusCode == HttpStatusCode.OK)
                    {
                        // Si el estatus de la respuesta HTTP fue exitosa, leemos el valor devuelto.
                        var ResultWebAPI = await Response.Content.ReadAsStringAsync();
                        Evidences = JsonConvert.DeserializeObject<List<Evidence>>(ResultWebAPI);
                    }
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
            }
            return Evidences;
        }

        /// <summary>
        /// Obtiene el detalle de una evidencia.
        /// </summary>
        /// <param name="token">Token de autenticación del usuario</param>
        /// <param name="evidenceID">Identificador de la evidencia.</param>
        /// <returns>Información de la evidencia.</returns>
        public async Task<EvidenceDetail> GetEvidenceByIDAsync(string token, int evidenceID)
        {
            EvidenceDetail Result = null;

            // URI de la evidencia.
            string URI = $"{WebAPIBaseAddress}api/evidence/getevidencebyid?token={token}&&evidenceid={evidenceID}";

            using (var Client = new HttpClient())
            {
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    // Realizamos una petición GET
                    var Response =
                            await Client.GetAsync(URI);

                    var ResultWebAPI = await Response.Content.ReadAsStringAsync();
                    if (Response.StatusCode == HttpStatusCode.OK)
                    {
                        // Si el estatus de la respuesta HTTP fue exitosa, leemos
                        // el valor devuelto. 
                        Result = JsonConvert.DeserializeObject<EvidenceDetail>(ResultWebAPI);
                    }
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
