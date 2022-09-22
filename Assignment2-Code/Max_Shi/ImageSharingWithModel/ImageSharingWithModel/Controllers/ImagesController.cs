using System;
using System.Collections.Generic;
using System.Linq;

using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

using ImageSharingWithModel.DAL;
using ImageSharingWithModel.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ImageSharingWithModel.Controllers
{
    public class ImagesController : BaseController
    {
        private readonly ApplicationDbContext db;

        private readonly IWebHostEnvironment hostingEnvironment;

        private readonly ILogger<ImagesController> logger;

        // Dependency injection
        public ImagesController(ApplicationDbContext db, IWebHostEnvironment environment, ILogger<ImagesController> logger)
        {
            this.db = db;
            this.hostingEnvironment = environment;
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
        }

        protected string imageDataFile(int id)
        {
            return Path.Combine(
               hostingEnvironment.WebRootPath,
               "data", "images", "img-" + id + ".jpg");
        }

        public static string imageContextPath(int id)
        {
            return "data/images/img-" + id + ".jpg";
        }


        [HttpGet]
        public ActionResult Upload()
        {
            CheckAda();
            String Username = GetLoggedInUser();
            if (Username == null)
            {
                return ForceLogin();
            }

            ViewBag.Message = "";
            ImageView imageView = new ImageView();
            imageView.Tags = new SelectList(db.Tags, "Id", "Name", 1);
            return View(imageView);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(ImageView imageView)
        {
            CheckAda();

            String Username = GetLoggedInUser();
            if (Username == null)
            {
                return ForceLogin();
            }

            await TryUpdateModelAsync(imageView);

            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Please correct the errors in the form!";
                ViewBag.Tags = new SelectList(db.Tags, "Id", "Name", 1);
                return View();
            }

            User user = await db.Users.SingleOrDefaultAsync(u => u.Username.Equals(Username));
            if (user == null)
            {
                ViewBag.Message = "No such Username registered!";
                return View();
            }

            if (imageView.ImageFile == null || imageView.ImageFile.Length <= 0)
            {
                ViewBag.Message = "No image file specified!";
                imageView.Tags = new SelectList(db.Tags, "Id", "Name", 1);
                return View(imageView);
            }

            Tag tag = db.Tags.Find(imageView.TagId);

            Image image = new Image { User = user, Tag = tag };
            image.Caption = imageView.Caption;
            image.TagId = imageView.TagId;
            image.Description = imageView.Description;
            image.DateTaken = imageView.DateTaken;
            image.UserId = user.Id;
            image.TagId = tag.Id;
            db.Images.Add(image);
            db.SaveChanges();

            mkDirectories();


            var imageFilePath = imageDataFile(image.Id);
            var imageFileStream = new FileStream(imageFilePath, FileMode.Create);
            await imageView.ImageFile.CopyToAsync(imageFileStream);

            return RedirectToAction("Details", new { Id = image.Id });
        }

        [HttpGet]
        public IActionResult Query()
        {
            CheckAda();
            if (GetLoggedInUser() == null)
            {
                return ForceLogin();
            }

            ViewBag.Message = "";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int Id)
        {
            CheckAda();
            if (GetLoggedInUser() == null)
            {
                return ForceLogin();
            }

            Image image = await db.Images.FindAsync(Id);
            if (image == null)
            {
                return RedirectToAction("Error", "Home", new { ErrId = "Details:" + Id });
            }

            ImageView imageView = new ImageView();
            imageView.Id = image.Id;
            imageView.Caption = image.Caption;
            imageView.Description = image.Description;
            imageView.DateTaken = image.DateTaken;
            /*
             * Eager loading of related entities
             */
            var imageEntry = db.Entry(image);
            await imageEntry.Reference(i => i.Tag).LoadAsync();
            await imageEntry.Reference(i => i.User).LoadAsync();
            imageView.TagName = image.Tag.Name;
            imageView.Username = image.User.Username;
            return View(imageView);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            CheckAda();
            if (GetLoggedInUser() == null)
            {
                return ForceLogin();
            }

            Image image = await db.Images.FindAsync(Id);
            if (image == null)
            {
                return RedirectToAction("Error", "Home", new { ErrId = "EditNotFound" });
            }

            String Username = GetLoggedInUser();
            await db.Entry(image).Reference(im => im.User).LoadAsync();  // Eager load of user
            if (!image.User.Username.Equals(Username))
            {
                return RedirectToAction("Error", "Home", new { ErrId = "EditNotAuth" });
            }

            ViewBag.Message = "";

            ImageView imageView = new ImageView();
            imageView.Tags = new SelectList(db.Tags, "Id", "Name", image.TagId);
            imageView.Id = image.Id;
            imageView.TagId = image.TagId;
            imageView.Caption = image.Caption;
            imageView.Description = image.Description;
            imageView.DateTaken = image.DateTaken;

            return View("Edit", imageView);
        }

        [HttpPost]
        public async Task<IActionResult> DoEdit(int Id, ImageView imageView)
        {
            CheckAda();
            String Username = GetLoggedInUser();
            if (Username == null)
            {
                return ForceLogin();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Please correct the errors on the page";
                imageView.Id = Id;
                imageView.Tags = new SelectList(db.Tags, "Id", "Name", imageView.TagId);
                return View("Edit", imageView);
            }

            logger.LogDebug("Saving changes to image " + Id);
            Image image = await db.Images.FindAsync(Id);
            if (image == null)
            {
                return RedirectToAction("Error", "Home", new { ErrId = "EditNotFound" });
            }

            await db.Entry(image).Reference(im => im.User).LoadAsync ();  // Explicit load of user
            if (!image.User.Username.Equals(Username))
            {
                return RedirectToAction("Error", "Home", new { ErrId = "EditNotAuth" });
            }

            image.TagId = imageView.TagId;
            image.Caption = imageView.Caption;
            image.Description = imageView.Description;
            image.DateTaken = imageView.DateTaken;
            db.Entry(image).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Details", new { Id = Id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            CheckAda();
            String Username = GetLoggedInUser();
            if (Username == null)
            {
                return ForceLogin();
            }

            Image image = await db.Images.FindAsync(Id);
            if (image == null)
            {
                return RedirectToAction("Error", "Home", new { ErrId = "Delete" });
            }

            await db.Entry(image).Reference(im => im.User).LoadAsync();  // Explicit load of user
            if (!image.User.Username.Equals(Username))
            {
                return RedirectToAction("Error", "Home", new { ErrId = "DeleteNotAuth" });
            }

            ImageView imageView = new ImageView();
            imageView.Id = image.Id;
            imageView.Caption = image.Caption;
            imageView.Description = image.Description;
            imageView.DateTaken = image.DateTaken;
            /*
             * Eager loading of related entities
             */
            await db.Entry(image).Reference(i => i.Tag).LoadAsync();
            imageView.TagName = image.Tag.Name;
            imageView.Username = image.User.Username;
            return View(imageView);
        }

        [HttpPost]
        public async Task<IActionResult> DoDelete(int Id)
        {
            CheckAda();
            String Username = GetLoggedInUser();
            if (Username == null)
            {
                return ForceLogin();
            }

            Image image = await db.Images.FindAsync(Id);
            if (image == null)
            {
                return RedirectToAction("Error", "Home", new { ErrId = "DeleteNotFound" });
            }

            await db.Entry(image).Reference(im => im.User).LoadAsync();  // Explicit load of user
            if (!image.User.Username.Equals(Username))
            {
                return RedirectToAction("Error", "Home", new { ErrId = "DeleteNotAuth" });
            }

            //db.Entry(imageEntity).State = EntityState.Deleted;
            db.Images.Remove(image);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        public IActionResult ListAll()
        {
            CheckAda();
            String Username = GetLoggedInUser();
            if (Username == null)
            {
                return ForceLogin();
            }

            IEnumerable<Image> images = db.Images.Include(im => im.User).Include(im => im.Tag);
            ViewBag.Username = Username;
            return View(images);

        }

        [HttpGet]
        public IActionResult ListByUser()
        {
            CheckAda();
            if (GetLoggedInUser() == null)
            {
                return ForceLogin();
            }

            ListByUserView listViewModel = new ListByUserView();
            listViewModel.Users = new SelectList(db.Users, "Id", "Username", 1);
            return View(listViewModel);


        }

        [HttpGet]
        public async Task<IActionResult> DoListByUser(ListByUserView userView)
        {
            CheckAda();
            String Username = GetLoggedInUser();
            if (Username == null)
            {
                return ForceLogin();
            }
            User user = db.Users.Find(userView.Id);
            if (user == null)
            {
                return RedirectToAction("Error", "Home", new { ErrId = "ListByUser" });
            }
            ViewBag.Username = Username;
            var images = db.Entry(user).Collection(t => t.Images).Query().Include(im => im.Tag).ToList();
            return View("ListAll", user.Images);



        }

        [HttpGet]
        public IActionResult ListByTag()
        {
            CheckAda();
            if (GetLoggedInUser() == null)
            {
                return ForceLogin();
            }

            ListByTagViewModel tagView = new ListByTagViewModel();
            tagView.Tags = new SelectList(db.Tags, "Id", "Name", 1);
            return View(tagView);

        }

        [HttpGet]
        public async Task<IActionResult> DoListByTag(ListByTagViewModel tagView)
        {
            CheckAda();
            String Username = GetLoggedInUser();
            if (Username == null)
            {
                return ForceLogin();
            }

            Tag tag = await db.Tags.FindAsync(tagView.Id);
            if (tag == null)
            {
                return RedirectToAction("Error", "Home", new { ErrId = "ListByTag" });
            }

            ViewBag.Username = Username;
            /*
             * Eager loading of related entities
             */
            var images = db.Entry(tag).Collection(t => t.Images).Query().Include(im => im.User).ToList();
            return View("ListAll", tag.Images);
        }
    }
}
