namespace PartnerApi.Models
{
    public class PropertyListing
    {
        public Guid Id { get; set; }

        public string FundaId { get; set; }

        public DateTime AddedDateTime { get; set; }

        public string Address { get; set; }

        public bool HasGarden { get; set; }

        public bool HasBalconyOrTerrace { get; set; }

        public bool HasGarage { get; set; }

        public RealEstateBroker Broker { get; set; }
    }
}