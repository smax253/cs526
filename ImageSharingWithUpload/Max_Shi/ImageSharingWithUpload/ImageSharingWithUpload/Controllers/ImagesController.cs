using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;

using ImageSharingWithUpload.Models;
using Microsoft.Extensions.Logging;

namespace ImageSharingWithUpload.Controllers
{
    public class ImagesController : Controller
    {
        private readonly IWebHostEnvironment hostingEnvironment;

        private readonly ILogger logger;

        public ImagesController(IWebHostEnvironment environment, ILogger<ImagesController> logger)
        {
            hostingEnvironment = environment;
            this.logger = logger;
        }

        protected void mkDirectories()
        {
            var dataDir = Path.Combine(hostingEnvironment.WebRootPath,
               "data", "images");
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }
            var infoDir = Path.Combine(hostingEnvironment.WebRootPath,
               "data", "info");
            if (!Directory.Exists(infoDir))
            {
                Directory.CreateDirectory(infoDir);
            }
        }

        protected string imageDataFile(string id)
        {
            return Path.Combine(
               hostingEnvironment.WebRootPath,
               "data", "images", id + ".jpg");
        }

        protected string imageInfoFile(string id)
        {
            return Path.Combine(
               hostingEnvironment.WebRootPath,
               "data", "info", id + ".js");
        }

        protected void CheckAda()
        {
            var cookie = Request.Cookies["ADA"];
            logger.LogDebug("ADA cookie value: " + cookie);
            if (cookie != null && "true".Equals(cookie))
            {
                ViewBag.isADA = true;
            }
            else
            {
                ViewBag.isADA = false;
            }
        }


        [HttpGet]
        public IActionResult Upload()
        {
            CheckAda();
            ViewBag.Message = "";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(Image image,
                                    IFormFile imageFile)
        {
            CheckAda();

            if (ModelState.IsValid)
            {
                var userid = Request.Cookies["userid"];
                if (userid == null)
                {
                    return RedirectToAction("Register", "Account");
                }

                image.Userid = userid;

                /*
                 * Save image information on the server file system.
                 */

                if (imageFile != null && imageFile.Length > 0)
                {
                    mkDirectories();
                    if (imageFile.Length > 5000000)
                    {
                        ViewBag.Message = "Image must be smaller than 5MB!";
                        return View(image);
                    }
                    
                    System.Drawing.Image img = System.Drawing.Image.FromStream(imageFile.OpenReadStream());
                    if (imageFile.ContentType != "image/jpeg" || img.RawFormat.Guid != System.Drawing.Imaging.ImageFormat.Jpeg.Guid)
                    {
                        ViewBag.Message = "Image is not a valid JPEG!";
                        return View(image);
                    }

                    var jsonOptions = new JsonSerializerOptions { WriteIndented = true };

                    // TODO save image and metadata
                    var imageFilePath = Path.Combine(hostingEnvironment.WebRootPath, "data", "images", image.Id+".jpg");
                    var imageFileStream = new FileStream(imageFilePath, FileMode.Create);
                    await imageFile.CopyToAsync(imageFileStream);
                    var dataFilePath = Path.Combine(hostingEnvironment.WebRootPath, "data", "info", image.Id+".js");
                    System.IO.File.WriteAllText(dataFilePath, JsonSerializer.Serialize(image, jsonOptions));
                    return View("Details", image);
                }
                else
                {
                    ViewBag.Message = "No image file specified!";
                    return View(image);
                }

            }
            else
            {
                ViewBag.Message = "Please correct the errors in the form!";
                return View(image);
            }
        }

        [HttpGet]
        public IActionResult Query()
        {
            CheckAda();
            ViewBag.Message = "";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(Image image)
        {
            CheckAda();

            var userid = Request.Cookies["userid"];
            if (userid == null)
            {
                return RedirectToAction("Register", "Account");
            }

            String fileName = imageInfoFile(image.Id);
            if (System.IO.File.Exists(fileName))
            {
                String jsonData = await System.IO.File.ReadAllTextAsync(fileName);
                Image imageInfo = JsonSerializer.Deserialize<Image>(jsonData);

                return View(imageInfo);
            }
            else
            {
                ViewBag.Message = "Image with identifer " + image.Id + " not found";
                ViewBag.Id = image.Id;

                return View("Query");
            }

        }

    }
}