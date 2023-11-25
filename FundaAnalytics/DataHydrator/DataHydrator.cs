using Azure.Messaging.ServiceBus;
using CacheClient.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PartnerApi.Services;
using System.Text.Json;
using PartnerApiModels.Models;

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

                // Getting property listing details.
                var propertyListing = await _partnerApiService.GetPropertyListingAsync(propertyListingId);

                // Saving property listing details.
                await _cacheService.SetDataAsync<PropertyListing>("PropertyListings", $"$.{propertyListing.FundaId}", propertyListing);

                _logger.LogInformation("Property listing hydration completed for property ID {id}", propertyListingId);
            }
            catch (Exception e)
            {
                _logger.LogError("Error during executing DataHydrator for Property Listing {id}: {error}", propertyListingId, e);
            }
        }
    }
}
