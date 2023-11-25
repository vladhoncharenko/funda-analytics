using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using PartnerApi.Services;
using System;
using System.Threading.Tasks;

namespace DataLoader
{
    public class DataLoader
    {
        private readonly IPartnerApiService _partnerApiService;

        public DataLoader(IPartnerApiService partnerApiService)
        {
            _partnerApiService = partnerApiService;
        }

        [FunctionName("DataLoader")]
        public async Task Run([TimerTrigger("*/5 * * * * *")] TimerInfo myTimer, [ServiceBus("property-listings-to-process", Connection = "ServiceBusConnectionString")] ICollector<string> messagesCollector, ILogger log)
        {
            log.LogInformation($"Starting of new property listing IDs adding.");

            // Getting most recent property added date time.
            var cetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            var lastPropertyAddedDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cetTimeZone).AddDays(-2);

            // Getting all new property listing IDs.
            var newPropertyListingIds = await _partnerApiService.GetAllNewPropertyListingIdsAsync(lastPropertyAddedDateTime);

            // Saving new property listing IDs to the DB.

            // Sending Service Bus messages, so new listings data can be enriched.
            foreach (var newPropertyListingId in newPropertyListingIds)
            {
                messagesCollector.Add(newPropertyListingId);
            }

            log.LogInformation($"{newPropertyListingIds.Count} new property listing IDs were added.");
        }
    }
}