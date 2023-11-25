using CacheClient.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using PartnerApi.Models;
using PartnerApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLoader
{
    public class DataLoader
    {
        private readonly IPartnerApiService _partnerApiService;
        private readonly ICacheService _cacheService;

        public DataLoader(IPartnerApiService partnerApiService, ICacheService cacheService)
        {
            _partnerApiService = partnerApiService;
            _cacheService = cacheService;
        }

        [FunctionName("DataLoader")]
        public async Task Run([TimerTrigger("*/5 * * * * *")] TimerInfo myTimer, [ServiceBus("property-listings-to-process", Connection = "ServiceBusConnectionString")] ICollector<string> messagesCollector, ILogger log)
        {
            try
            {
                log.LogInformation($"Property listing IDs setting.");

                // Getting property listing IDs.
                var propertyListingIds = await _partnerApiService.GetPropertyListingIdsAsync();

                // Adding property listing IDs to the cache.
                var listingsToAdd = propertyListingIds.ToDictionary(key => key, value => (PropertyListing)null);
                await _cacheService.SetDataAsync<IDictionary<string, PropertyListing>>("PropertyListings", "$", listingsToAdd);

                // Sending Service Bus messages, so new listings data can be hydrated.
                foreach (var propertyListingId in propertyListingIds)
                    messagesCollector.Add(propertyListingId);

                log.LogInformation($"{propertyListingIds.Count} property listing IDs were added.");
            }
            catch (Exception e)
            {
                log.LogError($"Error during DataLoader execution: {e}.");
            }
        }
    }
}