using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOWTank.Custom;

namespace DOWTank.Controllers
{
    
    public class HomeController : Controller
    {
        //[ClaimsAuthorize(Roles = "Dispatch")]
        [ClaimsAuthorize(Roles = "")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AccessDenied()
        {
            
            return View();
        }

        
    }
}