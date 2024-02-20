using Newtonsoft.Json;

namespace UploadVideoAPI.Exception
{
    public class ErrorResult
    {
        public int Status { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Source { get; set; }

    }
}
