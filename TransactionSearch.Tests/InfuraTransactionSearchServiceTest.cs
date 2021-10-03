using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Polly;
using TransactionSearch.Core.Configuration;
using TransactionSearch.Core.Extensions;
using TransactionSearch.Core.Infrastructure;
using TransactionSearch.Core.Models;
using TransactionSearch.Core.Services;

namespace TransactionSearch.Api.Tests
{
    [TestClass]
    public class InfuraTransactionSearchServiceTest
    {
        private Mock<IPolicyHolder> _policyHolderMock;
        private Mock<IHttpClientFactory> _clientFactoryMock;
        private Mock<EthereumHexParser> _mockParser;
        private IOptions<InfuraApiConfig> _option;

        public InfuraTransactionSearchServiceTest()
        {
            _option = Options.Create(new InfuraApiConfig
            {
                BaseUrl = "http://test.com",
                ProjectId = "test123",
                JsonRpc = "2.0",
                RetryAttempt = 3,
                WaitBeforeRetryAttemptSecond = 10
            });

            _policyHolderMock = new Mock<IPolicyHolder>();
            _clientFactoryMock = new Mock<IHttpClientFactory>();
            _mockParser = new Mock<EthereumHexParser>();


        }

        [TestMethod]
        public async Task Search_Validate_Request_Null_Returns_BadRequest()
        {
            var transactionSearch = new InfuraTransactionSearchService(_policyHolderMock.Object, _clientFactoryMock.Object, _option, _mockParser.Object);
            var result = await transactionSearch.Search(null);
            Assert.IsNull(result.Data);
            Assert.Equals(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task Search_Validate_Request_BlockNumber_Returns_BadRequest()
        {
            var transactionSearch = new InfuraTransactionSearchService(_policyHolderMock.Object, _clientFactoryMock.Object, _option, _mockParser.Object);

            var request = new SearchRequest { BlockNumber = "" };
            var result = await transactionSearch.Search(request);
            Assert.IsNull(result.Data);
            Assert.Equals(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task Search_ParseHex_Returns_Valid_Value()
        {
            var request = new SearchRequest { BlockNumber = "1234" };

            var data = new TransactionResponseDto
            {
                Data = new List<TransactionData>
               {
                   new TransactionData
                   {
                       Value = 119999,
                       BlockNumber = 11111
                   }
               }
            };

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("PostAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(data))
                });

            var httpClient = new HttpClient(mockMessageHandler.Object);
            _clientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var transactionSearch = new InfuraTransactionSearchService(_policyHolderMock.Object, _clientFactoryMock.Object, _option, _mockParser.Object);

            var result = await transactionSearch.Search(request);

            //_policyHolderMock.Setup(p=>p.GetRetryPolicy(1,2).ExecuteAsync())
            Assert.IsNull(result.Data);
            Assert.Equals(HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}
