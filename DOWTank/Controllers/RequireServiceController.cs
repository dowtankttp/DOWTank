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
    [ClaimsAuthorize(Roles = "Require Service")]
    public class RequireServiceController : Controller
    {

        private readonly IUtilityService _utilityService;
        private readonly ISharedFunctions _sharedFunctions;

        public RequireServiceController(IUtilityService utilityService, ISharedFunctions sharedFunctions)
        {
            _utilityService = utilityService;
            _sharedFunctions = sharedFunctions;
        }

        [OutputCache(Duration = 3600, VaryByParam = "none")]
        public ActionResult Index()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int securityProfileId = identity.GetSecurityProfileId();
            var permissionList = _sharedFunctions.GetSecuritySettings(securityProfileId, (int)SecurityCatEnum.RequireService, null);

            // database call

            var TANK_usp_rpt_RequiresMaint_spParams = new TANK_usp_rpt_RequiresMaint_spParams()
            {
                //TODO: re-factor it later from hard coded
                LocationID = 1
            };
            DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_rpt_RequiresMaint", TANK_usp_rpt_RequiresMaint_spParams);
            dataTable.Columns["EquipmentID"].SetOrdinal(9);
            //# database call

            @ViewBag.TotalRecords = dataTable.Rows.Count;
            return View(dataTable);
        }

        public ActionResult ViewInExcel()
        {
            // database call

            var TANK_usp_rpt_RequiresMaint_spParams = new TANK_usp_rpt_RequiresMaint_spParams()
            {
                //TODO: re-factor it later from hard coded
                LocationID = 1
            };
            DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_rpt_RequiresMaint", TANK_usp_rpt_RequiresMaint_spParams);

            //# database call

            _sharedFunctions.LoadExcel(dataTable);

            return RedirectToAction("Index");
        }
    }
}