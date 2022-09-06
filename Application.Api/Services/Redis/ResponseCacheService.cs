using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Application.Api.Services.Redis;

public class ResponseCacheService : IResponseCacheService
{
    private readonly IDistributedCache _distributedCache;

    public ResponseCacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }
    
    public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan ttl)
    {
        if (response is null) return;
        
        var serializeResponse = JsonConvert.SerializeObject(response);

        await _distributedCache.SetStringAsync(cacheKey, serializeResponse, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = ttl
        });
    }

    public async Task<string?> GetCacheResponseAsync(string cacheKey)
    {
        var response = await _distributedCache.GetStringAsync(cacheKey);
        return string.IsNullOrEmpty(response) ? null : response;
    }
}