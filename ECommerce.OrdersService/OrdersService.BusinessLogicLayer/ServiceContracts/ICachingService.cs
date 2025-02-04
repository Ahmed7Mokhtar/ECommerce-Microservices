using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.BusinessLogicLayer.ServiceContracts
{
    public interface ICachingService
    {
        Task<T?> GetFromCacheAsync<T>(string cacheKey) where T : class;
        Task SetCacheAsync<T>(string cacheKey, T data, DistributedCacheEntryOptions? options = null);
    }
}
