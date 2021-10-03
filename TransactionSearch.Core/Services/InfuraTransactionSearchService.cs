using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TransactionSearch.Core.Configuration;
using TransactionSearch.Core.Extensions;
using TransactionSearch.Core.Infrastructure;
using TransactionSearch.Core.Models;

namespace TransactionSearch.Core.Services
{
    public class InfuraTransactionSearchService : ITransactionSearchService
    {
        private readonly IPolicyHolder _policyHolder;
        private readonly IHttpClientFactory _clientFactory;
        private readonly InfuraApiConfig _apiConfig;
        private readonly EthereumHexParser _parser;

        public InfuraTransactionSearchService(IPolicyHolder policyHolder, IHttpClientFactory clientFactory, IOptions<InfuraApiConfig> apiConfig, EthereumHexParser parser)
        {
            _policyHolder = policyHolder;
            _clientFactory = clientFactory;
            _parser = parser;
            _apiConfig = apiConfig?.Value;
        }

        public async Task<TransactionResponseDto> Search(SearchRequest request)
        {
            if (!Validate(request))
            {
                return new TransactionResponseDto
                {
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            var httpClient = _clientFactory.CreateClient("InfuraApi");
            var model = new InfuraRequestModel
            {
                JsonRpc = _apiConfig.JsonRpc,
                Method = InfuraEthereumMethodOptions.GetBlockByNumber,
                Id = request.BlockNumber,
                Params = new List<object> { "latest", true }
            };
            var json = new StringContent(JsonSerializer.Serialize(model));
            var responseMessage =
               await _policyHolder.GetRetryPolicy(_apiConfig.RetryAttempt, _apiConfig.WaitBeforeRetryAttemptSecond)
                   .ExecuteAsync(async () => await httpClient.PostAsync($"{_apiConfig.BaseUrl}{_apiConfig.ProjectId}", json));

            responseMessage.EnsureSuccessStatusCode();
            var content = await responseMessage.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<InfuraTransactionResponse>(content);

            var txnData = data?.Result?.Transactions
                .Select(d => new TransactionData
                {
                    BlockNumber = _parser.ParseBlockNumber(d.BlockNumber),
                    BlockHash = d.BlockHash,
                    From = d.From,
                    To = d.To,
                    Gas = _parser.ParseGasValue(d.Gas),
                    Hash = d.Hash,
                    Value = _parser.ParseAmountValue(d.Value)
                }).ToList();

            if (!string.IsNullOrWhiteSpace(request.Address))
            {
                txnData = txnData?.Where(j => j.From.Equals(request.Address)).ToList();
            }

            return new TransactionResponseDto
            {
                Data = txnData,
                StatusCode = HttpStatusCode.OK
            };
        }
        
        private static bool Validate(SearchRequest request)
        {
            if (request == null)
                return false;

            return !string.IsNullOrWhiteSpace(request.BlockNumber);
        }
    }
}
