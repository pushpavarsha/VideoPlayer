﻿using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Catalogue()
        {
            ViewBag.MediaFolderPath = _mediaFolderPath;
            var VideoFileModel = _filerepository.GetVideoFiles();
            return View(VideoFileModel);
        }
        [HttpGet]
        public IActionResult UploadFiles()
        { 
           return View();
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
                if (videoFiles.Files == null||videoFiles.Files.Count == 0)
                {
                    ModelState.AddModelError("", "No files selected for upload.");
                    return View();
                }
                var success = await _filerepository.UploadFiles(videoFiles);
                if(success)
                {
                    return RedirectToAction("Catalogue", "Upload");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to upload files. Please try again later.");
                    return View();
                }
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View();
            }
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }

    }
}