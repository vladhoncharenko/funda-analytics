using Microsoft.Extensions.Logging;
using PartnerApi.Clients;
using PartnerApiModels.Models;

namespace PartnerApi.Services
{
    public class PartnerApiService : IPartnerApiService
    {
        private readonly IPartnerApiClient _partnerApiClient;
        private readonly ILogger<PartnerApiService> _logger = null;

        public PartnerApiService(IPartnerApiClient partnerApiClient, ILogger<PartnerApiService> logger)
        {
            _partnerApiClient = partnerApiClient;
            _logger = logger;
        }

        public async Task<IList<string>> GetPropertyListingIdsAsync()
        {
            try
            {
                var allPropertyListingIds = new List<string>();
                var propertyListingIds = await _partnerApiClient.GetPropertyListingIdsAsync(1);
                var totalPages = propertyListingIds.PagingInfo.TotalPages;
                allPropertyListingIds.AddRange(propertyListingIds.PropertyListingInfo.Select(pli => pli.Id));

                for (var pageNumber = 2; pageNumber <= totalPages; pageNumber++)
                {
                    propertyListingIds = await _partnerApiClient.GetPropertyListingIdsAsync(pageNumber);
                    allPropertyListingIds.AddRange(propertyListingIds.PropertyListingInfo.Select(pli => pli.Id));
                }

                return allPropertyListingIds.Distinct().ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error during getting all new property listing IDs: {Error}", ex);
            }

            return new List<string>() { };
        }

        public async Task<PropertyListing?> GetPropertyListingAsync(string propertyFundaId)
        {
            return await _partnerApiClient.GetPropertyListingAsync(propertyFundaId);
        }
    }
}
