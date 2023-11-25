using PartnerApi.Models;

namespace PartnerApi.Client
{
    public interface IPartnerApiClient
    {
        Task<PropertyListing?> GetPropertyListingAsync(string propertyFundaId);

        Task<PropertyListingIds> GetPropertyListingIdsAsync(HttpClient httpClient, int page, int pageSize = 25);
    }
}