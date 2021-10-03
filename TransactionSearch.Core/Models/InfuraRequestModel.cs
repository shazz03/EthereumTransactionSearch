using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TransactionSearch.Core.Models
{
    public class InfuraRequestModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("jsonrpc")]
        public string JsonRpc { get; set; }

        [JsonPropertyName("method")]
        public string Method { get; set; }

        [JsonPropertyName("params")]
        public List<object> Params { get; set; }
    }
}
