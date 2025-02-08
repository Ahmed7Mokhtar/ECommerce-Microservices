using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using OrdersService.BusinessLogicLayer.DTOs;
using OrdersService.BusinessLogicLayer.JsonConverters;
using OrdersService.BusinessLogicLayer.ServiceContracts;
using Polly.Bulkhead;
using System.Net.Http.Json;
using System.Text.Json;

namespace OrdersService.BusinessLogicLayer.HttpClients
{
    public class ProductsMicroserviceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductsMicroserviceClient> _logger;
        private readonly ICachingService _cacheService;

        public ProductsMicroserviceClient(HttpClient httpClient, ILogger<ProductsMicroserviceClient> logger, ICachingService cacheService)
        {
            _httpClient = httpClient;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<ProductDTO> GetById(Guid productId)
        {
            try
            {
                var cachedProduct = await _cacheService.GetFromCacheAsync<ProductDTO>($"product-{productId}");
                if (cachedProduct is not null)
                    return cachedProduct;

                HttpResponseMessage response = await _httpClient.GetAsync($"gateway/products/search/product-id/{productId}");
                if (!response.IsSuccessStatusCode)
                {
                    // Comming from Fallback policy
                    if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                    {
                        ProductDTO? fallbackProduct = await response.Content.ReadFromJsonAsync<ProductDTO>();
                        return fallbackProduct is null
                            ? throw new NotImplementedException("Product fallback response is not implemented yet")
                            : fallbackProduct;
                    }

                    return response.StatusCode switch
                    {
                        System.Net.HttpStatusCode.NotFound => throw new ArgumentNullException($"Invalid product with id {productId}"),
                        System.Net.HttpStatusCode.BadRequest => throw new HttpRequestException("Bad request", null, System.Net.HttpStatusCode.BadRequest),
                        _ => throw new HttpRequestException("Internal server error", null, response.StatusCode)
                    };
                }

                ProductDTO? product = await response.Content.ReadFromJsonAsync<ProductDTO>();
                ArgumentNullException.ThrowIfNull(product);

                // Add to cache
                await _cacheService.SetCacheAsync($"product-{productId}", product, new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(300))
                    .SetSlidingExpiration(TimeSpan.FromSeconds(100))
                );

                return product;
            }
            catch (BulkheadRejectedException ex)    // TODO: Try testing it using Load Testing
            {
                _logger.LogError(ex, "Bulkhead isolation policy is being executed, Blocks request since request queue is full");
                return new ProductDTO(ProductId: Guid.Empty, ProductName: "Temporarily unavailable (Bulkhead)", Category: "Temporarily unavailable (Bulkhead)", UnitPrice: 0, QuantityInStock: 0);
            }
        }
    }
}
