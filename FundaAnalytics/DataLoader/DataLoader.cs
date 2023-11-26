using CacheClient.Exceptions;
using CacheClient.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using PartnerApi.Services;
using PartnerApiClient.Exceptions;
using PartnerApiModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
        [ExponentialBackoffRetry(7, "00:00:15", "00:16:00")]
        public async Task Run([TimerTrigger("*/5 * * * * *")] TimerInfo myTimer, [ServiceBus("property-listings-to-process", Connection = "ServiceBusConnectionString")] ICollector<string> messagesCollector, ILogger log)
        {
            try
            {
                log.LogInformation($"Property listing IDs loading.");

                // Getting property listing IDs.
                var propertyListingIds = await _partnerApiService.GetPropertyListingIdsAsync();
                if (!propertyListingIds.Any())
                    throw new PartnerApiAccessException();

                // Adding property listing IDs to the cache.
                var listingsToAdd = propertyListingIds.ToDictionary(key => key, value => (PropertyListing)null);
                var isDataAdded = await _cacheService.SetDataAsync<IDictionary<string, PropertyListing>>("PropertyListings", "$", listingsToAdd);
                if (!isDataAdded)
                    throw new CacheClientException("DataLoader was not able to add the data to the cache.");

                // Sending Service Bus messages, so new listings data can be hydrated.
                foreach (var propertyListingId in propertyListingIds)
                    messagesCollector.Add(JsonSerializer.Serialize(propertyListingId));

                log.LogInformation($"{propertyListingIds.Count} property listing IDs were added.");
            }
            catch (PartnerApiAccessException e)
            {
                const string errorMessage = $"DataLoader was not able to get the data from the Partner API. Throwing an error to retry data loading.";
                log.LogError(errorMessage);
                throw new Exception(errorMessage);
            }
            catch (CacheClientException e)
            {
                log.LogError("Cache error: {e}", e);
                throw new Exception("Cache error. Throwing an error to retry data adding.");
            }
            catch (Exception e)
            {
                log.LogError($"Unknown Error during DataLoader execution: {e}.");
            }
        }
    }
}