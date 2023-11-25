namespace PartnerApi.Models
{
    public class PropertyListingIds
    {
        public List<PropertyListingInfo> PropertyListingInfo { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public int TotalAmountOfListings { get; set; }
    }

    public class PropertyListingInfo
    {
        public string Id { get; set; }

        public DateTime AddedDateTime { get; set; }
    }

    public class PagingInfo
    {
        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }
    }
}