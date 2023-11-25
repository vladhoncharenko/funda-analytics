using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using PartnerApi.Client;
using PartnerApi.Services;
using PartnerApiClient.Utils;

[assembly: FunctionsStartup(typeof(DataLoader.Startup))]

namespace DataLoader
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient("HttpClient").AddPolicyHandler(PartnerApiRetryPolicy.GetRetryPolicy());
            builder.Services.AddSingleton<IPartnerApiService, PartnerApiService>();
            builder.Services.AddSingleton<IPartnerApiClient, PartnerApi.Client.PartnerApiClient>();
        }
    }
}