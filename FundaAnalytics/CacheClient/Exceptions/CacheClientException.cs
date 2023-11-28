namespace CacheClient.Exceptions
{
    /// <summary>
    /// Represents an exception thrown by the CacheClient.
    /// Used to wrap exceptions thrown by the underlying cache provider.
    /// </summary>
    public class CacheClientException : Exception
    {
        public CacheClientException() { }

        public CacheClientException(string message) : base(message) { }

        public CacheClientException(string message, Exception inner) : base(message, inner) { }
    }
}