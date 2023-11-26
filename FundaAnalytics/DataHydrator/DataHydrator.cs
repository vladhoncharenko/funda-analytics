using Azure.Messaging.ServiceBus;
using CacheClient.Exceptions;
using CacheClient.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PartnerApi.Services;
using PartnerApiClient.Exceptions;
using PartnerApiModels.Models;
using System.Text.Json;

namespace DataHydrator
{
    public class DataHydrator
    {
        private readonly ILogger<DataHydrator> _logger;
        private readonly IPartnerApiService _partnerApiService;
        private readonly ICacheService _cacheService;

        public DataHydrator(ILogger<DataHydrator> logger, IPartnerApiService partnerApiService, ICacheService cacheService)
        {
            _logger = logger;
            _partnerApiService = partnerApiService;
            _cacheService = cacheService;
        }

        [Function(nameof(DataHydrator))]
        public async Task Run([ServiceBusTrigger("property-listings-to-process", Connection = "ServiceBusConnectionString")] ServiceBusReceivedMessage message)
        {
            var propertyListingId = string.Empty;
            try
            {
                // Getting the property listing ID to hydrate.
                propertyListingId = JsonSerializer.Deserialize<string>(message.Body);
                _logger.LogInformation("Property listing hydration started for property ID {id}", propertyListingId);

                if (propertyListingId == null)
                    throw new ArgumentNullException(nameof(propertyListingId));

                // Getting property listing details.
                var propertyListing = await _partnerApiService.GetPropertyListingAsync(propertyListingId);
                if (propertyListing == null)
                    throw new PartnerApiAccessException();

                // Saving property listing details.
                var isDataAdded = await _cacheService.SetDataAsync<PropertyListing>("PropertyListings", $"$.{propertyListing.FundaId}", propertyListing);
                if (!isDataAdded)
                    throw new CacheClientException("DataHydrator was not able to add the data to the cache.");

                _logger.LogInformation("Property listing hydration completed for property ID {id}", propertyListingId);
            }
            catch (PartnerApiAccessException e)
            {
                const string errorMessage = $"DataHydrator was not able to get the data from the Partner API. Throwing an error to retry message processing.";
                _logger.LogError(errorMessage);
                throw new Exception(errorMessage);
            }
            catch (CacheClientException e)
            {
                _logger.LogError("Cache error: {e}", e);
                throw new Exception("Cache error. Throwing an error to retry message processing.");
            }
            catch (Exception e)
            {
                _logger.LogError("Error during executing DataHydrator for Property Listing {id}: {error}", propertyListingId, e);
            }
        }
    }
}
