namespace VideoPlayer.Exception
{
    public class RequestBodyTooLargeException: System.Exception
    {
        public RequestBodyTooLargeException()
        {
        }

        public RequestBodyTooLargeException(string message) : base(message)
        {
        }

        public RequestBodyTooLargeException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
