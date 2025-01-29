using Microsoft.Extensions.Logging;
using OrdersService.BusinessLogicLayer.DTOs;
using Polly.CircuitBreaker;
using Polly.Timeout;
using System.Net.Http.Json;

namespace OrdersService.BusinessLogicLayer.HttpClients
{
    public class UsersMicroserviceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UsersMicroserviceClient> _logger;

        public UsersMicroserviceClient(HttpClient httpClient, ILogger<UsersMicroserviceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<UserDTO> GetById(Guid userId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/Users/{userId}");
                if (!response.IsSuccessStatusCode)
                {
                    return response.StatusCode switch
                    {
                        System.Net.HttpStatusCode.NotFound => throw new ArgumentNullException("Invalid user id"),
                        System.Net.HttpStatusCode.BadRequest => throw new HttpRequestException("Bad request", null, System.Net.HttpStatusCode.BadRequest),
                        //_ => throw new HttpRequestException("Internal server error", null, response.StatusCode)
                        _ => new UserDTO(Name: "Temporarly unavailable", Email: "Temporarly unavailable", Gender: DTOs.Enums.Gender.Male, Id: Guid.Empty)
                    };
                }

                UserDTO? user = await response.Content.ReadFromJsonAsync<UserDTO>();
                ArgumentNullException.ThrowIfNull(user);

                return user;
            }
            catch (BrokenCircuitException ex)
            {
                _logger.LogError(ex, $"Circuit breaker is opened. Exception: {ex.Message}");
                return new UserDTO(Name: "Temporarly unavailable (Circuit Breacker)", Email: "Temporarly unavailable (Circuit Breacker)", Gender: DTOs.Enums.Gender.Male, Id: Guid.Empty);
            }
            catch(TimeoutRejectedException ex)
            {
                _logger.LogError(ex, "Timeout occured while fetching user data");
                return new UserDTO(Name: "Temporarly unavailable (Timeout)", Email: "Temporarly unavailable (Timeout)", Gender: DTOs.Enums.Gender.Male, Id: Guid.Empty);
            }
        }
    }
}
