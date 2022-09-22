using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using ImageSharingWithSecurity.DAL;
using ImageSharingWithSecurity.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace ImageSharingWithSecurity.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        protected SignInManager<ApplicationUser> signInManager;

        private readonly ILogger<AccountController> logger;

        // Dependency injection of DB context and user/signin managers
        public AccountController(UserManager<ApplicationUser> userManager, 
                                 SignInManager<ApplicationUser> signInManager, 
                                 ApplicationDbContext db,
                                 ILogger<AccountController> logger) 
            : base(userManager, db)
        {
            this.signInManager = signInManager;
            this.logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            CheckAda();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            CheckAda();

            if (ModelState.IsValid)
            {
                logger.LogDebug("Registering user: " + model.Email);
                IdentityResult result = null;
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded) {
                    await signInManager.SignInAsync(user, true);
                    return RedirectToAction("Index", "Home");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            CheckAda();
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl)
        {
            CheckAda();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            ApplicationUser user = await userManager.FindByEmailAsync(model.UserName);
            if (user == null || !user.Active)
            {
                return View(model);
            }

            var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, true, false);
            if (result.Succeeded)
            {
                return RedirectToLocal(returnUrl);
            }

            return View(model);
        }

        //
        // GET: /Account/Password

       [HttpGet]
        public ActionResult Password(PasswordMessageId? message)
        {
            CheckAda();
            ViewBag.StatusMessage =
                 message == PasswordMessageId.ChangePasswordSuccess ? "Your password has been changed."
                 : message == PasswordMessageId.SetPasswordSuccess ? "Your password has been set."
                 : message == PasswordMessageId.RemoveLoginSuccess ? "The external login was removed."
                 : "";
            ViewBag.ReturnUrl = Url.Action("Password");
            return View();
        }

        //
        // POST: /Account/Password

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Password(LocalPasswordModel model)
        {
            CheckAda();

            ViewBag.ReturnUrl = Url.Action("Password");
            if (ModelState.IsValid)
            {
                IdentityResult idResult = null; ;

                idResult = await userManager.ChangePasswordAsync(await GetLoggedInUser(), model.OldPassword, model.NewPassword);
                ApplicationUser user = await GetLoggedInUser();

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Manage()
        {
            CheckAda();

            List<SelectListItem> users = new List<SelectListItem>();
            foreach (var u in db.Users)
            {
                SelectListItem item = new SelectListItem { Text = u.UserName, Value = u.Id, Selected = u.Active };
                users.Add(item);
            }

            ViewBag.message = "";
            ManageModel model = new ManageModel { Users = users };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Manage(ManageModel model)
        {
            CheckAda();

            foreach (var userItem in model.Users)
            {
                ApplicationUser user = await userManager.FindByIdAsync(userItem.Value);

                // Need to reset user name in view model before returning to user, it is not posted back
                userItem.Text = user.UserName;

                if (user.Active && !userItem.Selected)
                {
                    var images = db.Entry(user).Collection(u => u.Images).Query().ToList();
                    foreach (Image image in images)
                    {
                        db.Images.Remove(image);
                    }
                    user.Active = false;
                }
                else if (!user.Active && userItem.Selected)
                {
                    /*
                     * Reactivate a user
                     */
                    user.Active = true;
                }
            }
            await db.SaveChangesAsync();

            ViewBag.message = "Users successfully deactivated/reactivated";

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logoff()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        protected void SaveADACookie(bool value)
        {
            var options = new CookieOptions()
            {
                IsEssential = true,
                Expires = DateTime.Now.AddMonths(3),
                Path = "/"
            };
            Response.Cookies.Append("ADA", value.ToString(), options);
        }

        public enum PasswordMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

    }
}
