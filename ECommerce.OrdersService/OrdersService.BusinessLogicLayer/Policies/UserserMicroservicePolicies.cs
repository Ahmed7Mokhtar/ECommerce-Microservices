using Polly;
using Polly.Wrap;

namespace OrdersService.BusinessLogicLayer.Policies
{
    public class UserserMicroservicePolicies : IUserserMicroservicePolicies
    {
        private readonly IPolyPolicies _polyPolicies;

        public UserserMicroservicePolicies(IPolyPolicies polyPolicies)
        {
            _polyPolicies = polyPolicies;
        }

        public IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy()
        {
            var retryPolicy = _polyPolicies.GetRetryPolicy(5);
            var circuitBreakerPolicy = _polyPolicies.GetCircuitBreakerPolicy(3, TimeSpan.FromMinutes(2));
            var timeoutPolicy = _polyPolicies.GetTimeoutPolicy(TimeSpan.FromSeconds(5));

            AsyncPolicyWrap<HttpResponseMessage> wrapPolicy = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy, timeoutPolicy); // will be excuted in the order they are passed

            return wrapPolicy;
        }
    }
}
