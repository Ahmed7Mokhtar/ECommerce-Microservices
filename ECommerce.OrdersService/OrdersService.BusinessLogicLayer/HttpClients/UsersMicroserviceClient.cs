using OrdersService.BusinessLogicLayer.DTOs;
using System.Net.Http.Json;

namespace OrdersService.BusinessLogicLayer.HttpClients
{
    public class UsersMicroserviceClient
    {
        private readonly HttpClient _httpClient;

        public UsersMicroserviceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserDTO> GetById(Guid userId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/Users/{userId}");
            if (!response.IsSuccessStatusCode)
            {
                return response.StatusCode switch
                {
                    System.Net.HttpStatusCode.NotFound => throw new ArgumentNullException("Invalid user id"),
                    System.Net.HttpStatusCode.BadRequest => throw new HttpRequestException("Bad request", null, System.Net.HttpStatusCode.BadRequest),
                    _ => throw new HttpRequestException("Internal server error", null, response.StatusCode)
                };
            }

            UserDTO? user = await response.Content.ReadFromJsonAsync<UserDTO>();
            ArgumentNullException.ThrowIfNull(user);

            return user;
        }
    }
}
