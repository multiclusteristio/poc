using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Quantic.Core;
using Transfers.API.Model;
using Microsoft.Extensions.Logging;
using System;

namespace Transfers.API.Query
{
    public class GetAccountByNumberHandler : IQueryHandler<GetAccountByNumber, Account>
    {
        private readonly HttpClient httpClient;
        private readonly Config config;

        private readonly ILogger<GetAccountByNumberHandler> logger;

        public GetAccountByNumberHandler(HttpClient httpClient,
            Config config,
            ILogger<GetAccountByNumberHandler> logger)
        {
            this.httpClient = httpClient;
            this.config = config;
            this.logger = logger;
        }

        public async Task<QueryResult<Account>> Handle(GetAccountByNumber query, RequestContext context)
        {
            logger.LogInformation("Handling GetAccountByNumber query with AccountNumber: {AccountNumber} and TraceId: {TraceId}", query.AccountNumber, context.TraceId);

            var requestUrl = $"{config.AccountsApiUrl}/accounts?accountNumber={query.AccountNumber}";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("quantic-trace-id", context.TraceId);

            logger.LogDebug("Sending HTTP request to URL: {Url} with TraceId: {TraceId}", requestUrl, context.TraceId);

            HttpResponseMessage response;
            try
            {
                response = await httpClient.SendAsync(request);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while sending HTTP request to {Url}", requestUrl);
                return QueryResult<Account>.WithError(Msg.GetAccountError, "Failed to fetch account information");
            }

            logger.LogDebug("Received HTTP response with StatusCode: {StatusCode} for TraceId: {TraceId}", response.StatusCode, context.TraceId);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Request to {Url} failed with StatusCode: {StatusCode} for TraceId: {TraceId}", requestUrl, response.StatusCode, context.TraceId);
                return QueryResult<Account>.WithError(Msg.GetAccountError, $"status code is {response.StatusCode}");
            }

            try
            {                
                var getAccount = await JsonSerializer.DeserializeAsync<QueryResponse<Account>>(await response.Content.ReadAsStreamAsync(), JsonCfg.Options);
                logger.LogInformation("Successfully deserialized account information for AccountNumber: {AccountNumber}", getAccount);
                return QueryResult<Account>.WithResult(getAccount.Data);
            }
            catch (JsonException jsonEx)
            {
                logger.LogError(jsonEx, "Error deserializing response content for AccountNumber: {AccountNumber}", query.AccountNumber);
                return QueryResult<Account>.WithError(Msg.GetAccountError, "Failed to parse account information");
            }
        }
    }

    public class GetAccountByNumber : IQuery<Account>
    {
        public GetAccountByNumber(string accountNumber)
        {
            AccountNumber = accountNumber;
        }

        public string AccountNumber { get; }
    }
}
