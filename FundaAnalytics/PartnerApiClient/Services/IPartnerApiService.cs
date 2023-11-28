using PartnerApiModels.Models;

namespace PartnerApi.Services
{
    /// <summary>
    /// Service for interacting with the Partner API to retrieve property listing information.
    /// </summary>
    public interface IPartnerApiService
    {
        /// <summary>
        /// Gets all property listing IDs asynchronously.
        /// </summary>
        /// <returns>A list of unique property listing IDs.</returns>
        Task<IList<string>> GetPropertyListingIdsAsync();

        /// <summary>
        /// Gets property listing details asynchronously based on propertyFundaId.
        /// </summary>
        /// <param name="propertyFundaId">The unique identifier of the property.</param>
        /// <returns>A Task that represents the asynchronous operation and contains the PropertyListing, or null if unsuccessful.</returns>
        Task<PropertyListing?> GetPropertyListingAsync(string propertyFundaId);
    }
}