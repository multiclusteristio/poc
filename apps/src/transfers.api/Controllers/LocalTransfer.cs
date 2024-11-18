using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using System.Text.Json.Serialization;
using Transfers.API.Model;

namespace Transfers.API.Controllers
{
    [ApiController]
    [Route("api/v{v:apiVersion}")]
    public class TransfersControllerV2 : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly Config _config;

        public TransfersControllerV2(IHttpClientFactory httpClientFactory, Config config)
        {
            _httpClient = httpClientFactory.CreateClient();
            _config = config;
        }

        [HttpPost("local-transfer")]
        public async Task<IActionResult> GetCombinedData()
        {
            // Define service endpoints
            var limitsUrl = "http://limits-ms-base.limits/api/v1/limits";
            var accountsUrl = "http://accounts-ms-base.accounts/api/v1/accounts";

            try
            {
                // Make requests to both endpoints
                var limitsTask = _httpClient.GetStringAsync(limitsUrl);
                var accountsTask = _httpClient.GetStringAsync(accountsUrl);

                await Task.WhenAll(limitsTask, accountsTask);

                // Deserialize JSON responses
                var limitsResponse = JsonSerializer.Deserialize<LimitsResponse>(limitsTask.Result);
                var accountsResponse = JsonSerializer.Deserialize<AccountsResponse>(accountsTask.Result);

                // Combine results into a single object
                var combinedResult = new
                {
                    Limits = limitsResponse,
                    Accounts = accountsResponse,
                    TransferRegion = _config.Region
                };

                // Return combined result
                return Ok(combinedResult);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(503, new { Error = "Failed to retrieve data from one or both services", Details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An unexpected error occurred", Details = ex.Message });
            }
        }

        // Classes to map the JSON responses
        public class LimitsResponse
        {
            [JsonPropertyName("dailyLimit")]
            public int DailyLimit { get; set; }

            [JsonPropertyName("monthlyLimit")]
            public int MonthlyLimit { get; set; }

            [JsonPropertyName("weeklyLimit")]
            public int WeeklyLimit { get; set; }

            [JsonPropertyName("region")]
            public string Region { get; set; }
        }

        public class AccountsResponse
        {
            [JsonPropertyName("accountNumber")]
            public string AccountNumber { get; set; }

            [JsonPropertyName("accountType")]
            public string AccountType { get; set; }

            [JsonPropertyName("cif")]
            public string Cif { get; set; }

            [JsonPropertyName("balance")]
            public decimal Balance { get; set; }

            [JsonPropertyName("branchCode")]
            public string BranchCode { get; set; }

            [JsonPropertyName("region")]
            public string Region { get; set; }
        }
    }
}
