namespace PartnerApiModels.DTOs
{
    public class PropertyListingIdsDto
    {
        public List<PropertyListingInfoDto> Objects { get; set; }

        public PagingInfoDto Paging { get; set; }

        public int TotaalAantalObjecten { get; set; }
    }

    public class PropertyListingInfoDto
    {
        public string Id { get; set; }

        public string PublicatieDatum { get; set; }
    }

    public class PagingInfoDto
    {
        public int AantalPaginas { get; set; }
        public int HuidigePagina { get; set; }
    }
}