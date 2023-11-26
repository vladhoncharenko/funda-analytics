using CacheClient.Clients;
using CacheClient.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PartnerApi.Clients;
using PartnerApi.Services;
using PartnerApiClient.RateLimiters;
using PartnerApiClient.Utils;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddHttpClient("HttpClient").AddPolicyHandler(PartnerApiRetryPolicy.GetRetryPolicy());
        services.AddSingleton<IPartnerApiService, PartnerApiService>();
        services.AddSingleton<IPartnerApiClient, PartnerApi.Clients.PartnerApiClient>();
        services.AddSingleton<IRedisConnectionFactory>(_ => new RedisConnectionFactory(Environment.GetEnvironmentVariable("RedisConnectionString")));
        services.AddSingleton<IJsonCacheService, RedisJsonCacheService>();
        services.AddSingleton<ICacheService, RedisCacheService>();
        services.AddSingleton<IRateLimiter, RateLimiter>();
    })
    .Build();

host.Run();
