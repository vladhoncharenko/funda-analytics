using PartnerApi.Models;

namespace PartnerApi.Services
{
    public interface IPartnerApiService
    {
        Task<IList<string>> GetPropertyListingIdsAsync();

        Task<PropertyListing?> GetPropertyListingAsync(string propertyFundaId);
    }
}