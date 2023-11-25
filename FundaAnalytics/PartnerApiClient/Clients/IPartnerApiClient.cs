using PartnerApi.Models;

namespace PartnerApi.Clients
{
    public interface IPartnerApiClient
    {
        Task<PropertyListing?> GetPropertyListingAsync(string propertyFundaId);

        Task<PropertyListingIds?> GetPropertyListingIdsAsync(int page, int pageSize = 25);
    }
}