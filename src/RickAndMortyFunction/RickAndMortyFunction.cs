using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RickAndMortyFunction
{
    public static class RickAndMortyFunction
    {
        private static readonly HttpClient httpClient = new HttpClient();

        [FunctionName("GetCharacterInfo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "character/{id}")] HttpRequest req,
            int id,
            ILogger log)
        {
            log.LogInformation("Processing request to get character information from Rick and Morty API.");

            try
            {
                string baseApiUrl = "https://rickandmortyapi.com/api";
                string apiUrl = $"{baseApiUrl}/character/{id}";

                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return new OkObjectResult(content);
                }
                else
                {
                    return new BadRequestObjectResult("Error fetching character info.");
                }
            }
            catch (Exception ex)
            {
                log.LogError($"An error occurred: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
