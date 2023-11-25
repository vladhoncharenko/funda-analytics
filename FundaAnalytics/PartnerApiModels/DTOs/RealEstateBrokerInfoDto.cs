namespace PartnerApiModels.DTOs
{
    public class RealEstateBrokerInfoDto
    {
        public int FundaId { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public int AmountOfHomesWithGarden { get; set; }

        public int AmountOfHomesWithBalconyOrTerrace { get; set; }

        public int AmountOfHomesWithGarage { get; set; }
    }
}