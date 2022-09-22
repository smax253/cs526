using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ImageSharingWithUpload.Controllers
{
    public class AccountController : Controller
    {
        protected void CheckAda()
        {
            var cookie = Request.Cookies["ADA"];
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
        public ActionResult Register()
        {
            CheckAda();
            return View();
        }

        [HttpPost]
        public ActionResult Register(String Userid, String ADA)
        {
            CheckAda();
            var options = new CookieOptions() { IsEssential = true, Expires = DateTime.Now.AddMonths(3)  };
            // TODO add cookies for "Userid" and "ADA"
            Response.Cookies.Append("Userid", Userid, options);

            if (ADA == null)
            {
                Response.Cookies.Append("ADA", "false", options);
                ViewBag.isADA = false;
            }
            else
            {
                Response.Cookies.Append("ADA", "true", options);
                ViewBag.isADA = true;
            }
            
            // End TODO

            ViewBag.Userid = Userid;
            return View("RegisterSuccess");
        }

    }
}