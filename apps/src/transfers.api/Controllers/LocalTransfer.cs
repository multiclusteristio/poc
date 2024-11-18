using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System;

namespace Transfers.API.Controllers
{
    [ApiController]
    [Route("api/v{v:apiVersion}")]
    public class TransfersControllerV2 : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public TransfersControllerV2(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("local-transfer")]
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
                    Accounts = accountsResponse
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
        private class LimitsResponse
        {
            public int DailyLimit { get; set; }
            public int MonthlyLimit { get; set; }
            public int WeeklyLimit { get; set; }
            public string Region { get; set; }
        }

        private class AccountsResponse
        {
            public string AccountNumber { get; set; }
            public string AccountType { get; set; }
            public string Cif { get; set; }
            public double Balance { get; set; }
            public string BranchCode { get; set; }
            public string Region { get; set; }
        }
    }
}
