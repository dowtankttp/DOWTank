using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOWTank.Common;
using DOWTank.Core.Service;
using DOWTank.Custom;

namespace DOWTank.Controllers
{

    public class HomeController : BaseController
    {

        private readonly IUtilityService _utilityService;
        private readonly ISharedFunctions _sharedFunctions;

        public HomeController(IUtilityService utilityService, ISharedFunctions sharedFunctions)
        {
            _utilityService = utilityService;
            _sharedFunctions = sharedFunctions;
        }

        [ClaimsAuthorize(Roles = "")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AccessDenied()
        {

            return View();
        }

        public ActionResult _LocationSelection()
        {
            PopulateSecurityExtended();
            var list = new List<SelectListItem>();
            var response = _sharedFunctions.PopulateSecurityLocations(SecurityExtended.UserName);
            if (response != null && response.Any())
            {
                foreach (var item in response)
                {
                    if (item.LocationID == SecurityExtended.LocationId)
                    {
                        list.Add(new SelectListItem { Text = item.LocationDS, Value = item.LocationID.ToString(), Selected = true });
                    }
                    else
                    {
                        list.Add(new SelectListItem { Text = item.LocationDS, Value = item.LocationID.ToString() });
                    }
                }
                ViewBag.SecurityLocations = list;
            }
            return PartialView("_LocationSelection");
        }

    }
}