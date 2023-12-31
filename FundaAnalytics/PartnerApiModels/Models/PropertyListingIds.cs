﻿namespace PartnerApiModels.Models
{
    /// <summary>
    /// Property listing ids.
    /// </summary>
    public class PropertyListingIds
    {
        public List<PropertyListingInfo> PropertyListingInfo { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public int TotalAmountOfListings { get; set; }
    }

    /// <summary>
    /// Property listing info.
    /// </summary>
    public class PropertyListingInfo
    {
        public string Id { get; set; }

        public DateTime AddedDateTime { get; set; }
    }

    /// <summary>
    /// Paging info.
    /// </summary>
    public class PagingInfo
    {
        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }
    }
}