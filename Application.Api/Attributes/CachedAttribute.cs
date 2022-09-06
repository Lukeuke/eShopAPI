using System.Text;
using Application.Api.Cache;
using Application.Api.Services.Redis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Application.Api.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CachedAttribute : Attribute, IAsyncActionFilter
{
    private readonly int _timeToLiveSeconds;

    public CachedAttribute(int timeToLiveSeconds)
    {
        _timeToLiveSeconds = timeToLiveSeconds;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) 
    {
        // Check if request is cached
        // if it is => return

        var cacheSettings = context.HttpContext.RequestServices.GetRequiredService<RedisCacheSettings>();

        if (!cacheSettings.Enabled)
        {
            await next();
            return;
        }

        var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

        var cacheKey = GenerateCachedKeyFromRequest(context.HttpContext.Request);
        var cacheResponse = await cacheService.GetCacheResponseAsync(cacheKey);

        if (!string.IsNullOrEmpty(cacheResponse))
        {
            var contentResult = new ContentResult
            {
                Content = cacheResponse,
                ContentType = "application/json",
                StatusCode = 200
            };
            context.Result = contentResult;
            
            return;
        }
        
        // Get the value
        // Cache the response
        
        var executedContext = await next();

        if (executedContext.Result is OkObjectResult okObjectResult)
        {
            await cacheService.CacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveSeconds));
        }
    }

    private static string GenerateCachedKeyFromRequest(HttpRequest request)
    {
        var sb = new StringBuilder();

        sb.Append($"{request.Path}");

        foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
        {
            sb.Append($"|{key}-{value}");
        }

        return sb.ToString();
    }
}