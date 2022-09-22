using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ImageSharingWithCloudStorage.Models;
using Microsoft.AspNetCore.Identity;
using ImageSharingWithCloudStorage.DAL;

namespace ImageSharingWithCloudStorage.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(UserManager<ApplicationUser> userManager, ApplicationDbContext db, ILogger<HomeController> logger)
            :base(userManager, db)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(String UserName = "Stranger")
        {
            CheckAda();
            ViewBag.Title = "Welcome!";
            ApplicationUser user = await GetLoggedInUser();
            if (user == null)
            {
                ViewBag.UserName = UserName;
            }
            else
            {
                ViewBag.UserName = user.UserName;
            }
            return View();
        }

        public IActionResult Privacy()
        {
            CheckAda();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            CheckAda();
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
