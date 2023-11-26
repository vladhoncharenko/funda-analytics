﻿using Azure.Messaging.ServiceBus;
using CacheClient.Clients;
using CacheClient.Services;
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
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient("HttpClient").AddPolicyHandler(PartnerApiRetryPolicy.GetRetryPolicy());
            builder.Services.AddSingleton<IPartnerApiService, PartnerApiService>();
            builder.Services.AddSingleton<IPartnerApiClient, PartnerApi.Clients.PartnerApiClient>();
            builder.Services.AddSingleton<IRedisConnectionFactory>(_ => new RedisConnectionFactory(Environment.GetEnvironmentVariable("RedisConnectionString")));
            builder.Services.AddSingleton(_ => new ServiceBusClient(Environment.GetEnvironmentVariable("ServiceBusConnectionString")));
            builder.Services.AddSingleton<IJsonCacheService, RedisJsonCacheService>();
            builder.Services.AddSingleton<ICacheService, RedisCacheService>();
            builder.Services.AddSingleton<IRateLimiter, RateLimiter>();
        }
    }
}