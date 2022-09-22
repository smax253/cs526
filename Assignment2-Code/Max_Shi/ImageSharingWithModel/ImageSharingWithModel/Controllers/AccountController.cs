using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using ImageSharingWithModel.DAL;
using ImageSharingWithModel.Models;

using Microsoft.EntityFrameworkCore;

namespace ImageSharingWithModel.Controllers
{
    public class AccountController : BaseController
    {
        private ApplicationDbContext db;

        // Dependency injection of DB context (see Startup)
        public AccountController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public ActionResult Register()
        {
            CheckAda();

            ViewBag.Username = GetLoggedInUser();
 
            ViewBag.Message = "";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserView info)
        {
            CheckAda();

            if (ModelState.IsValid)
            {
                User User = await db.Users.SingleOrDefaultAsync(u => u.Username.Equals(info.Username));
                if (User == null)
                {
                    // Save to database
                    User = new User(info.Username, info.IsADA());
                    db.Users.Add(User);
                }
                else
                {
                    User.ADA = info.IsADA();
                    db.Entry(User).State = EntityState.Modified;
                }

                await db.SaveChangesAsync();

                SaveCookies(User.Username, User.ADA);

                return RedirectToAction("Index", "Home", new { Username = info.Username });
            }
            else
            {
                ViewBag.Message = "Input validation errors!";
                return View();
            }

        }

        [HttpGet]
        public ActionResult Login()
        {
            CheckAda();
            ViewBag.Message = "";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserView info)
        {
            CheckAda();
            User User = await db.Users.SingleOrDefaultAsync(u => u.Username.Equals(info.Username));
            if (User != null)
            {
                SaveCookies(info.Username, User.ADA);
                return RedirectToAction("Index", "Home", new { Username = info.Username });
            }
            else
            {
                ViewBag.Message = "No such user registered!";
                return View("Login");
            }
        }

        protected void SaveCookies (String username, bool ADA) {
            SaveCookie("Username", username);
            SaveCookie("ADA", ADA.ToString());
        }

        protected void SaveCookie (String key, String value)
        {
            var options = new CookieOptions() {
                IsEssential = true,
                Expires = DateTime.Now.AddMonths(3),
                Path = "/"
            };
            Response.Cookies.Append(key, value, options);
        }

    }
}
