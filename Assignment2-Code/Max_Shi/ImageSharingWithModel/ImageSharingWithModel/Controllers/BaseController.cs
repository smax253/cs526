using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ImageSharingWithModel.Controllers
{
    public class BaseController : Controller
    {
        protected void CheckAda()
        {
            ViewBag.isADA = GetADAFlag();
        }

        protected Boolean GetADAFlag()
        {
            var cookie = Request.Cookies["ADA"];
            return (cookie != null && "true".Equals(cookie));
        }

        protected String GetLoggedInUser()
        {
            return Request.Cookies["Username"];
        }

        protected ActionResult ForceLogin()
        {
            return RedirectToAction("Login", "Account");
        }
    }
}