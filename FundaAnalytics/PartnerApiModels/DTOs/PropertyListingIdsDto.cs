namespace PartnerApiModels.DTOs
{
    /// <summary>
    /// DTO for Property Listing Ids
    /// </summary>
    public class PropertyListingIdsDto
    {
        public List<PropertyListingInfoDto> Objects { get; set; }

        public PagingInfoDto Paging { get; set; }

        public int TotaalAantalObjecten { get; set; }
    }

    /// <summary>
    /// DTO for Property Listing Info
    /// </summary>
    public class PropertyListingInfoDto
    {
        public string Id { get; set; }

        public string PublicatieDatum { get; set; }
    }

    /// <summary>
    /// DTO for Paging Info
    /// </summary>
    public class PagingInfoDto
    {
        public int AantalPaginas { get; set; }

        public int HuidigePagina { get; set; }
    }
}