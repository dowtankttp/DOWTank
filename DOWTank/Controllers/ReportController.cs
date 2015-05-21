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
using DOWTank.Models;
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

        [HttpGet]
        public ActionResult AuditMissingMoves()
        {
            var postModel = new AuditMissingMovesPostModel();
            if (TempData["AuditMissingMovesPostModel"] != null)
            {
                // database call
                postModel = (AuditMissingMovesPostModel)TempData["AuditMissingMovesPostModel"];
                var TANK_usp_rpt_AuditMissingMoves_spParams = new TANK_usp_rpt_AuditMissingMoves_spParams()
                {
                    //TODO: re-factor it later from hard coded
                    LocationID = 1,
                    StartDT = postModel.StartDate,
                    EndDT = postModel.EndDate,
                    EquipmentAn = postModel.TankNumber
                };
                DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_rpt_AuditMissingMoves",
                                                                                      TANK_usp_rpt_AuditMissingMoves_spParams);

                DataSet dataSet = new DataSet("AuditMissingMoves");
                dataSet.Tables.Add(dataTable);

                _sharedFunctions.LoadExcel(dataTable);

                //# database call
            }


            return View(postModel);

        }

        [HttpPost]
        public ActionResult AuditMissingMoves(AuditMissingMovesPostModel postModel)
        {
            TempData["AuditMissingMovesPostModel"] = postModel;
            return RedirectToAction("AuditMissingMoves", "Report");
        }

        [HttpGet]
        public ActionResult AuditMovesByUser()
        {
            var postModel = new AuditMovesByUserPostModel();
            if (TempData["AuditMovesByUserPostModel"] != null)
            {
                // database call
                postModel = (AuditMovesByUserPostModel)TempData["AuditMovesByUserPostModel"];
                var TANK_usp_rpt_AuditMovesByUser_spParams = new TANK_usp_rpt_AuditMovesByUser_spParams()
                {
                    //TODO: re-factor it later from hard coded
                    LocationID = 1,
                    StartDT = postModel.StartDate,
                    EndDT = postModel.EndDate,
                    UserAN = postModel.UserAn
                };
                DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_rpt_AuditMovesByUser",
                                                                                      TANK_usp_rpt_AuditMovesByUser_spParams);

                _sharedFunctions.LoadExcel(dataTable);

                //# database call
            }


            return View(postModel);

        }

        [HttpPost]
        public ActionResult AuditMovesByUser(AuditMovesByUserPostModel postModel)
        {
            TempData["AuditMovesByUserPostModel"] = postModel;
            return RedirectToAction("AuditMovesByUser", "Report");
        }

        [HttpGet]
        public ActionResult AuditWarningMessages()
        {
            var postModel = new AuditWarningMessagesPostModel();
            if (TempData["AuditWarningMessagesPostModel"] != null)
            {
                // database call
                postModel = (AuditWarningMessagesPostModel)TempData["AuditWarningMessagesPostModel"];
                var TANK_usp_rpt_AuditWarningMessages_spParams = new TANK_usp_rpt_AuditWarningMessages_spParams()
                {
                    //TODO: re-factor it later from hard coded
                    LocationID = 1,
                    StartDT = postModel.StartDate,
                    EndDT = postModel.EndDate,
                    UserAN = postModel.UserAn
                };
                DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_rpt_AuditWarningMessages",
                                                                                      TANK_usp_rpt_AuditWarningMessages_spParams);

                _sharedFunctions.LoadExcel(dataTable);

                //# database call
            }


            return View(postModel);

        }

        [HttpPost]
        public ActionResult AuditWarningMessages(AuditWarningMessagesPostModel postModel)
        {
            TempData["AuditWarningMessagesPostModel"] = postModel;
            return RedirectToAction("AuditWarningMessages", "Report");
        }

        [HttpGet]
        public ActionResult DedicatedTanks()
        {
            var postModel = new DedicatedTanksPostModel();
            if (TempData["DedicatedTanksPostModel"] != null)
            {
                // database call
                postModel = (DedicatedTanksPostModel)TempData["DedicatedTanksPostModel"];
                var TANK_usp_rpt_DedicatedTanks_spParams = new TANK_usp_rpt_DedicatedTanks_spParams()
                {
                    //TODO: re-factor it later from hard coded
                    LocationID = 1,
                    CurrentLocationDS = postModel.CurrentLocationDS,
                    DedicatedLocationDS = postModel.DedicatedLocationDS,
                    DedicatedProductDS = postModel.DedicatedLocationDS
                };
                DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_rpt_DedicatedTanks",
                                                                                      TANK_usp_rpt_DedicatedTanks_spParams);

                _sharedFunctions.LoadExcel(dataTable);

                //# database call
            }


            return View(postModel);

        }

        [HttpPost]
        public ActionResult DedicatedTanks(DedicatedTanksPostModel postModel)
        {
            TempData["DedicatedTanksPostModel"] = postModel;
            return RedirectToAction("DedicatedTanks", "Report");
        }

        //PopulateContacts
        [HttpGet]
        public JsonResult PopulateSecurityDDL(string searchTerm)
        {
            searchTerm = searchTerm.Trim();
            //todo: re-factor it later as required
            var response = _sharedFunctions.PopulateSecurityDDL();
            if (response != null && response.Any())
            {
                var data = response.Where(r => r.UserAN != null).Select(r => new { id = r.UserAN, text = r.FullName }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
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
    public class AuditMissingMovesPostModel
    {
        public AuditMissingMovesPostModel()
        {
            StartDate = DateTime.Now.AddMonths(-1);
            EndDate = DateTime.Now;
        }
        public string TankNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class AuditMovesByUserPostModel
    {
        public AuditMovesByUserPostModel()
        {
            StartDate = DateTime.Now.AddMonths(-1);
            EndDate = DateTime.Now;
        }
        public string UserAn { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class AuditWarningMessagesPostModel
    {
        public AuditWarningMessagesPostModel()
        {
            StartDate = DateTime.Now.AddMonths(-1);
            EndDate = DateTime.Now;
        }
        public string UserAn { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class DedicatedTanksPostModel
    {
        public int LocationID { get; set; }
        public string CurrentLocationDS { get; set; }
        public string DedicatedLocationDS { get; set; }
        public string DedicatedProductDS { get; set; }
    }


}