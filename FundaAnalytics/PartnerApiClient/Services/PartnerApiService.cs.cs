using Microsoft.Extensions.Logging;
using PartnerApi.Client;
using PartnerApi.Models;

namespace PartnerApi.Services
{
    public class PartnerApiService : IPartnerApiService
    {
        private readonly IPartnerApiClient _partnerAPIClient;
        private readonly ILogger<PartnerApiService> _logger = null;

        public PartnerApiService(IPartnerApiClient partnerAPIClient, ILogger<PartnerApiService> logger)
        {
            _partnerAPIClient = partnerAPIClient;
            _logger = logger;
        }

        public async Task<IList<string>> GetAllNewPropertyListingIdsAsync(DateTime lastPropertyAddedDateTime)
        {
            try
            {
                var allNewPropertyListingIds = new List<string>();
                var propertyListingIds = await _partnerAPIClient.GetPropertyListingIdsAsync(1);
                var totalPages = propertyListingIds.PagingInfo.TotalPages;
                allNewPropertyListingIds.AddRange(propertyListingIds.PropertyListingInfo.Select(pli => pli.Id));

                for (var pageNumber = 2; pageNumber <= totalPages; pageNumber++)
                {
                    propertyListingIds = await _partnerAPIClient.GetPropertyListingIdsAsync(pageNumber);
                    allNewPropertyListingIds.AddRange(propertyListingIds.PropertyListingInfo.Select(pli => pli.Id));
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
    }
}
