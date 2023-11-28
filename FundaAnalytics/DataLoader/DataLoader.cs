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
    /// <summary>
    /// Azure Function for loading property listing IDs from the Partner API into the cache.
    /// </summary>
    public class DataLoader
    {
        private readonly IPartnerApiService _partnerApiService;
        private readonly IJsonCacheService _cacheService;

        public DataLoader(IPartnerApiService partnerApiService, IJsonCacheService cacheService)
        {
            _partnerApiService = partnerApiService;
            _cacheService = cacheService;
        }

        /// <summary>
        /// Azure Function that runs on a timer trigger to load property listing IDs into the cache.
        /// </summary>
        /// <param name="myTimer">The timer trigger information.</param>
        /// <param name="messagesCollector">The Service Bus message collector.</param>
        /// <param name="logger">The logger for logging messages.</param>
        [FunctionName("DataLoader")]
        [ExponentialBackoffRetry(7, "00:00:15", "00:16:00")]
        public async Task Run([TimerTrigger("0 0 2 * * *")] TimerInfo myTimer, [ServiceBus("property-listings-to-process", Connection = "ServiceBusConnectionString")] ICollector<string> messagesCollector, ILogger logger)
        {
            try
            {
                logger.LogInformation($"Property listing IDs loading.");

                // Getting property listing IDs.
                var propertyListingIds = await _partnerApiService.GetPropertyListingIdsAsync();
                if (!propertyListingIds.Any())
                    throw new PartnerApiAccessException();

                // Adding property listing IDs to the cache.
                var listingsToAdd = propertyListingIds.ToDictionary(key => key, value => (PropertyListing)null);
                var isDataAdded = await _cacheService.SetJsonDataAsync<IDictionary<string, PropertyListing>>("PropertyListings", "$", listingsToAdd);
                if (!isDataAdded)
                    throw new CacheClientException("DataLoader was not able to add the data to the cache.");

                // Sending Service Bus messages, so new listings data can be hydrated.
                foreach (var propertyListingId in propertyListingIds)
                    messagesCollector.Add(JsonSerializer.Serialize(propertyListingId));

                logger.LogInformation($"{propertyListingIds.Count} property listing IDs were added.");
            }
            catch (PartnerApiAccessException e)
            {
                const string errorMessage = $"DataLoader was not able to get the data from the Partner API. Throwing an error to retry data loading.";
                logger.LogError(errorMessage);
                throw new Exception(errorMessage);
            }
            catch (CacheClientException e)
            {
                logger.LogError("Cache error: {e}", e);
                throw new Exception("Cache error. Throwing an error to retry data adding.");
            }
            catch (Exception e)
            {
                logger.LogError($"Unknown Error during DataLoader execution: {e}.");
            }
        }
    }
}