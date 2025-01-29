using Microsoft.Extensions.Logging;
using Polly;
using Polly.Bulkhead;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Retry;
using Polly.Timeout;

namespace OrdersService.BusinessLogicLayer.Policies
{
    public class PolyPolicies : IPolyPolicies
    {
        private readonly ILogger<UserserMicroservicePolicies> _logger;

        public PolyPolicies(ILogger<UserserMicroservicePolicies> logger)
        {
            _logger = logger;
        }

        public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount)
        {
            AsyncRetryPolicy<HttpResponseMessage> policy = Policy.HandleResult<HttpResponseMessage>(response => !response.IsSuccessStatusCode)
                    .WaitAndRetryAsync(
                        retryCount: retryCount,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromMilliseconds(Math.Pow(2, retryAttempt)), // Delay between retries (Exponential Backoff)
                        onRetry: (outcome, timespan, retryAttempt, context) =>
                        {
                            _logger.LogInformation($"Retry {retryAttempt} after {timespan.TotalSeconds} seconds");
                        }
                    );

            return policy;
        }

        public IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(int handledEventsAllowedBeforeBreaking, TimeSpan durationOfBreak)
        {
            AsyncCircuitBreakerPolicy<HttpResponseMessage> policy = Policy.HandleResult<HttpResponseMessage>(response => !response.IsSuccessStatusCode)
                    .CircuitBreakerAsync(
                        handledEventsAllowedBeforeBreaking: handledEventsAllowedBeforeBreaking,  // after how many continuas requests the circuit breaker should be in opened state
                        durationOfBreak: durationOfBreak, // how much time from open to half open state 
                        onBreak: (outcome, timespan) =>
                        {
                            _logger.LogInformation($"Circuit breaker is now opened for {timespan.TotalMinutes} due to conscutive 4 failures. subsequest requests will be blocked");
                        },
                        onReset: () =>
                        {
                            _logger.LogInformation($"Circuit breaker is now closed. Requests are allowed to pass through");
                        }
                    );

            return policy;
        }

        public IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy(TimeSpan timeoutDuration)
        {
            AsyncTimeoutPolicy<HttpResponseMessage> policy = Policy.TimeoutAsync<HttpResponseMessage>(timeoutDuration);

            return policy;
        }

        public IAsyncPolicy<HttpResponseMessage> GetFallbackPolicy(Func<CancellationToken, Task<HttpResponseMessage>> fallbackDelegate)
        {
            AsyncFallbackPolicy<HttpResponseMessage> policy = Policy.HandleResult<HttpResponseMessage>(response => !response.IsSuccessStatusCode)
                .FallbackAsync(fallbackDelegate);

            return policy;
        }

        public IAsyncPolicy<HttpResponseMessage> GetBulkheadIsolationPolicy(int maxParallelization, int maxQueuingActions)
        {
            AsyncBulkheadPolicy<HttpResponseMessage> policy = Policy.BulkheadAsync<HttpResponseMessage>(
                    maxParallelization: maxParallelization,  // Allows up to 2 concurrent requests
                    maxQueuingActions: maxQueuingActions,   // Allows up to 2 queued requests
                    onBulkheadRejectedAsync: (context) =>
                    {
                        _logger.LogWarning("Bulkhead isolation policy is being executed");
                        throw new BulkheadRejectedException("Bulkhead queue is full!");
                    }
                );

            return policy;
        }
    }
}
