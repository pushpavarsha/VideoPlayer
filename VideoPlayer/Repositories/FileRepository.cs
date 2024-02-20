using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using System.Web;
using VideoPlayer.Models;
using VideoPlayer.Models.Domain;

namespace VideoPlayer.Services
{
    public class FileRepository:IFileRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _mediaFolderPath;
        public FileRepository(IServiceProvider serviceProvider)
        {
            _httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            _configuration = serviceProvider.GetRequiredService<IConfiguration>();
            _webHostEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
            _mediaFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, _configuration["MediaFolderPath"]);
            
        }
        public async Task<bool> UploadFiles(FileDetail files)
        {
            try
            {
                var apiBaseUrl = _configuration["UploadAPIEndpoint"];
                apiBaseUrl += $"?_mediaFolderPath={HttpUtility.UrlEncode(_mediaFolderPath)}";
                var client = _httpClientFactory.CreateClient();
                var multipartContent = new MultipartFormDataContent();

                foreach (var file in files.Files)
                {
                    if (file.Length > 0)
                    {
                        multipartContent.Add(new StreamContent(file.OpenReadStream()), "files", file.FileName);
                    }
                }
                
                var response = await client.PostAsync(apiBaseUrl, multipartContent);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false; 
            }
        }
        public IEnumerable<VideoFile> GetVideoFiles()
        {
            var files = Directory.GetFiles(_mediaFolderPath, "*.mp4")
                                 .Select(filePath => new VideoFile
                                 {
                                     FileName = Path.GetFileName(filePath),
                                     Size = new FileInfo(filePath).Length
                                 });
            return files;
        }
    }

}
