using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using TransactionSearch.Core.Configuration;
using TransactionSearch.Core.Extensions;
using TransactionSearch.Core.Models;
using TransactionSearch.Core.Services;

namespace TransactionSearch.Tests
{
    [TestClass]
    public class InfuraTransactionSearchServiceTest
    {
        private readonly Mock<IHttpClientFactory> _clientFactoryMock;
        private readonly Mock<EthereumHexParser> _mockParser;
        private readonly IOptions<InfuraApiConfig> _option;

        public InfuraTransactionSearchServiceTest()
        {
            _option = Options.Create(new InfuraApiConfig
            {
                BaseUrl = "http://test.com/",
                ProjectId = "test123",
                JsonRpc = "2.0"
            });

            _clientFactoryMock = new Mock<IHttpClientFactory>();
            _mockParser = new Mock<EthereumHexParser>();
        }

        [TestMethod]
        public async Task Search_Validate_Request_Null_Returns_BadRequest()
        {
            var transactionSearch = new InfuraTransactionSearchService(_clientFactoryMock.Object, _option, _mockParser.Object);
            var result = await transactionSearch.Search(null);
            Assert.IsNotNull(result);
            Assert.IsNull(result.Data);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task Search_Validate_Request_BlockNumber_Returns_BadRequest()
        {
            var transactionSearch = new InfuraTransactionSearchService(_clientFactoryMock.Object, _option, _mockParser.Object);

            var request = new SearchRequest { BlockNumber = "" };
            var result = await transactionSearch.Search(request);
            Assert.IsNotNull(result);
            Assert.IsNull(result.Data);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        public InfuraTransactionSearchService GetSearchService(InfuraTransactionResponse data)
        {
            var mockMessageHandler = new Mock<HttpMessageHandler>();

            mockMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(data))
                });

            var httpClient = new HttpClient(mockMessageHandler.Object);
            _clientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            return new InfuraTransactionSearchService(_clientFactoryMock.Object, _option, _mockParser.Object);
        }

        [TestMethod]
        public async Task Search_ByBlockNumber_Returns_Valid_Result()
        {
            var request = new SearchRequest { BlockNumber = "13334026" };

            var data = new InfuraTransactionResponse
            {
                Id = "13334026",
                Result = new Result
                {
                    Transactions = new List<Transaction>
                   {
                       new Transaction{
                               BlockHash = "0x5d87dc0ad30cbc8f3163ae1333333ebab916fe266159c1d3a8f3f98693c18154",
                               BlockNumber= "0xcb760a",
                               From= "0xd24400ae8bfebb18ca49be86258a3c749cf46853",
                               Gas="0x15f90",
                               GasPrice = "0xf9d7aec38",
                               Hash = "0xf926a3ba0d65e29bf2d7d7e39c98093609a3f0776fe973a2399b014ebb0e54ac",
                               To= "0x93a493f10f11b6c4574eae1fcb4d12241174090e",
                               Value= "0x2c68af0bb140000"
                           },
                       new Transaction{
                           BlockHash = "0x5d87dc0ad30cbc8f3163ae1333333ebab916fe266159c1d3a8f3f98693c18154",
                           BlockNumber= "0xcb760a",
                           From= "0xd24400ae8bfebb18ca49be86258a3c749cf46853",
                           Gas="0x55ca8",
                           GasPrice = "0xf9d7aec38",
                           Hash = "0x16fe441e7af55862cccf6916ba8088609f1a99b64326230c37ad0dfb71ca18f8",
                           To= "0x5f65f7b609678448494de4c87521cdf6cef1e932",
                           Value= "0x0"
                       }
                   }
                }
            };

            var transactionSearch = GetSearchService(data);
            var result = await transactionSearch.Search(request);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(2, result.Data.Count);
            Assert.AreEqual(0.2m, result.Data.First()?.Value);
            Assert.AreEqual(0, result.Data.Last()?.Value);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
        
        [TestMethod]
        public async Task Search_ByBlockNumber_And_Address_Returns_Valid_Result()
        {
            var request = new SearchRequest { BlockNumber = "13334026", Address = "0xd24400ae8bfebb18ca49be86258a3c749cf46853" };

            var data = new InfuraTransactionResponse
            {
                Result = new Result
                {
                    Transactions = new List<Transaction>
                   {
                       new Transaction{
                               BlockNumber= "0xcb760a",
                               From= "0xd24400ae8bfebb18ca49be86258a3c749cf46853",
                               To= "0x93a493f10f11b6c4574eae1fcb4d12241174090e",
                               Value= "0x2c68af0bb140000"
                           },
                       new Transaction{
                               BlockNumber= "0xcb760a",
                           From= "0xd37b0ea4257ff186d31f0860243c2048d7155e89",
                           To= "0x5f65f7b609678448494de4c87521cdf6cef1e932",
                           Value= "0x0"
                       },
                       new Transaction{
                               BlockNumber= "0xcb760a",
                           From= "0x2cd121b6af2685459e09aa94fa5d68e2156f4158",
                           To= "0x5f65f7b609678448494de4c87521cdf6cef1e932",
                           Value= "0x0"
                       }
                   }
                }
            };

            var transactionSearch = GetSearchService(data);
            var result = await transactionSearch.Search(request);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(1, result.Data.Count);
            Assert.AreEqual("0xd24400ae8bfebb18ca49be86258a3c749cf46853", result.Data.First()?.From);
            Assert.AreEqual(0.2m, result.Data.First()?.Value);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
