using Microsoft.AspNetCore.Mvc;
using System.Web;
using VideoPlayer.Exception;
using VideoPlayer.Models;
using VideoPlayer.Models.Domain;
using VideoPlayer.Services;


namespace VideoPlayer.Controllers
{
    public class UploadController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly  string _mediaFolderPath;
        private readonly IFileRepository _filerepository;
        public UploadController(IHttpClientFactory httpClientFactory, IConfiguration configuration, IWebHostEnvironment hostingEnvironment, IFileRepository filerepository)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _mediaFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, _configuration["MediaFolderPath"]);
            _filerepository = filerepository;
        }

        [HttpGet]
        public IActionResult Catalogue(bool partial = false)
        {

            ViewBag.MediaFolderPath = _mediaFolderPath;
            var VideoFileModel = _filerepository.GetVideoFiles();
            if (partial)
            {
                return PartialView(VideoFileModel);
            }
            else
            {
                return View(VideoFileModel);
            }
        }

        [HttpGet]
        public IActionResult UploadFiles(bool partial = false)
        {
            if (partial)
            {
                return PartialView("UploadFiles");
            }
            else
            {
                return View("UploadFiles");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UploadFiles(FileDetail videoFiles)
        {
            try
            {
                if (videoFiles == null)
                {
                    throw new RequestBodyTooLargeException("whilst uploading file(s).Response Code 413.Please try again.");
                }
                if (videoFiles.Files == null || videoFiles.Files.Count == 0)
                {
                     ModelState.AddModelError("", "No files selected for upload.");
                    return PartialView("UploadFiles", videoFiles);
                }
                var success = await _filerepository.UploadFiles(videoFiles);
                if (success)
                {
                    return RedirectToAction("Catalogue", "Upload", new { partial = true });
                }
                else
                {
                    ModelState.AddModelError("", "Failed to upload files. Please try again later.");
                    return PartialView("UploadFiles", videoFiles);
                }
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return PartialView("UploadFiles", videoFiles);
            }
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }

    }
}
