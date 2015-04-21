using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOWTank.Core.Domain.TANK_usp_rpt;
using DOWTank.Core.Service;

namespace DOWTank.Controllers
{
    public class TankSearchController : Controller
    {
        private readonly IUtilityService _utilityService;

        public TankSearchController(IUtilityService utilityService)
        {
            _utilityService = utilityService;
        }
        //
        // GET: /TankSearch/
        public ActionResult Index()
        {
            TankSearchPostModel postModel;
            if (TempData["postModel"] != null)
            {
                postModel = (TankSearchPostModel)TempData["postModel"];
            }
            // database call

            //todo: re-factor it later, and search using postModel listed above
            var TANK_usp_rpt_TankSearch_spParams = new TANK_usp_rpt_TankSearch_spParams();
            TANK_usp_rpt_TankSearch_spParams.LocationID = 1;
            TANK_usp_rpt_TankSearch_spParams.EquipmentAN = "LT 1029";


            DataTable data = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_rpt_TankSearch", TANK_usp_rpt_TankSearch_spParams);

            //# database call

            @ViewBag.TotalRecords = data.Rows.Count;
            return View(data);
        }

        public ActionResult Search(TankSearchPostModel postModel)
        {
            TempData["postModel"] = postModel;
            return RedirectToAction("Index", "TankSearch");
        }

    }

    public class TankSearchPostModel
    {
        public string TankNumber { get; set; }
        public string Chassis { get; set; }
    }
}