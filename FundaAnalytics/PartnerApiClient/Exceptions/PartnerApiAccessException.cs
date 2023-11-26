namespace PartnerApiClient.Exceptions
{
    public class PartnerApiAccessException : Exception
    {
        public PartnerApiAccessException() { }

        public PartnerApiAccessException(string message) : base(message) { }

        public PartnerApiAccessException(string message, Exception inner) : base(message, inner) { }
    }
}
