using Microsoft.Extensions.Logging;
using OrdersService.BusinessLogicLayer.DTOs;
using Polly;
using Polly.Bulkhead;
using Polly.Fallback;
using Polly.Wrap;
using System.Text;
using System.Text.Json;

namespace OrdersService.BusinessLogicLayer.Policies
{
    public class ProductsMicroservicePolicies : IProductsMicroservicePolicies
    {
        private readonly ILogger<ProductsMicroservicePolicies> _logger;
        private readonly IPolyPolicies _polyPolicies;

        public ProductsMicroservicePolicies(ILogger<ProductsMicroservicePolicies> logger, IPolyPolicies polyPolicies)
        {
            _logger = logger;
            _polyPolicies = polyPolicies;
        }

        public IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy()
        {
            var fallbackPolicy = _polyPolicies.GetFallbackPolicy(GetProductsFallbackDelegate());
            var bulkheadIsolationPolicy = _polyPolicies.GetBulkheadIsolationPolicy(2, 40);

            AsyncPolicyWrap<HttpResponseMessage> policy = Policy.WrapAsync(fallbackPolicy, bulkheadIsolationPolicy);

            return policy;
        }

        private Func<CancellationToken, Task<HttpResponseMessage>> GetProductsFallbackDelegate()
        {
            return async (context) =>
            {
                _logger.LogInformation("Fallback policy is being executed");

                ProductDTO productDTO = new ProductDTO(ProductId: Guid.Empty, ProductName: "Temporarly unavailable", Category: "Temporarly unavailable", 0, 0);

                return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonSerializer.Serialize(productDTO), Encoding.UTF8, "application/json"),
                };
            };
        }
    }
}
