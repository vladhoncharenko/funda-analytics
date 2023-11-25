using CacheClient.Clients;
using CacheClient.Services;
using DataApi.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddSingleton<IRedisConnectionFactory>(_ => new RedisConnectionFactory(Environment.GetEnvironmentVariable("RedisConnectionString")));
        services.AddSingleton<ICacheService, RedisCacheService>();
        services.AddSingleton<IRealEstateBrokersService, RealEstateBrokersService>();
    })
    .Build();

host.Run();
