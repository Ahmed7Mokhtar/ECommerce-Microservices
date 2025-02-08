using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using OrdersService.BusinessLogicLayer.DTOs;
using OrdersService.BusinessLogicLayer.ServiceContracts;
using Polly.CircuitBreaker;
using Polly.Timeout;
using System.Net.Http.Json;
using System.Text.Json;

namespace OrdersService.BusinessLogicLayer.HttpClients
{
    public class UsersMicroserviceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UsersMicroserviceClient> _logger;
        private readonly ICachingService _cacheService;

        public UsersMicroserviceClient(HttpClient httpClient, ILogger<UsersMicroserviceClient> logger, ICachingService cacheService)
        {
            _httpClient = httpClient;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<UserDTO> GetById(Guid userId)
        {
            try
            {
                var cachedUser = await _cacheService.GetFromCacheAsync<UserDTO>($"user-{userId}");
                if (cachedUser is not null)
                    return cachedUser;

                HttpResponseMessage response = await _httpClient.GetAsync($"gateway/Users/{userId}");
                if (!response.IsSuccessStatusCode)
                {
                    // Comming from Fallback policy
                    if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                    {
                        UserDTO? fallbackUser = await response.Content.ReadFromJsonAsync<UserDTO>();
                        return fallbackUser is null
                            ? throw new NotImplementedException("User fallback response is not implemented yet")
                            : fallbackUser;
                    }

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

                // Add to cache
                await _cacheService.SetCacheAsync($"user-{userId}", user, new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTimeOffset.UtcNow.AddMinutes(5))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(3))
                );

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
