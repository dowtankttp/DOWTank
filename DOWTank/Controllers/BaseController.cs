using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using BootstrapSupport;
using DOWTank.Utility;

namespace DOWTank.Controllers
{
    public class BaseController : Controller
    {
        protected SecurityExtended SecurityExtended { get; set; }

        protected void PopulateSecurityExtended()
        {
            SecurityExtended = new SecurityExtended();
            if (HttpContext != null && HttpContext.User.Identity.IsAuthenticated)
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                SecurityExtended.SecurityProfileId = identity.GetSecurityProfileId();
                SecurityExtended.UserName = identity.GetUserName();
                SecurityExtended.LocationId = identity.GetLocationId().ToNullableInt32();
                SecurityExtended.LocationName = identity.GetLocationName();
            }
        }

        public void Attention(string message)
        {
            TempData.Add(Alerts.ATTENTION, message);
        }

        public void Success(string message)
        {
            TempData.Add(Alerts.SUCCESS, message);
        }

        public void Information(string message)
        {
            TempData.Add(Alerts.INFORMATION, message);
        }

        public void Error(string message)
        {
            TempData.Add(Alerts.ERROR, message);
        }
    }
}