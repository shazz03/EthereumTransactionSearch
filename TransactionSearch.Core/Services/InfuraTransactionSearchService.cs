using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TransactionSearch.Core.Configuration;
using TransactionSearch.Core.Extensions;
using TransactionSearch.Core.Models;

namespace TransactionSearch.Core.Services
{
    public class InfuraTransactionSearchService : ITransactionSearchService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly InfuraApiConfig _apiConfig;
        private readonly EthereumHexParser _parser;

        public InfuraTransactionSearchService(IHttpClientFactory clientFactory, IOptions<InfuraApiConfig> apiConfig, EthereumHexParser parser)
        {
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

            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_apiConfig.BaseUrl}{_apiConfig.ProjectId}"),
                Content = new StringContent(JsonSerializer.Serialize(model)),
                Method = HttpMethod.Post
            };

            var responseMessage = await httpClient.SendAsync(httpRequest);

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
                    Gas = _parser.ParseGasValue(d.GasPrice),
                    Hash = d.Hash,
                    Value = _parser.ParseAmountValue(d.Value)
                }).ToList();

            txnData = FilterTransactions(request, txnData);

            return new TransactionResponseDto
            {
                Data = txnData,
                StatusCode = HttpStatusCode.OK
            };
        }

        private static List<TransactionData> FilterTransactions(SearchRequest request, List<TransactionData> txnData)
        {
            if (!string.IsNullOrWhiteSpace(request.Address))
            {
                txnData = txnData?.Where(j => j.From.Equals(request.Address)).ToList();
            }

            return txnData;
        }

        private static bool Validate(SearchRequest request)
        {
            if (request == null)
                return false;

            return !string.IsNullOrWhiteSpace(request.BlockNumber);
        }
    }
}
