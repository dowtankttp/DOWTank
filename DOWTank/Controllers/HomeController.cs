using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOWTank.Common;
using DOWTank.Core.Domain.TANK_usp_upd;
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

        [HttpPost]
        public JsonResult UpdateUserLocation(int locationId)
        {
            PopulateSecurityExtended();
            
            var TANK_usp_upd_SecurityDefaultLocation_spParams = new TANK_usp_upd_SecurityDefaultLocation_spParams()
                {
                    UserAn = SecurityExtended.UserName,
                    LocationID = locationId
                };
            _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_upd_SecurityDefaultLocation", TANK_usp_upd_SecurityDefaultLocation_spParams);

            return Json(1);
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