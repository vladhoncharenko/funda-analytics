using Azure.Messaging.ServiceBus;
using CacheClient.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using PartnerApi.Services;
using PartnerApiModels.Models;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataHydrator
{
    public class DataHydrator
    {
        private readonly IPartnerApiService _partnerApiService;
        private readonly IJsonCacheService _cacheService;
        private readonly ServiceBusClient _serviceBusClient;

        public DataHydrator(IPartnerApiService partnerApiService, IJsonCacheService cacheService, ServiceBusClient serviceBusClient)
        {
            _partnerApiService = partnerApiService;
            _cacheService = cacheService;
            _serviceBusClient = serviceBusClient;
        }

        [FunctionName("DataHydrator")]
        public async Task Run([TimerTrigger("*/30 * * * * *")] TimerInfo myTimer, ILogger logger)
        {
            try
            {
                var receiver = _serviceBusClient.CreateReceiver("property-listings-to-process");
                var messages = await receiver.ReceiveMessagesAsync(Convert.ToInt32(Environment.GetEnvironmentVariable("RateLimitPerMinute")) / 2, TimeSpan.FromSeconds(15));
                logger.LogInformation("DataHydrator got {idsAmount} to process", messages.Count);

                foreach (var message in messages)
                {
                    // Getting the property listing ID to hydrate.
                    var propertyListingId = JsonSerializer.Deserialize<string>(message.Body);
                    logger.LogInformation("Property listing hydration started for property ID {id}", propertyListingId);
                    if (propertyListingId == null)
                        continue;

                    // Getting property listing details.
                    var propertyListing = await _partnerApiService.GetPropertyListingAsync(propertyListingId);
                    if (propertyListing == null)
                        continue;

                    // Saving property listing details.
                    var isDataAdded = await _cacheService.SetJsonDataAsync<PropertyListing>("PropertyListings", $"$.{propertyListing.FundaId}", propertyListing);
                    if (!isDataAdded)
                        continue;

                    await receiver.CompleteMessageAsync(message);
                }

                logger.LogInformation("Property listing hydration completed for {idsCount} property IDs.", messages.Count);
            }
            catch (Exception e)
            {
                logger.LogError("Error during executing DataHydrator for Property Listings: {error}.", e);
            }
        }
    }
}
