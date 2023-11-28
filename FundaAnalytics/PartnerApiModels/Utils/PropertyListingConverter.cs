using PartnerApiModels.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PartnerApiModels.Utils
{
    /// <summary>
    /// Custom converter for PropertyListing class.
    /// Used to prevent deserialization of the Broker property.
    /// </summary>
    public class PropertyListingConverter : JsonConverter<PropertyListing>
    {
        public override PropertyListing Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return (JsonSerializer.Deserialize<PropertyListing>(ref reader, options) ?? default) ?? throw new JsonException("Error during deserializing the object.");
        }

        public override void Write(Utf8JsonWriter writer, PropertyListing value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString(nameof(PropertyListing.FundaId), value.FundaId);
            writer.WriteString(nameof(PropertyListing.AddedDateTime), value.AddedDateTime);
            writer.WriteString(nameof(PropertyListing.Address), value.Address);
            writer.WriteBoolean(nameof(PropertyListing.HasGarden), value.HasGarden);
            writer.WriteBoolean(nameof(PropertyListing.HasBalconyOrTerrace), value.HasBalconyOrTerrace);
            writer.WriteBoolean(nameof(PropertyListing.HasGarage), value.HasGarage);
            writer.WriteEndObject();
        }
    }
}