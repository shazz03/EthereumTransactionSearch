using System.Collections.Generic;

namespace TransactionSearch.Core.Models
{
    public class TransactionResponseDto : ErrorResponseDto
    {
        public IList<TransactionData> Data { get; set; }
    }

    public class TransactionData
    {
        public string BlockHash { get; set; }
        public long BlockNumber { get; set; }
        public string From { get; set; }
        public long Gas { get; set; }
        public string Hash { get; set; }
        public string To { get; set; }
        public long Value { get; set; }
    }
}
