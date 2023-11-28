using PartnerApiModels.Models;

namespace PartnerApi.Clients
{
    /// <summary>
    /// Represents a client for interacting with the Partner API.
    /// </summary>
    public interface IPartnerApiClient
    {
        /// <summary>
        /// Gets property listing details asynchronously based on propertyFundaId.
        /// </summary>
        /// <param name="propertyFundaId">The unique identifier of the property.</param>
        /// <returns>A Task that represents the asynchronous operation and contains the PropertyListing, or null if unsuccessful.</returns>
        Task<PropertyListing?> GetPropertyListingAsync(string propertyFundaId);

        /// <summary>
        /// Gets property listing IDs asynchronously based on page and pageSize.
        /// </summary>
        /// <param name="page">The page number of the result set.</param>
        /// <param name="pageSize">The number of items per page (default is 25).</param>
        /// <returns>A Task that represents the asynchronous operation and contains the PropertyListingIds, or null if unsuccessful.</returns>
        Task<PropertyListingIds?> GetPropertyListingIdsAsync(int page, int pageSize = 25);
    }
}