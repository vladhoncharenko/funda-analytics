namespace PartnerApiModels.Models
{
    public class RealEstateBroker
    {
        public int FundaId { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var other = (RealEstateBroker)obj;
            return FundaId == other.FundaId;
        }

        public override int GetHashCode()
        {
            return (FundaId).GetHashCode();
        }
    }
}
