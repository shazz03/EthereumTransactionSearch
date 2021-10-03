using System.Net;
using System.Text.Json.Serialization;

namespace TransactionSearch.Core.Models
{
    public class ErrorResponseDto
    {
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }
    }
}
