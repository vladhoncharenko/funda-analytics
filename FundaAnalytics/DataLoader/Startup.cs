using CacheClient.Clients;
using CacheClient.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using PartnerApi.Clients;
using PartnerApi.Services;
using PartnerApiClient.RateLimiters;
using PartnerApiClient.Utils;
using System;

[assembly: FunctionsStartup(typeof(DataLoader.Startup))]
namespace DataLoader
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
            builder.Services.AddSingleton<IRedisConnectionFactory>(_ => new RedisConnectionFactory(Environment.GetEnvironmentVariable("RedisConnectionString")));
            builder.Services.AddSingleton<IJsonCacheService, RedisJsonCacheService>();
            builder.Services.AddSingleton<ICacheService, RedisCacheService>();
            builder.Services.AddSingleton<IRateLimiter, RateLimiter>();
        }
    }
}