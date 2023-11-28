namespace PartnerApiModels.Models
{
    /// <summary>
    /// Real estate broker.
    /// </summary>
    public class RealEstateBroker
    {
        public int FundaId { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        /// <summary>
        /// Returns true if the object is equal to this object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var other = (RealEstateBroker)obj;
            return FundaId == other.FundaId;
        }

        /// <summary>
        /// Returns the hash code of the object.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (FundaId).GetHashCode();
        }
    }
}
