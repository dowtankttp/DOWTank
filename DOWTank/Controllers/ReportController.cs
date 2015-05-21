using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DOWTank.Common;
using DOWTank.Core.Domain.TANK_usp_rpt;
using DOWTank.Core.Service;
using DOWTank.Reports;

namespace DOWTank.Controllers
{
    public class ReportController : Controller
    {
        private readonly IUtilityService _utilityService;
        private readonly ISharedFunctions _sharedFunctions;
        private byte[] _contentBytes;

        public ReportController(IUtilityService utilityService, ISharedFunctions sharedFunctions)
        {
            _utilityService = utilityService;
            _sharedFunctions = sharedFunctions;
        }
       
        public ActionResult AuditDriverList()
        {
            var postModel = new AuditDriverListPostModel();
            if (TempData["AuditDriverListPostModel"] != null)
            {
                // database call
                postModel = (AuditDriverListPostModel)TempData["AuditDriverListPostModel"];
                var TANK_usp_rpt_AuditDrivers_spParams = new TANK_usp_rpt_AuditDrivers_spParams()
                    {
                        //TODO: re-factor it later from hard coded
                        LocationID = 1,
                        StartDT = postModel.StartDate,
                        EndDT = postModel.EndDate
                    };
                DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_rpt_AuditDrivers",
                                                                                      TANK_usp_rpt_AuditDrivers_spParams);

                DataSet dataSet = new DataSet("DailyDriverActivity");
                dataSet.Tables.Add(dataTable);

                _sharedFunctions.LoadExcel(dataTable);

                //# database call
            }
            
            return View(postModel);

        }

        [HttpPost]
        public ActionResult AuditDriverList(AuditDriverListPostModel postModel)
        {
            TempData["AuditDriverListPostModel"] = postModel;
            return RedirectToAction("AuditDriverList", "Report");
        }




        private byte[] StreamToBytes(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

    }

    public class AuditDriverListPostModel
    {
        public AuditDriverListPostModel()
        {
            StartDate = DateTime.Now.AddMonths(-1);
            EndDate = DateTime.Now;
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}