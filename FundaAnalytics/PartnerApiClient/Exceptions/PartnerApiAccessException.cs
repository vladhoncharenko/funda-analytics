namespace PartnerApiClient.Exceptions
{
    /// <summary>
    /// Partner API access exception.
    /// Used to indicate that an exception occurred while accessing the Partner API.
    /// </summary>
    public class PartnerApiAccessException : Exception
    {
        public PartnerApiAccessException() { }

        public PartnerApiAccessException(string message) : base(message) { }

        public PartnerApiAccessException(string message, Exception inner) : base(message, inner) { }
    }
}
