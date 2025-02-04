using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using OrdersService.BusinessLogicLayer.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OrdersService.BusinessLogicLayer.Services
{
    public class CachingService : ICachingService
    {
        private readonly ILogger<CachingService> _logger;
        private readonly IDistributedCache _cache;

        public CachingService(ILogger<CachingService> logger, IDistributedCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        public async Task<T?> GetFromCacheAsync<T>(string cacheKey) where T : class
        {
            string? cacheData = await _cache.GetStringAsync(cacheKey);
            if(!string.IsNullOrWhiteSpace(cacheData))
            {
                _logger.LogInformation("Data is fetched from cache for key: {CacheKey}", cacheKey);
                return JsonSerializer.Deserialize<T>(cacheData);
            }

            return null;
        }

        public async Task SetCacheAsync<T>(string cacheKey, T data, DistributedCacheEntryOptions? options = null)
        {
            _logger.LogInformation("Data is added to cache for key: {CacheKey}", cacheKey);
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(data), options ?? GetDefaultCacheOptions());
        }

        private DistributedCacheEntryOptions GetDefaultCacheOptions() => 
            new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(300))
                .SetSlidingExpiration(TimeSpan.FromSeconds(100));
    }
}
