namespace PartnerApiModels.DTOs;

public class PropertyListingDto
{
    public string InternalId { get; set; }

    public string PublicatieDatum { get; set; }

    public string Adres { get; set; }

    public string Tuin { get; set; }

    public string BalkonDakterras { get; set; }

    public string Garage { get; set; }

    public string Makelaar { get; set; }

    public int MakelaarId { get; set; }

    public string MakelaarTelefoon { get; set; }
}