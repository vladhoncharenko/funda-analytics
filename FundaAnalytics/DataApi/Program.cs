using CacheClient.Clients;
using CacheClient.Services;
using DataApi.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Configure and build instances of the IHost interface.
var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddSingleton<IRedisConnectionFactory>(_ => new RedisConnectionFactory(Environment.GetEnvironmentVariable("RedisConnectionString")));
        services.AddSingleton<IJsonCacheService, RedisJsonCacheService>();
        services.AddSingleton<IRealEstateBrokersService, RealEstateBrokersService>();
    })
    .Build();

host.Run();
