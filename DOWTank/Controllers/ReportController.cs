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

        [HttpGet]
        public ActionResult TankCostToDate()
        {
            var postModel = new TankCostToDatePostModel();
            if (TempData["TankCostToDatePostModel"] != null)
            {
                // database call
                postModel = (TankCostToDatePostModel)TempData["TankCostToDatePostModel"];
                var TANK_usp_rpt_TankCostToDate_spParams = new TANK_usp_rpt_TankCostToDate_spParams()
                {
                    //TODO: re-factor it later from hard coded
                    LocationID = 1,
                    StartDT = postModel.StartDate,
                    EndDT = postModel.EndDate,
                    OperatorID = postModel.OperatorID,
                    OwnerID = postModel.OwnerID,
                    PoolFL = postModel.PoolFL
                };
                DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_rpt_TankCostToDate",
                                                                                      TANK_usp_rpt_TankCostToDate_spParams);

                _sharedFunctions.LoadExcel(dataTable);

                //# database call
            }


            return View(postModel);

        }

        [HttpPost]
        public ActionResult TankCostToDate(TankCostToDatePostModel postModel)
        {
            TempData["TankCostToDatePostModel"] = postModel;
            return RedirectToAction("TankCostToDate", "Report");
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

        //PopulateOwnerDDL
        [HttpGet]
        public JsonResult PopulateOwnerDDL(string searchTerm)
        {
            searchTerm = searchTerm.Trim();
            //todo: re-factor it later as required
            var response = _sharedFunctions.PopulateOwnerDDL();
            if (response != null && response.Any())
            {
                var data = response.Where(r => r.OwnerID != null).Select(r => new { id = r.OwnerID, text = r.OwnerNM }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        //PopulateOperatorDDL
        [HttpGet]
        public JsonResult PopulateOperatorDDL(string searchTerm)
        {
            searchTerm = searchTerm.Trim();
            //todo: re-factor it later as required
            var response = _sharedFunctions.PopulateOperatorDDL();
            if (response != null && response.Any())
            {
                var data = response.Where(r => r.OperatorID != null).Select(r => new { id = r.OperatorID, text = r.OperatorNM }).ToList();
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


        public ActionResult HSEReport()
        {           

            return View();

        }

        [HttpPost]
        public ActionResult HSEReport(HSEReportPostModel hSEReportPostModel)
        {
            // database call                
            var TANK_usp_rpt_WasteExtract_spParams = new TANK_usp_rpt_WasteExtract_spParams()
            {
                //TODO: re-factor it later from hard coded
                LocationID = 1
            };
            DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_rpt_WasteExtract",
                                                                                      TANK_usp_rpt_WasteExtract_spParams);

            DataSet dataSet = new DataSet("HSEReport");
            dataSet.Tables.Add(dataTable);

            _sharedFunctions.LoadExcel(dataTable);

            //# database call
            return View();
        }

        public ActionResult IdleEquipment()
        {

            return View();

        }

        [HttpPost]
        public ActionResult IdleEquipment(IdleEquipmentPostModel idleEquipmentPostModel)
        {
            // database call                
            var TANK_usp_rpt_IdleEquipment_spParams = new TANK_usp_rpt_IdleEquipment_spParams()
            {
                //TODO: re-factor it later from hard coded
                LocationID = 1
            };
            DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_rpt_IdleEquipment",
                                                                                      TANK_usp_rpt_IdleEquipment_spParams);

            DataSet dataSet = new DataSet("IdleEquipment");
            dataSet.Tables.Add(dataTable);

            _sharedFunctions.LoadExcel(dataTable);

            //# database call
            return View();
        }

        public ActionResult InvoiceSummary()
        {
            var postModel = new InvoiceSummmaryPostModel();
            if (TempData["InvoiceSummmaryPostModel"] != null)
            {
                // database call
                postModel = (InvoiceSummmaryPostModel)TempData["InvoiceSummmaryPostModel"];
                var TANK_usp_rpt_InvoiceSummary_spParams = new TANK_usp_rpt_InvoiceSummary_spParams()
                {
                    //TODO: re-factor it later from hard coded
                    LocationID = 1,
                    StartDT = postModel.StartDate,
                    EndDT = postModel.EndDate
                };
                DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_rpt_InvoiceSummary",
                                                                                      TANK_usp_rpt_InvoiceSummary_spParams);

                DataSet dataSet = new DataSet("InvoiceSummary");
                dataSet.Tables.Add(dataTable);

                _sharedFunctions.LoadExcel(dataTable);

                //# database call
            }

            return View(postModel);

        }

        [HttpPost]
        public ActionResult InvoiceSummary(InvoiceSummmaryPostModel postModel)
        {
            TempData["InvoiceSummmaryPostModel"] = postModel;
            return RedirectToAction("InvoiceSummary", "Report");
        }

        public ActionResult FacilityEquipment()
        {
            PopulateFacilityEquipmentDropDowns();

            var postModel = new FacilityEquipmentPostModel();
            if (TempData["FacilityEquipmentPostModel"] != null)
            {
                // database call
                postModel = (FacilityEquipmentPostModel)TempData["FacilityEquipmentPostModel"];
                var TANK_usp_rpt_EquipmentInventory_spParams = new TANK_usp_rpt_EquipmentInventory_spParams()
                {
                    //TODO: re-factor it later from hard coded
                    LocationID = 1,
                    OwnerID=postModel.OwnerID,
                    BarrelConditionTypeCD = postModel.BarrelConditionTypeCD,
                    CurrentLocationDS = postModel.CurrentLocationDS,
                    EquipmentClassTypeCD = postModel.EquipmentClassTypeCD,
                    EquipmentTypeCD = postModel.EquipmentTypeCD,
                    LoadStatusTypeCD = postModel.LoadStatusTypeCD,
                    MoveTypeCD = postModel.MoveTypeCD,
                    OperatorID = postModel.OperatorID,
                    TankGradeTypeCD = postModel.TankGradeTypeCD
                };
                DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_rpt_EquipmentInventory",
                                                                                      TANK_usp_rpt_EquipmentInventory_spParams);

                
                _sharedFunctions.LoadExcel(dataTable);

                //# database call
            }

            return View(postModel);

        }

        [HttpPost]
        public ActionResult FacilityEquipment(FacilityEquipmentPostModel postModel)
        {
            TempData["FacilityEquipmentPostModel"] = postModel;
            return RedirectToAction("FacilityEquipment", "Report");
        }

        private void PopulateFacilityEquipmentDropDowns()
        {
            #region Equip Class

            var equipmentClassList = new List<SelectListItem>();
            var equipmentClassResponse = _sharedFunctions.PopulateEquipmentClassType();
            if (equipmentClassResponse != null && equipmentClassResponse.Any())
            {
                equipmentClassList.Add(new SelectListItem { Text = "", Value = "" });
                foreach (var item in equipmentClassResponse)
                {
                    equipmentClassList.Add(new SelectListItem { Text = item.EquipmentClassTypeDS, Value = item.EquipmentClassTypeCD.HasValue ? item.EquipmentClassTypeCD.Value.ToString() : string.Empty });
                }
                ViewBag.EquipmentClassTypeCD = equipmentClassList;
            }

            #endregion Equip Class

            #region Tank Grade

            var tankGradeList = new List<SelectListItem>();
            var tankGradeResponse = _sharedFunctions.PopulateTankGrade();
            if (tankGradeResponse != null && tankGradeResponse.Any())
            {
                tankGradeList.Add(new SelectListItem { Text = "", Value = "" });
                foreach (var item in tankGradeResponse)
                {
                    tankGradeList.Add(new SelectListItem { Text = item.TankGradeTypeDS, Value = item.TankGradeTypeCD.HasValue ? item.TankGradeTypeCD.Value.ToString() : string.Empty });
                }
                ViewBag.TankGradeTypeCD = tankGradeList;
            }


            #endregion Tank Grade

            #region Tank Condition

            var barrelConditionList = new List<SelectListItem>();
            var barrelConditionResponse = _sharedFunctions.PopulateBarrelCondition();
            if (barrelConditionResponse != null && barrelConditionResponse.Any())
            {
                barrelConditionList.Add(new SelectListItem { Text = "", Value = "" });
                foreach (var item in barrelConditionResponse)
                {
                    barrelConditionList.Add(new SelectListItem { Text = item.BarrelConditionTypeDS, Value = item.BarrelConditionTypeCD.HasValue ? item.BarrelConditionTypeCD.Value.ToString() : string.Empty });
                }
                ViewBag.BarrelConditionTypeCD = barrelConditionList;
            }


            #endregion Tank Condition

            #region PopulateLoadStatusType

            var statusTypeList = new List<SelectListItem>();
            var responseStustType = _sharedFunctions.PopulateLoadStatusType();
            if (responseStustType != null && responseStustType.Any())
            {
                statusTypeList.Add(new SelectListItem { Text = "", Value = "" });
                foreach (var item in responseStustType)
                {
                    statusTypeList.Add(new SelectListItem { Text = item.LoadStatusTypeDS, Value = item.LoadStatusTypeCD.ToString() });
                }
                ViewBag.LoadStatusTypeCD = statusTypeList;
            }

            #endregion PopulateLoadStatusType

            #region PopulateMoveType

            var moveTypeCDList = new List<SelectListItem>();
            var moveTypeCDType = _sharedFunctions.PopulateMoveType();
            if (moveTypeCDType != null && moveTypeCDType.Any())
            {
                moveTypeCDList.Add(new SelectListItem { Text = "", Value = "" });
                foreach (var item in moveTypeCDType)
                {
                    moveTypeCDList.Add(new SelectListItem { Text = item.MoveTypeDS, Value = item.MoveTypeCD.ToString() });
                }
                ViewBag.MoveTypeCD = moveTypeCDList;
            }

            #endregion PopulateMoveType
        }

        //PopulateEquipmentType
        [HttpGet]
        public JsonResult PopulateEquipmentType(string searchTerm, Int16? equipmentClassTypeCD)
        {
            searchTerm = searchTerm.Trim();
            //todo: re-factor it later as required
            var response = _sharedFunctions.PopulateEquipmentType(equipmentClassTypeCD);

            if (response != null && response.Any())
            {
                var data = response.Where(r => r.EquipmentTypeCD != null).Select(r => new { id = r.EquipmentTypeCD, text = r.EquipmentTypeDS }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

    }

    public class FacilityEquipmentPostModel
    {
        public int? OwnerID { get; set; }
        public int? OperatorID { get; set; }
        public int? EquipmentClassTypeCD { get; set; }
        public int? EquipmentTypeCD { get; set; }
        public int? TankGradeTypeCD { get; set; }
        public int? BarrelConditionTypeCD { get; set; }
        public int? LoadStatusTypeCD { get; set; }
        public string CurrentLocationDS { get; set; }
        public int? MoveTypeCD { get; set; }
    }

    public class HSEReportPostModel
    {
        public HSEReportPostModel()
        {

        }

    }

    public class IdleEquipmentPostModel
    {
        public IdleEquipmentPostModel()
        {

        }

    }

    public class InvoiceSummmaryPostModel
    {
        public InvoiceSummmaryPostModel()
        {
            StartDate = DateTime.Now.AddMonths(-1);
            EndDate = DateTime.Now;
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
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

    public class TankCostToDatePostModel
    {
        public TankCostToDatePostModel()
        {
            StartDate = DateTime.Now.AddMonths(-1);
            EndDate = DateTime.Now;
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int LocationID { get; set; }
        public int? OwnerID { get; set; }
        public int? OperatorID { get; set; }
        public bool PoolFL { get; set; }
    }


}