using Microsoft.Extensions.Logging;
using OrdersService.BusinessLogicLayer.DTOs;
using OrdersService.BusinessLogicLayer.JsonConverters;
using Polly.Bulkhead;
using System.Net.Http.Json;
using System.Text.Json;

namespace OrdersService.BusinessLogicLayer.HttpClients
{
    public class ProductsMicroserviceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductsMicroserviceClient> _logger;

        public ProductsMicroserviceClient(HttpClient httpClient, ILogger<ProductsMicroserviceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ProductDTO> GetById(Guid productId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/products/search/product-id/{productId}");
                if (!response.IsSuccessStatusCode)
                {
                    return response.StatusCode switch
                    {
                        System.Net.HttpStatusCode.NotFound => throw new ArgumentNullException($"Invalid product with id {productId}"),
                        System.Net.HttpStatusCode.BadRequest => throw new HttpRequestException("Bad request", null, System.Net.HttpStatusCode.BadRequest),
                        _ => throw new HttpRequestException("Internal server error", null, response.StatusCode)
                    };
                }

                //var options = new JsonSerializerOptions
                //{
                //    Converters = { new CategoryConverter() }
                //};

                ProductDTO? product = await response.Content.ReadFromJsonAsync<ProductDTO>();
                ArgumentNullException.ThrowIfNull(product);

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
