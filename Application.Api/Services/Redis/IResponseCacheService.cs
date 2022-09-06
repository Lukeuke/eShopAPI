namespace Application.Api.Services.Redis;

public interface IResponseCacheService
{
    Task CacheResponseAsync(string cacheKey, object response, TimeSpan ttl);
    Task<string?> GetCacheResponseAsync(string cacheKey);
}