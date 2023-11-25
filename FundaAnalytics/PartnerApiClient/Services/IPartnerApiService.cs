using PartnerApi.Models;

namespace PartnerApi.Services
{
    public interface IPartnerApiService
    {
        Task<IList<string>> GetAllNewPropertyListingIdsAsync(DateTime lastPropertyAddedDateTime);

        Task<PropertyListing?> GetPropertyListingAsync(string propertyFundaId);
    }
}