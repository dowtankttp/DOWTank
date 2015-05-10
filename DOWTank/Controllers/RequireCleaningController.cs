using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOWTank.Common;
using DOWTank.Core.Domain.TANK_usp_rpt;
using DOWTank.Core.Service;

namespace DOWTank.Controllers
{
    public class RequireCleaningController : Controller
    {
        private readonly IUtilityService _utilityService;
        private readonly ISharedFunctions _sharedFunctions;

        public RequireCleaningController(IUtilityService utilityService, ISharedFunctions sharedFunctions)
        {
            _utilityService = utilityService;
            _sharedFunctions = sharedFunctions;
        }

        //
        // GET: /RequireCleaning/
        public ActionResult Index(int? page = 1)
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