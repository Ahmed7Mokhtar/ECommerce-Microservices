using Polly;

namespace OrdersService.BusinessLogicLayer.Policies
{
    public interface IPolyPolicies
    {
        IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount);
        IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(int handledEventsAllowedBeforeBreaking, TimeSpan durationOfBreak);
        public IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy(TimeSpan timeoutDuration);
        public IAsyncPolicy<HttpResponseMessage> GetFallbackPolicy(Func<CancellationToken, Task<HttpResponseMessage>> fallbackDelegate);
        IAsyncPolicy<HttpResponseMessage> GetBulkheadIsolationPolicy(int maxParallelization, int maxQueuingActions);
    }
}
