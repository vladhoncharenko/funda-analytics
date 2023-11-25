using Microsoft.Extensions.Logging;
using PartnerApi.Client;
using PartnerApi.Models;

namespace PartnerApi.Services
{
    public class PartnerApiService : IPartnerApiService
    {
        private readonly IPartnerApiClient _partnerAPIClient;
        private readonly ILogger<PartnerApiService> _logger = null;
        private readonly IHttpClientFactory _httpClientFactory = null;

        public PartnerApiService(IPartnerApiClient partnerAPIClient, ILogger<PartnerApiService> logger, IHttpClientFactory httpClientFactory)
        {
            _partnerAPIClient = partnerAPIClient;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IList<string>> GetAllNewPropertyListingIdsAsync(DateTime lastPropertyAddedDateTime)
        {
            using var httpClient = _httpClientFactory.CreateClient("HttpClient");

            try
            {
                var allNewPropertyListingIds = new List<string>();
                var propertyListingIds = await _partnerAPIClient.GetPropertyListingIdsAsync(httpClient, 1);
                var (newPropertyListingIds, areAllPropertiesNew) = GetNewPropertyListingsOnly(propertyListingIds.PropertyListingInfo, lastPropertyAddedDateTime);

                allNewPropertyListingIds.AddRange(newPropertyListingIds);
                if (!areAllPropertiesNew)
                    return allNewPropertyListingIds.Distinct().ToList();

                for (var pageNumber = 2; pageNumber <= propertyListingIds.PagingInfo.TotalPages; pageNumber++)
                {
                    propertyListingIds = await _partnerAPIClient.GetPropertyListingIdsAsync(httpClient, pageNumber);
                    (newPropertyListingIds, areAllPropertiesNew) = GetNewPropertyListingsOnly(propertyListingIds.PropertyListingInfo, lastPropertyAddedDateTime);
                    allNewPropertyListingIds.AddRange(newPropertyListingIds);

                    if (!areAllPropertiesNew)
                        break;
                }

                return allNewPropertyListingIds.Distinct().ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error during getting all new property listing IDs: {Error}", ex);
            }

            return new List<string>() { };
        }

        public async Task<PropertyListing?> GetPropertyListingAsync(string propertyFundaId)
        {
            return await _partnerAPIClient.GetPropertyListingAsync(propertyFundaId);
        }

        private (List<string> newPropertyListingIds, bool areAllPropertiesNew) GetNewPropertyListingsOnly(List<PropertyListingInfo> propertyListingInfo, DateTime lastPropertyAddedDateTime)
        {
            var newPropertyListingsOnly = propertyListingInfo.Where(x => x.AddedDateTime > lastPropertyAddedDateTime).Select(x => x.Id).ToList();

            return (newPropertyListingsOnly, newPropertyListingsOnly.Count() == propertyListingInfo.Count);
        }
    }
}
