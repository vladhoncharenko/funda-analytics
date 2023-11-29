using Microsoft.Extensions.Logging;
using PartnerApi.Clients;
using PartnerApiClient.Exceptions;
using PartnerApiClient.RateLimiters;
using PartnerApiModels.Models;

namespace PartnerApi.Services
{
    /// <inheritdoc/>
    public class PartnerApiService : IPartnerApiService
    {
        private readonly IPartnerApiClient _partnerApiClient;
        private readonly ILogger<PartnerApiService> _logger;
        private readonly IRateLimiter _rateLimiter;

        public PartnerApiService(IPartnerApiClient partnerApiClient, ILogger<PartnerApiService> logger, IRateLimiter rateLimiter)
        {
            _partnerApiClient = partnerApiClient;
            _logger = logger;
            _rateLimiter = rateLimiter;
        }

        /// <inheritdoc/>
        public async Task<IList<string>> GetPropertyListingIdsAsync()
        {
            try
            {
                var allPropertyListingIds = new List<string>();
                var propertyListingIds = await _partnerApiClient.GetPropertyListingIdsAsync(1);
                if (propertyListingIds == null)
                    return new List<string>() { };

                if (await _rateLimiter.ShouldLimitRequestAsync("PartnerApiRateLimit"))
                    _rateLimiter.WaitTillLimitEnd();

                var totalPages = propertyListingIds.PagingInfo.TotalPages;
                allPropertyListingIds.AddRange(propertyListingIds.PropertyListingInfo.Select(pli => pli.Id));

                for (var pageNumber = 2; pageNumber <= totalPages; pageNumber++)
                {
                    if (await _rateLimiter.ShouldLimitRequestAsync("PartnerApiRateLimit"))
                        _rateLimiter.WaitTillLimitEnd();

                    propertyListingIds = await _partnerApiClient.GetPropertyListingIdsAsync(pageNumber);
                    if (propertyListingIds == null)
                        return new List<string>() { };

                    allPropertyListingIds.AddRange(propertyListingIds.PropertyListingInfo.Select(pli => pli.Id));
                }

                return allPropertyListingIds.Distinct().ToList();
            }
            catch (PartnerApiAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error during getting all new property listing IDs: {Error}", ex);
            }

            return new List<string>() { };
        }

        /// <inheritdoc/>
        public async Task<PropertyListing?> GetPropertyListingAsync(string propertyFundaId)
        {
            return await _partnerApiClient.GetPropertyListingAsync(propertyFundaId);
        }
    }
}
