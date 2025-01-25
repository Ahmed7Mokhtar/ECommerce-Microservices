using OrdersService.BusinessLogicLayer.DTOs;
using OrdersService.BusinessLogicLayer.JsonConverters;
using System.Net.Http.Json;
using System.Text.Json;

namespace OrdersService.BusinessLogicLayer.HttpClients
{
    public class ProductsMicroserviceClient
    {
        private readonly HttpClient _httpClient;
        public ProductsMicroserviceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ProductDTO> GetById(Guid productId)
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
    }
}
