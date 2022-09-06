using Application.Api.Cache;
using Application.Api.Services.Redis;

namespace Application.Api.Installers;

public class CacheInstaller : IInstaller
{
    public void InstallServices(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var redisCacheSettings = new RedisCacheSettings();
        configuration.GetSection(nameof(RedisCacheSettings)).Bind(redisCacheSettings);
        serviceCollection.AddSingleton(redisCacheSettings);
        
        if(!redisCacheSettings.Enabled) return;

        serviceCollection.AddStackExchangeRedisCache(o => o.Configuration = redisCacheSettings.ConnectionString);
        serviceCollection.AddSingleton<IResponseCacheService, ResponseCacheService>();
    }
}