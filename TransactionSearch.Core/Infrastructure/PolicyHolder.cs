using System;
using System.Net.Http;
using Polly;
using Polly.Extensions.Http;

namespace TransactionSearch.Core.Infrastructure
{
    public interface IPolicyHolder
    {
        IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryAttempt,
            int waitBeforeRetryAttemptSecond);
    }
    public class PolicyHolder : IPolicyHolder
    {
        public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryAttempt,
            int waitBeforeRetryAttemptSecond) =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(retryAttempt,
                    ra => TimeSpan.FromSeconds(waitBeforeRetryAttemptSecond));
    }
}
