using Azure.Messaging.ServiceBus;
using CacheClient.Clients;
using CacheClient.Services;
using CacheClient.Wrappers;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using PartnerApi.Clients;
using PartnerApi.Services;
using PartnerApiClient.RateLimiters;
using PartnerApiClient.Utils;
using System;

[assembly: FunctionsStartup(typeof(DataHydrator.Startup))]
namespace DataHydrator
{
    /// <summary>
    /// Configures services for the Azure Functions application.
    /// </summary>
    public class Startup : FunctionsStartup
    {
        /// <summary>
        /// Configures services for the Azure Functions application.
        /// </summary>
        /// <param name="builder">The builder used to configure services.</param>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient("HttpClient").AddPolicyHandler(PartnerApiRetryPolicy.GetRetryPolicy());
            builder.Services.AddSingleton<IPartnerApiService, PartnerApiService>();
            builder.Services.AddSingleton<IPartnerApiClient, PartnerApi.Clients.PartnerApiClient>();
            builder.Services.AddSingleton<IConnectionMultiplexerWrapper>(_ => new ConnectionMultiplexerWrapper(Environment.GetEnvironmentVariable("RedisConnectionString")));
            builder.Services.AddSingleton<IRedisConnectionFactory, RedisConnectionFactory>(); builder.Services.AddSingleton(_ => new ServiceBusClient(Environment.GetEnvironmentVariable("ServiceBusConnectionString")));
            builder.Services.AddSingleton<IJsonCacheService, RedisJsonCacheService>();
            builder.Services.AddSingleton<ICacheService, RedisCacheService>();
            builder.Services.AddSingleton<IJsonCommandsAsyncWrapper, JsonCommandsAsyncWrapper>();
            builder.Services.AddSingleton<IRateLimiter, RateLimiter>();
        }
    }
}