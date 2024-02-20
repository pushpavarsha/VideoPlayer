using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;
using System.Reflection;

namespace UploadVideo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoUploadController : ControllerBase
    {
        private readonly long MaxFileSize;
        public VideoUploadController(IConfiguration configuration)
        {
            MaxFileSize = configuration.GetValue<long>("MaxFileSize");
        }

        [HttpPost]
        public IActionResult UploadFiles(List<IFormFile> files,string _mediaFolderPath)
        {
            if (files == null || files.Count == 0)
                return BadRequest("No files uploaded.");

            foreach (var file in files)
            {
                if (file.Length == 0)
                    return BadRequest("Empty file uploaded.");

                if (file.Length > MaxFileSize)
                    return BadRequest("File size exceeds the limit.");

                if (!Path.GetExtension(file.FileName).Equals(".mp4", StringComparison.OrdinalIgnoreCase))
                    return BadRequest("Only MP4 files are allowed.");

                var filePath = Path.Combine(_mediaFolderPath, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            return Ok("All files uploaded successfully.");
        }

    }
}
