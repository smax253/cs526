using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ImageSharingWithModel.Models;

namespace ImageSharingWithModel.Controllers
{
    public class HomeController : BaseController
    {

        [HttpGet]
        public IActionResult Index(String Username = "Stranger")
        {
            CheckAda();
            ViewBag.Title = "Welcome!";
            ViewBag.Username = Username;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string ErrId)
        {
            CheckAda();
            return View(new ErrorViewModel { ErrId = ErrId, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
