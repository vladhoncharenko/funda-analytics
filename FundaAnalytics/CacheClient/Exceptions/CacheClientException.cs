namespace CacheClient.Exceptions
{
    public class CacheClientException : Exception
    {
        public CacheClientException() { }

        public CacheClientException(string message) : base(message) { }

        public CacheClientException(string message, Exception inner) : base(message, inner) { }
    }
}