﻿using System;
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
using DOWTank.Custom;
using DOWTank.Models;
using DOWTank.Reports;

namespace DOWTank.Controllers
{
    [ClaimsAuthorize(Roles = "")]
    public class ReportController : BaseController
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
            PopulateSecurityExtended();
            var postModel = new AuditDriverListPostModel();
            if (TempData["AuditDriverListPostModel"] != null)
            {
                // database call
                postModel = (AuditDriverListPostModel)TempData["AuditDriverListPostModel"];
                var TANK_usp_rpt_AuditDrivers_spParams = new TANK_usp_rpt_AuditDrivers_spParams()
                    {
                        LocationID = SecurityExtended.LocationId??0,
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
            PopulateSecurityExtended();	
            var postModel = new AuditMissingMovesPostModel();
            if (TempData["AuditMissingMovesPostModel"] != null)
            {
                // database call
                postModel = (AuditMissingMovesPostModel)TempData["AuditMissingMovesPostModel"];
                var TANK_usp_rpt_AuditMissingMoves_spParams = new TANK_usp_rpt_AuditMissingMoves_spParams()
                {
                    //TODO: re-factor it later from hard coded
                    LocationID = SecurityExtended.LocationId.Value,
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
            PopulateSecurityExtended();	
            var postModel = new AuditMovesByUserPostModel();
            if (TempData["AuditMovesByUserPostModel"] != null)
            {
                // database call
                postModel = (AuditMovesByUserPostModel)TempData["AuditMovesByUserPostModel"];
                var TANK_usp_rpt_AuditMovesByUser_spParams = new TANK_usp_rpt_AuditMovesByUser_spParams()
                {
                    //TODO: re-factor it later from hard coded
                    LocationID = SecurityExtended.LocationId.Value,
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
            PopulateSecurityExtended();	
            var postModel = new AuditWarningMessagesPostModel();
            if (TempData["AuditWarningMessagesPostModel"] != null)
            {
                // database call
                postModel = (AuditWarningMessagesPostModel)TempData["AuditWarningMessagesPostModel"];
                var TANK_usp_rpt_AuditWarningMessages_spParams = new TANK_usp_rpt_AuditWarningMessages_spParams()
                {
                    //TODO: re-factor it later from hard coded
                    LocationID = SecurityExtended.LocationId.Value,
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
            PopulateSecurityExtended();	
            var postModel = new DedicatedTanksPostModel();
            if (TempData["DedicatedTanksPostModel"] != null)
            {
                // database call
                postModel = (DedicatedTanksPostModel)TempData["DedicatedTanksPostModel"];
                var TANK_usp_rpt_DedicatedTanks_spParams = new TANK_usp_rpt_DedicatedTanks_spParams()
                {
                    //TODO: re-factor it later from hard coded
                    LocationID = SecurityExtended.LocationId.Value,
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
            PopulateSecurityExtended();	
            var postModel = new TankCostToDatePostModel();
            if (TempData["TankCostToDatePostModel"] != null)
            {
                // database call
                postModel = (TankCostToDatePostModel)TempData["TankCostToDatePostModel"];
                var TANK_usp_rpt_TankCostToDate_spParams = new TANK_usp_rpt_TankCostToDate_spParams()
                {
                    //TODO: re-factor it later from hard coded
                    LocationID = SecurityExtended.LocationId.Value,
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
            PopulateSecurityExtended();	
            searchTerm = searchTerm.Trim();
            //todo: re-factor it later as required
            var response = _sharedFunctions.PopulateSecurityDDL(SecurityExtended.LocationId.Value);
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

        //PopulateDrivers
        [HttpGet]
        public JsonResult PopulateDriverDDL(string searchTerm)
        {
            searchTerm = searchTerm.Trim();
            //todo: re-factor it later as required
            var response = _sharedFunctions.PopulateDrivers(false);
            if (response != null && response.Any())
            {
                var data = response.Where(r => r.DriverID != null).Select(r => new { id = r.DriverID, text = r.Driver }).ToList();
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
            PopulateSecurityExtended();	
            // database call                
            var TANK_usp_rpt_WasteExtract_spParams = new TANK_usp_rpt_WasteExtract_spParams()
            {
                //TODO: re-factor it later from hard coded
                LocationID = SecurityExtended.LocationId.Value
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
            PopulateSecurityExtended();	
            // database call                
            var TANK_usp_rpt_IdleEquipment_spParams = new TANK_usp_rpt_IdleEquipment_spParams()
            {
                //TODO: re-factor it later from hard coded
                LocationID = SecurityExtended.LocationId.Value
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
            PopulateSecurityExtended();	
            var postModel = new InvoiceSummmaryPostModel();
            if (TempData["InvoiceSummmaryPostModel"] != null)
            {
                // database call
                postModel = (InvoiceSummmaryPostModel)TempData["InvoiceSummmaryPostModel"];
                var TANK_usp_rpt_InvoiceSummary_spParams = new TANK_usp_rpt_InvoiceSummary_spParams()
                {
                    //TODO: re-factor it later from hard coded
                    LocationID = SecurityExtended.LocationId.Value,
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
            PopulateSecurityExtended();	
            PopulateFacilityEquipmentDropDowns();

            var postModel = new FacilityEquipmentPostModel();
            if (TempData["FacilityEquipmentPostModel"] != null)
            {
                // database call
                postModel = (FacilityEquipmentPostModel)TempData["FacilityEquipmentPostModel"];
                var TANK_usp_rpt_EquipmentInventory_spParams = new TANK_usp_rpt_EquipmentInventory_spParams()
                {
                    //TODO: re-factor it later from hard coded
                    LocationID = SecurityExtended.LocationId.Value,
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

        public ActionResult DailyDispatch()
        {
            PopulateSecurityExtended();	
            var postModel = new DailyDispatchPostModel();
            if (TempData["DailyDispatchPostModel"] != null)
            {
                // database call
                postModel = (DailyDispatchPostModel)TempData["DailyDispatchPostModel"];
                var TANK_usp_rpt_DailyDispatch_spParams = new TANK_usp_rpt_DailyDispatch_spParams()
                {
                    //TODO: re-factor it later from hard coded
                    LocationID = postModel.LocationID,
                    StartDT = postModel.StartDate,
                    EndDT = DateTime.Now
                };
                DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_rpt_DailyDispatch", TANK_usp_rpt_DailyDispatch_spParams);
                DataSet dataSet = new DataSet("DailyDispatch");
                dataSet.Tables.Add(dataTable);

                #region Report
                               
                var crDailyDispatch = new ReportDocument();
                string reportPath = Path.Combine(Server.MapPath("~/Reports"), "crDailyDispatch.rpt");
                crDailyDispatch.Load(reportPath);
                var dbServer = System.Web.Configuration.WebConfigurationManager.AppSettings["DBServer"];
                var dbName = System.Web.Configuration.WebConfigurationManager.AppSettings["DBName"];
                var dbUserId = System.Web.Configuration.WebConfigurationManager.AppSettings["DBUserId"];
                var dbPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["DBPassword"];
                crDailyDispatch.SetDatabaseLogon(dbUserId, dbPassword, dbServer, dbName, true);
                crDailyDispatch.SetDataSource(dataSet);
                //todo: these parameters ll be dynamic, but for proof of concept i kept them hard coded
                crDailyDispatch.SetParameterValue("@StartDT", postModel.StartDate);
                crDailyDispatch.SetParameterValue("@EndDT", DateTime.Now);
                crDailyDispatch.SetParameterValue("@LocationId", SecurityExtended.LocationId.Value);
                crDailyDispatch.SetParameterValue("LocationName", "");
                _contentBytes = StreamToBytes(crDailyDispatch.ExportToStream(ExportFormatType.PortableDocFormat));

                #endregion

                return File(_contentBytes, "application/pdf");
            }

            return View(postModel);
        }

        [HttpPost]
        public ActionResult DailyDispatch(DailyDispatchPostModel postModel)
        {
            TempData["DailyDispatchPostModel"] = postModel;
            return RedirectToAction("DailyDispatch", "Report");
        }

        public ActionResult ChassisActivity()
        {
            PopulateSecurityExtended();	
            var postModel = new ChassisActivityPostModel();
            if (TempData["ChassisActivityPostModel"] != null)
            {
                // database call
                postModel = (ChassisActivityPostModel)TempData["ChassisActivityPostModel"];
                var TANK_usp_rpt_ChassisActivity_spParams = new TANK_usp_rpt_ChassisActivity_spParams()
                {
                    //TODO: re-factor it later from hard coded
                    LocationID = postModel.LocationID,
                    StartDT = postModel.StartDate,
                    EndDT = postModel.EndDate
                };
                DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_rpt_MoveSummary", TANK_usp_rpt_ChassisActivity_spParams);
                DataSet dataSet = new DataSet("ChassisActivity");
                dataSet.Tables.Add(dataTable);

                #region Report

                var crChassisActivity = new ReportDocument();
                string reportPath = Path.Combine(Server.MapPath("~/Reports"), "crDailyMoveSummary.rpt");
                
                crChassisActivity.Load(reportPath);
                var dbServer = System.Web.Configuration.WebConfigurationManager.AppSettings["DBServer"];
                var dbName = System.Web.Configuration.WebConfigurationManager.AppSettings["DBName"];
                var dbUserId = System.Web.Configuration.WebConfigurationManager.AppSettings["DBUserId"];
                var dbPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["DBPassword"];
                crChassisActivity.SetDatabaseLogon(dbUserId, dbPassword, dbServer, dbName, true);
                crChassisActivity.SetDataSource(dataSet);
                //todo: these parameters ll be dynamic, but for proof of concept i kept them hard coded
                crChassisActivity.SetParameterValue("@StartDT", postModel.StartDate);
                crChassisActivity.SetParameterValue("@EndDT", postModel.EndDate);
                crChassisActivity.SetParameterValue("@LocationId", SecurityExtended.LocationId.Value);
                crChassisActivity.SetParameterValue("LocationName", "");
                _contentBytes = StreamToBytes(crChassisActivity.ExportToStream(ExportFormatType.PortableDocFormat));

                #endregion

                return File(_contentBytes, "application/pdf");
            }

            return View(postModel);
        }

        [HttpPost]
        public ActionResult ChassisActivity(ChassisActivityPostModel postModel)
        {
            TempData["ChassisActivityPostModel"] = postModel;
            return RedirectToAction("ChassisActivity", "Report");
        }

        public ActionResult LastChassisLocation()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LastChassisLocation(String Status)
        {
            PopulateSecurityExtended();	
            var TANK_usp_rpt_ChassisLastLocation_spParams = new TANK_usp_rpt_ChassisLastLocation_spParams()
            {
                //TODO: re-factor it later from hard coded
                LocationId = SecurityExtended.LocationId.Value,
                InstallId = 1
            };
            DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_rpt_LastChassisLocation", TANK_usp_rpt_ChassisLastLocation_spParams);
            DataSet dataSet = new DataSet("LastChassisLocation");
            dataSet.Tables.Add(dataTable);

            #region Report

            var crLastChassisLocation = new ReportDocument();
            string reportPath = Path.Combine(Server.MapPath("~/Reports"), "crLastChassisLocation.rpt");

            crLastChassisLocation.Load(reportPath);
            var dbServer = System.Web.Configuration.WebConfigurationManager.AppSettings["DBServer"];
            var dbName = System.Web.Configuration.WebConfigurationManager.AppSettings["DBName"];
            var dbUserId = System.Web.Configuration.WebConfigurationManager.AppSettings["DBUserId"];
            var dbPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["DBPassword"];
            crLastChassisLocation.SetDatabaseLogon(dbUserId, dbPassword, dbServer, dbName, true);
            crLastChassisLocation.SetDataSource(dataSet);
            //todo: these parameters ll be dynamic, but for proof of concept i kept them hard coded
            crLastChassisLocation.SetParameterValue("@LocationID", SecurityExtended.LocationId.Value);
            crLastChassisLocation.SetParameterValue("@InstallID", 1);
            _contentBytes = StreamToBytes(crLastChassisLocation.ExportToStream(ExportFormatType.PortableDocFormat));

            #endregion

            return File(_contentBytes, "application/pdf");
        }

        public ActionResult DriverActivity()
        {
            var postModel = new DriverActivityPostModel();
            if (TempData["DriverActivityPostModel"] != null)
            {
                // database call
                postModel = (DriverActivityPostModel)TempData["DriverActivityPostModel"];
                var TANK_usp_rpt_DriverActivity_spParams = new TANK_usp_rpt_DriverActivity_spParams()
                {
                    //TODO: re-factor it later from hard coded
                    DriverID = postModel.DriverID,
                    LocationID = postModel.LocationID,
                    StartDT = postModel.StartDate,
                    EndDT = postModel.EndDate
                };
                DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_rpt_DriverActivity", TANK_usp_rpt_DriverActivity_spParams);
                DataSet dataSet = new DataSet("DriverActivity");
                dataSet.Tables.Add(dataTable);

                #region Report

                var crDriverActivity = new ReportDocument();
                string reportPath = Path.Combine(Server.MapPath("~/Reports"), "crDailyDriverActivity.rpt");

                crDriverActivity.Load(reportPath);
                var dbServer = System.Web.Configuration.WebConfigurationManager.AppSettings["DBServer"];
                var dbName = System.Web.Configuration.WebConfigurationManager.AppSettings["DBName"];
                var dbUserId = System.Web.Configuration.WebConfigurationManager.AppSettings["DBUserId"];
                var dbPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["DBPassword"];
                crDriverActivity.SetDatabaseLogon(dbUserId, dbPassword, dbServer, dbName, true);
                crDriverActivity.SetDataSource(dataSet);
                //todo: these parameters ll be dynamic, but for proof of concept i kept them hard coded
                crDriverActivity.SetParameterValue("@StartDT", postModel.StartDate);
                crDriverActivity.SetParameterValue("@EndDT", postModel.EndDate);
                crDriverActivity.SetParameterValue("@EndDT", postModel.EndDate);
                crDriverActivity.SetParameterValue("@DriverID", postModel.DriverID);
                crDriverActivity.SetParameterValue("@LocationID", postModel.LocationID);
                crDriverActivity.SetParameterValue("LocationName", "");
                _contentBytes = StreamToBytes(crDriverActivity.ExportToStream(ExportFormatType.PortableDocFormat));

                #endregion

                return File(_contentBytes, "application/pdf");
            }
            else
            {
                #region "Populate Drivers"

                var DriverList = new List<SelectListItem>();
                var Drivers = _sharedFunctions.PopulateDrivers(false);
                if (Drivers != null && Drivers.Any())
                {
                    DriverList.Add(new SelectListItem { Text = "", Value = "" });
                    foreach (var item in Drivers)
                    {
                        DriverList.Add(new SelectListItem { Text = item.Driver, Value = item.DriverID.ToString() });
                    }
                    ViewBag.ddlDriver = DriverList;
                }

                #endregion
            }

            return View(postModel);
        }

        [HttpPost]
        public ActionResult DriverActivity(DriverActivityPostModel postModel)
        {
            TempData["DriverActivityPostModel"] = postModel;
            return RedirectToAction("DriverActivity", "Report");
        }

        public ActionResult LastTankMove()
        {
            var postModel = new LastTankMovePostModel();
            if (TempData["LastTankMovePostModel"] != null)
            {
                // database call
                postModel = (LastTankMovePostModel)TempData["LastTankMovePostModel"];
                var TANK_usp_rpt_LastTankMove_spParams = new TANK_usp_rpt_LastTankMove_spParams()
                {
                    //TODO: re-factor it later from hard coded
                    LocationID = postModel.LocationID,
                    InstallID = postModel.InstallID,
                    PoolFL = postModel.PoolFL
                };
                DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_rpt_LastTankMove", TANK_usp_rpt_LastTankMove_spParams);
                DataSet dataSet = new DataSet("LastTankMove");
                dataSet.Tables.Add(dataTable);

                #region Report

                var crDriverActivity = new ReportDocument();
                string reportPath = Path.Combine(Server.MapPath("~/Reports"), "crLastTankMove.rpt");

                crDriverActivity.Load(reportPath);
                var dbServer = System.Web.Configuration.WebConfigurationManager.AppSettings["DBServer"];
                var dbName = System.Web.Configuration.WebConfigurationManager.AppSettings["DBName"];
                var dbUserId = System.Web.Configuration.WebConfigurationManager.AppSettings["DBUserId"];
                var dbPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["DBPassword"];
                crDriverActivity.SetDatabaseLogon(dbUserId, dbPassword, dbServer, dbName, true);
                crDriverActivity.SetDataSource(dataSet);
                //todo: these parameters ll be dynamic, but for proof of concept i kept them hard coded
                crDriverActivity.SetParameterValue("@InstallID", postModel.InstallID);
                crDriverActivity.SetParameterValue("@LocationID", postModel.LocationID);
                crDriverActivity.SetParameterValue("@PoolFL", postModel.PoolFL);
                _contentBytes = StreamToBytes(crDriverActivity.ExportToStream(ExportFormatType.PortableDocFormat));

                #endregion

                return File(_contentBytes, "application/pdf");
            }
            else
            {
                #region Pool

                var PoolList = new List<SelectListItem>();
                PoolList.Add(new SelectListItem { Text = "-All-", Value = "-1" });
                PoolList.Add(new SelectListItem { Text = "Pool", Value = "1" });
                PoolList.Add(new SelectListItem { Text = "Non", Value = "0" });

                ViewBag.PoolFL = PoolList;

                #endregion LoadPoint
            }

            return View(postModel);
        }

        [HttpPost]
        public ActionResult LastTankMove(LastTankMovePostModel postModel)
        {
            TempData["LastTankMovePostModel"] = postModel;
            return RedirectToAction("LastTankMove", "Report");
        }

        public ActionResult TankShipmentSchedule()
        {
            var postModel = new TankShipmentSchedulePostModel();
            if (TempData["TankShipmentSchedulePostModel"] != null)
            {
                // database callSe
                postModel = (TankShipmentSchedulePostModel)TempData["TankShipmentSchedulePostModel"];
                var TANK_usp_rpt_TankShipmentSchedule_spParams = new TANK_usp_rpt_TankShipmentSchedule_spParams()
                {
                    //TODO: re-factor it later from hard coded
                    InstallID = postModel.InstallID,
                    LocationID = postModel.LocationID,
                    DispatchedFL = postModel.DispatchedFL,
                    StartDT = postModel.StartDate,
                    EndDT=postModel.EndDate,
                    EquipmentAN = postModel.EquipmentAN
                };
                DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_rpt_PreppedMoves", TANK_usp_rpt_TankShipmentSchedule_spParams);
                DataSet dataSet = new DataSet("TankShipmentSchedule");
                dataSet.Tables.Add(dataTable);

                #region Report

                var crDriverActivity = new ReportDocument();
                string reportPath = Path.Combine(Server.MapPath("~/Reports"), "crTankPrep.rpt");

                crDriverActivity.Load(reportPath);
                var dbServer = System.Web.Configuration.WebConfigurationManager.AppSettings["DBServer"];
                var dbName = System.Web.Configuration.WebConfigurationManager.AppSettings["DBName"];
                var dbUserId = System.Web.Configuration.WebConfigurationManager.AppSettings["DBUserId"];
                var dbPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["DBPassword"];
                crDriverActivity.SetDatabaseLogon(dbUserId, dbPassword, dbServer, dbName, true);
                crDriverActivity.SetDataSource(dataSet);
                //todo: these parameters ll be dynamic, but for proof of concept i kept them hard coded
                crDriverActivity.SetParameterValue("LocationName", postModel.LocationName);
                crDriverActivity.SetParameterValue("@InstallID", postModel.InstallID);
                crDriverActivity.SetParameterValue("@LocationID", postModel.LocationID);
                crDriverActivity.SetParameterValue("@DispatchedFL", postModel.DispatchedFL);
                crDriverActivity.SetParameterValue("@StartDT", postModel.StartDate);
                crDriverActivity.SetParameterValue("@EndDT", postModel.EndDate);
                _contentBytes = StreamToBytes(crDriverActivity.ExportToStream(ExportFormatType.PortableDocFormat));

                #endregion

                return File(_contentBytes, "application/pdf");
            }

            return View(postModel);
        }

        [HttpPost]
        public ActionResult TankShipmentSchedule(TankShipmentSchedulePostModel postModel)
        {
            TempData["TankShipmentSchedulePostModel"] = postModel;
            return RedirectToAction("TankShipmentSchedule", "Report");
        }

        public ActionResult TankActivity()
        {
            var postModel = new TankActivityPostModel();
            if (TempData["TankActivityPostModel"] != null)
            {
                // database callSe
                postModel = (TankActivityPostModel)TempData["TankActivityPostModel"];
                var TANK_usp_rpt_TankActivity_spParams = new TANK_usp_rpt_TankActivity_spParams()
                {
                    //TODO: re-factor it later from hard coded
                    EquipmentAN = postModel.EquipmentAN,
                    StartDT = postModel.StartDate,
                    EndDT = postModel.EndDate,
                    OwnerID = postModel.OwnerID,
                    OperatorID = postModel.OperatorID,
                    PoolFL = postModel.PoolFL,
                    LocationID = postModel.LocationID
                };
                DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_rpt_TankActivity", TANK_usp_rpt_TankActivity_spParams);
                DataSet dataSet = new DataSet("TankActivity");
                dataSet.Tables.Add(dataTable);

                #region Report

                var crDriverActivity = new ReportDocument();
                string reportPath = Path.Combine(Server.MapPath("~/Reports"), "crTankActivity.rpt");

                crDriverActivity.Load(reportPath);
                var dbServer = System.Web.Configuration.WebConfigurationManager.AppSettings["DBServer"];
                var dbName = System.Web.Configuration.WebConfigurationManager.AppSettings["DBName"];
                var dbUserId = System.Web.Configuration.WebConfigurationManager.AppSettings["DBUserId"];
                var dbPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["DBPassword"];
                crDriverActivity.SetDatabaseLogon(dbUserId, dbPassword, dbServer, dbName, true);
                crDriverActivity.SetDataSource(dataSet);
                //todo: these parameters ll be dynamic, but for proof of concept i kept them hard coded
                crDriverActivity.SetParameterValue("LocationName", postModel.LocationName);
                crDriverActivity.SetParameterValue("@EquipmentAN", postModel.EquipmentAN);
                crDriverActivity.SetParameterValue("@StartDT", postModel.StartDate);
                crDriverActivity.SetParameterValue("@EndDT", postModel.EndDate);
                crDriverActivity.SetParameterValue("@OwnerID", postModel.OwnerID);
                crDriverActivity.SetParameterValue("@OperatorID", postModel.OperatorID);
                crDriverActivity.SetParameterValue("@PoolFL", postModel.PoolFL);
                crDriverActivity.SetParameterValue("@LocationID", postModel.LocationID);
                
                _contentBytes = StreamToBytes(crDriverActivity.ExportToStream(ExportFormatType.PortableDocFormat));

                #endregion

                return File(_contentBytes, "application/pdf");
            }
            else
            {
                #region Pool

                var PoolList = new List<SelectListItem>();
                PoolList.Add(new SelectListItem { Text = "-All-", Value = "-1" });
                PoolList.Add(new SelectListItem { Text = "Pool", Value = "1" });
                PoolList.Add(new SelectListItem { Text = "Non", Value = "0" });

                ViewBag.PoolFL = PoolList;

                #endregion LoadPoint
            }

            return View(postModel);
        }

        [HttpPost]
        public ActionResult TankActivity(TankActivityPostModel postModel)
        {
            TempData["TankActivityPostModel"] = postModel;
            return RedirectToAction("TankActivity", "Report");
        }

        public ActionResult YardCheck()
        {
            var postModel = new YardCheckPostModel();
            if (TempData["YardCheckPostModel"] != null)
            {
                // database call
                postModel = (YardCheckPostModel)TempData["YardCheckPostModel"];
                var TANK_usp_rpt_YardCheck_spParams = new TANK_usp_rpt_YardCheck_spParams()
                {
                    //TODO: re-factor it later from hard coded
                    MajorLocationID = postModel.MajorLocationID,
                    SectionLocationID = postModel.SectionLocationID
                };
                DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_sel_GroundedMatrix", TANK_usp_rpt_YardCheck_spParams);
                DataSet dataSet = new DataSet("YardCheck");
                dataSet.Tables.Add(dataTable);

                #region Report

                var crDriverActivity = new ReportDocument();
                string reportPath = Path.Combine(Server.MapPath("~/Reports"), "crYardCheck.rpt");

                crDriverActivity.Load(reportPath);
                var dbServer = System.Web.Configuration.WebConfigurationManager.AppSettings["DBServer"];
                var dbName = System.Web.Configuration.WebConfigurationManager.AppSettings["DBName"];
                var dbUserId = System.Web.Configuration.WebConfigurationManager.AppSettings["DBUserId"];
                var dbPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["DBPassword"];
                crDriverActivity.SetDatabaseLogon(dbUserId, dbPassword, dbServer, dbName, true);
                crDriverActivity.SetDataSource(dataSet);
                //todo: these parameters ll be dynamic, but for proof of concept i kept them hard coded
                crDriverActivity.SetParameterValue("LocationName", postModel.LocationName);
                crDriverActivity.SetParameterValue("@MajorLocationID", postModel.MajorLocationID);
                crDriverActivity.SetParameterValue("@SectionLocationID", postModel.SectionLocationID);
                _contentBytes = StreamToBytes(crDriverActivity.ExportToStream(ExportFormatType.PortableDocFormat));

                #endregion

                return File(_contentBytes, "application/pdf");
            }
            else
            {
                #region PopulateTankStatus

                var LocationsList = new List<SelectListItem>();
                var Locations = _sharedFunctions.PopulateGroundedSections(1, false);
                if (Locations != null && Locations.Any())
                {
                    LocationsList.Add(new SelectListItem { Text = "", Value = "" });
                    foreach (var item in Locations)
                    {
                        LocationsList.Add(new SelectListItem { Text = item.LocationDS, Value = item.LocationID.ToString() });
                    }
                   
                }

                ViewBag.SectionLocationID = LocationsList;
                #endregion PopulateLoadStatusType
            }

            return View(postModel);
        }

        [HttpPost]
        public ActionResult YardCheck(YardCheckPostModel postModel)
        {
            TempData["YardCheckPostModel"] = postModel;
            return RedirectToAction("YardCheck", "Report");
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

    public class DailyDispatchPostModel
    {
        public DailyDispatchPostModel()
        {
            LocationID = 1;
            LocationName = "";
            StartDate = DateTime.Now.AddMonths(-1);
        }
        public DateTime StartDate { get; set; }
        public int LocationID { get; set; }
        public string LocationName { get; set; }
    }

    public class ChassisActivityPostModel
    {
        public ChassisActivityPostModel()
        {
            LocationID = 1;
            LocationName = "";
            StartDate = DateTime.Now.AddMonths(-1);
            EndDate = DateTime.Now;
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int LocationID { get; set; }
        public string LocationName { get; set; }
    }

    public class DriverActivityPostModel
    {
        public DriverActivityPostModel()
        {
            LocationID = 1;
            LocationName = "";
            StartDate = DateTime.Now.AddMonths(-1);
            EndDate = DateTime.Now;
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? DriverID { get; set; }
        public int LocationID { get; set; }
        public string LocationName { get; set; }
    }

    public class LastTankMovePostModel
    {
        public LastTankMovePostModel()
        {
            LocationID = 1;
            InstallID = 1;
        }
        public int LocationID { get; set; }
        public int InstallID { get; set; }
        public int PoolFL { get; set; }
    }

    public class TankShipmentSchedulePostModel
    {
        public TankShipmentSchedulePostModel()
        {
            LocationID = 1;
            LocationName = "";
            InstallID = 1;
            StartDate = DateTime.Now.AddMonths(-1);
            EndDate = DateTime.Now;
        }
        public int LocationID { get; set; }
        public int InstallID { get; set; }
        public int? DispatchedFL { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string EquipmentAN { get; set; }
        public string LocationName { get; set; }
    }

    public class TankActivityPostModel
    {
        public TankActivityPostModel()
        {
            LocationID = 1;
            LocationName = "";
            StartDate = DateTime.Now.AddMonths(-1);
            EndDate = DateTime.Now;
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public string EquipmentAN { get; set; }
        public int? OwnerID { get; set; }
        public int? OperatorID { get; set; }
        public int? PoolFL { get; set; }
    }

    public class YardCheckPostModel
    {
        public YardCheckPostModel()
        {
            MajorLocationID = 1;
            LocationName = "";
        }
        public int MajorLocationID { get; set; }
        public int? SectionLocationID { get; set; }
        public string LocationName { get; set; }
    }
}