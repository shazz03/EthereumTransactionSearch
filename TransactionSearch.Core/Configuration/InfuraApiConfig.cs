namespace TransactionSearch.Core.Configuration
{
    public class InfuraApiConfig
    {
        public string BaseUrl { get; set; }
        public int RetryAttempt { get; set; }
        public int WaitBeforeRetryAttemptSecond { get; set; }
        public string ProjectId { get; set; }
        public string JsonRpc { get; set; }
    }
}
