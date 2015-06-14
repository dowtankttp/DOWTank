using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using DOWTank.Common;
using DOWTank.Core.Domain.TANK_usp_rpt;
using DOWTank.Core.Enum;
using DOWTank.Core.Service;
using DOWTank.Custom;
using DOWTank.Utility;

namespace DOWTank.Controllers
{
    [ClaimsAuthorize(Roles = "Require Cleaning")]
    public class RequireCleaningController : BaseController
    {
        private readonly IUtilityService _utilityService;
        private readonly ISharedFunctions _sharedFunctions;

        public RequireCleaningController(IUtilityService utilityService, ISharedFunctions sharedFunctions)
        {
            _utilityService = utilityService;
            _sharedFunctions = sharedFunctions;
        }

        [OutputCache(Duration = 3600, VaryByParam = "none")]
        public ActionResult Index(int? page = 1)
        {
            PopulateSecurityExtended();
            int securityProfileId = SecurityExtended.SecurityProfileId;
            var permissionList = _sharedFunctions.GetSecuritySettings(securityProfileId, (int)SecurityCatEnum.RequireCleaning, null);
            ViewBag.AccessDispatch = false;
            foreach (var permission in permissionList)
            {
                if (permission.PrivilegeDS == "Dispatch")
                {
                    ViewBag.AccessDispatch = (permission.GrantedFL == 1);
                }
            }

            // database call

            var TANK_usp_rpt_RequiresCleaning_spParams = new TANK_usp_rpt_RequiresCleaning_spParams()
            {
                InstallID = 1,
                LocationID = SecurityExtended.LocationId ?? 0
            };
            DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_rpt_RequiresCleaning", TANK_usp_rpt_RequiresCleaning_spParams);
            dataTable.Columns["EquipmentID"].SetOrdinal(8);

            //# database call
            @ViewBag.TotalRecords = dataTable.Rows.Count;

            return View(dataTable);

        }

        public ActionResult ViewInExcel()
        {
            // database call

            var TANK_usp_rpt_RequiresCleaning_spParams = new TANK_usp_rpt_RequiresCleaning_spParams()
            {
                //TODO: re-factor it later from hard coded
                InstallID = 1,
                LocationID = 1
            };
            DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_rpt_RequiresCleaning", TANK_usp_rpt_RequiresCleaning_spParams);
            dataTable.Columns["EquipmentID"].SetOrdinal(8);

            //# database call
            _sharedFunctions.LoadExcel(dataTable);

            return RedirectToAction("Index");
        }

    }
}