using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Transfers.API.Model;

namespace Transfers.API.Controllers
{
    [ApiController]
    [Route("api/v{v:apiVersion}")]
    public class TransfersControllerV2 : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly Config config;

        public TransfersControllerV2(IHttpClientFactory httpClientFactory, Config config)
        {
            _httpClient = httpClientFactory.CreateClient();
            config = config;
        }

        [HttpPost("local-transfers")]
        public async Task<IActionResult> GetCombinedData()
        {
            // Define the URLs of the other services

            try
            {
                // Make parallel requests to the services
                var taskA = _httpClient.GetStringAsync($"{config.AccountsApiUrl}/accounts");
                var taskB = _httpClient.GetStringAsync($"{config.LimitsApiUrl}/limits");

                await Task.WhenAll(taskA, taskB);

                // Parse the responses (adjust types to match your API's responses)
                var serviceAResponse = JsonSerializer.Deserialize<object>(taskA.Result);
                var serviceBResponse = JsonSerializer.Deserialize<object>(taskB.Result);

                // Combine the results
                var combinedResult = new
                {
                    Account = serviceAResponse,
                    Limit = serviceBResponse
                };

                // Return the combined result
                return Ok(combinedResult);
            }
            catch (HttpRequestException ex)
            {
                // Handle errors (e.g., service unavailable, timeout, etc.)
                return StatusCode(503, new { Error = "Failed to retrieve data from one or both services", Details = ex.Message });
            }
        }
    }
}
