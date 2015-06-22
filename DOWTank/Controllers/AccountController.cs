using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using DOWTank.Models;
using DOWTank.Utility;
using Microsoft.Owin.Security;

namespace DOWTank.Controllers
{
    public class AccountController : BaseController
    {
        private UserManager _userManager;

        public AccountController(UserManager userManager)
        {
            _userManager = userManager;
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            var model = new LoginPostModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginPostModel postModel)
        {
            if (!ModelState.IsValid)
            {
                return View(postModel);
            }

            AuthenticationManager.SignOut("ApplicationCookie");
            DOWTank.Core.Domain.TANK_usp_sel.TANK_usp_sel_Security_spResults userDetails = null;

            if (_userManager.Athenticate(postModel.UserName, postModel.Password, ref userDetails))
            {
                var identity = _userManager.CreateIdentity(userDetails);
                AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);
                return RedirectToAction("Index", "Home");
            }
            Error("User Name or Password is not valid, please try again.");
            return View(postModel);
        }

        public ActionResult LogOut()
        {
            AuthenticationManager.SignOut("ApplicationCookie");
            return RedirectToAction("Login", "Account");
        }

        //Reason for comment: a public action method was not found on controller post
        //[HttpGet]
        //[AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetUserName()
        {
            var identity = User.Identity as ClaimsIdentity;
            var userName = identity.GetUserDisplayName();
            return Content(userName);
        }
    }
}