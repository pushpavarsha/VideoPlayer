namespace UploadVideoAPI.Exception
{
    public class NotFoundException: System.Exception
    {
        [NonSerialized]
        protected readonly string _sourceName;
        [NonSerialized] 
        protected const string ERROR_HOST = "https://localhost:7236";
        public NotFoundException(string message, string sourceName) : base(message)
        {
            _sourceName = sourceName;
        }
        public string SourceName => _sourceName;

        public virtual ErrorResult ToErrorResult()
        {
            return new ErrorResult
            {
                Status = StatusCodes.Status404NotFound,
                Type = $"{ERROR_HOST}/not-found",
                Title = "NotFound",
                Detail = Message,
                Source = _sourceName
            };
        }
    }
}
