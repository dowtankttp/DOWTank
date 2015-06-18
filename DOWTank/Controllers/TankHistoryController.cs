using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOWTank.Common;
using DOWTank.Core.Domain.TANK_usp_insupd;
using DOWTank.Core.Domain.TANK_usp_rpt;
using DOWTank.Core.Domain.TANK_usp_sel;
using DOWTank.Core.Domain.TANK_usp_upd;
using DOWTank.Core.Enum;
using DOWTank.Core.Service;
using DOWTank.Custom;
using DOWTank.Models;
using DOWTank.Utility;

namespace DOWTank.Controllers
{
    [ClaimsAuthorize(Roles = "Tank History")]
    public class TankHistoryController : BaseController
    {
        private readonly IUtilityService _utilityService;
        private readonly ISharedFunctions _sharedFunctions;

        public TankHistoryController(IUtilityService utilityService, ISharedFunctions sharedFunctions)
        {
            _utilityService = utilityService;
            _sharedFunctions = sharedFunctions;
        }

        // GET: /TankHistory/
        public ActionResult Index(string tankNumber)
        {
            PopulateSecurityExtended();
            int securityProfileId = SecurityExtended.SecurityProfileId;
            var permissionList = _sharedFunctions.GetSecuritySettings(securityProfileId, (int)SecurityCatEnum.TankHistory, null);
            ViewBag.AllowDispatch = true;
            ViewBag.AllowPrep = true;
            ViewBag.AllowEditDispatch = true;
            ViewBag.AllowViewOnHireHistory = true;
            ViewBag.AllowEditOnHireHistory = true;
            ViewBag.AllowEditTankInfo = true;
            //foreach (var permission in permissionList)
            //{
            //    if (permission.PrivilegeDS == "Dispatch")
            //    {
            //        ViewBag.AllowDispatch = (permission.GrantedFL == 1);
            //    }
            //    else if (permission.PrivilegeDS == "Prep")
            //    {
            //        ViewBag.AllowPrep = (permission.GrantedFL == 1);
            //    }
            //    else if (permission.PrivilegeDS == "Edit Dispatch")
            //    {
            //        ViewBag.AllowEditDispatch = (permission.GrantedFL == 1);
            //    }
            //    else if (permission.PrivilegeDS == "View On Hire History")
            //    {
            //        ViewBag.AllowViewOnHireHistory = (permission.GrantedFL == 1);
            //    }
            //    else if (permission.PrivilegeDS == "Edit On Hire History")
            //    {
            //        ViewBag.AllowEditOnHireHistory = (permission.GrantedFL == 1);
            //    }
            //    else if (permission.PrivilegeDS == "Edit Tank Info")
            //    {
            //        ViewBag.AllowEditTankInfo = (permission.GrantedFL == 1);
            //    }
            //}

            TankHistoryModel tankHistoryModel = new TankHistoryModel();

            string equipment = string.Empty;
            if (!string.IsNullOrEmpty(tankNumber))
            {
                equipment = tankNumber.TrimEnd();
            }
            if (TempData["Equipment"] != null)
            {
                equipment = TempData["Equipment"].ToString();
            }

            int locationId = SecurityExtended.LocationId.Value;
            int majorLocationID = SecurityExtended.LocationId.Value;

            tankHistoryModel.DataTableActivityHistory = GetTankActivityHistory(false, locationId, equipment, "");
            tankHistoryModel.DataTableActivityHistory.Columns["DispatchID"].SetOrdinal(15);
            tankHistoryModel.DataTableEquipmentInfo = GetEquipmentInfo(false, majorLocationID, equipment);
            tankHistoryModel.DataTableEquipmentInfo.Columns["EquipmentID"].SetOrdinal(44);
            tankHistoryModel.DataTableEquipmentProduct = GetEquipmentProduct(false, equipment);
            tankHistoryModel.DataTableOnHireHistory = GetOnHireHistory(false, locationId, equipment);

            tankHistoryModel.HfShowResult = !string.IsNullOrEmpty(equipment) ? "true" : "false";
            tankHistoryModel.TextSearch = equipment;
            return View(tankHistoryModel);
        }

        public ActionResult Search(TankHistoryModel postModel)
        {
            if (string.IsNullOrEmpty(postModel.TextSearch))
                return RedirectToAction("Index");

            TankHistoryModel tankHistoryModel = new TankHistoryModel();

            string equipment = postModel.TextSearch;

            TempData["Equipment"] = equipment;

            return RedirectToAction("Index", "TankHistory");
        }

        public ActionResult Filter(TankHistoryModel postModel)
        {
            PopulateSecurityExtended();
            int securityProfileId = SecurityExtended.SecurityProfileId;
            var permissionList = _sharedFunctions.GetSecuritySettings(securityProfileId, (int)SecurityCatEnum.TankHistory, null);
            ViewBag.AllowDispatch = true;
            ViewBag.AllowPrep = true;
            ViewBag.AllowEditDispatch = true;
            ViewBag.AllowViewOnHireHistory = true;
            ViewBag.AllowEditOnHireHistory = true;
            ViewBag.AllowEditTankInfo = true;
            //foreach (var permission in permissionList)
            //{
            //    if (permission.PrivilegeDS == "Dispatch")
            //    {
            //        ViewBag.AllowDispatch = (permission.GrantedFL == 1);
            //    }
            //    else if (permission.PrivilegeDS == "Prep")
            //    {
            //        ViewBag.AllowPrep = (permission.GrantedFL == 1);
            //    }
            //    else if (permission.PrivilegeDS == "Edit Dispatch")
            //    {
            //        ViewBag.AllowEditDispatch = (permission.GrantedFL == 1);
            //    }
            //    else if (permission.PrivilegeDS == "View On Hire History")
            //    {
            //        ViewBag.AllowViewOnHireHistory = (permission.GrantedFL == 1);
            //    }
            //    else if (permission.PrivilegeDS == "Edit On Hire History")
            //    {
            //        ViewBag.AllowEditOnHireHistory = (permission.GrantedFL == 1);
            //    }
            //    else if (permission.PrivilegeDS == "Edit Tank Info")
            //    {
            //        ViewBag.AllowEditTankInfo = (permission.GrantedFL == 1);
            //    }
            //}


            TankHistoryModel tankHistoryModel = new TankHistoryModel();

            int locationId = SecurityExtended.LocationId.Value;
            int majorLocationID = SecurityExtended.LocationId.Value;
            string equipment = TempData["Equipment"].ToString();
            string filter = postModel.TextFilter;
            string filterColumn = postModel.SelectedColumn;
            string filterExpression = "";

            if (!string.IsNullOrEmpty(filter.Trim()))
            {
                filterExpression = filterColumn + " like '" + filter + "%'";
            }

            tankHistoryModel.DataTableActivityHistory = GetTankActivityHistory(false, locationId, equipment, filterExpression);
            tankHistoryModel.DataTableEquipmentInfo = GetEquipmentInfo(false, majorLocationID, equipment);
            tankHistoryModel.DataTableEquipmentProduct = GetEquipmentProduct(false, equipment);
            tankHistoryModel.DataTableOnHireHistory = GetOnHireHistory(false, locationId, equipment);

            tankHistoryModel.HfShowResult = "true";
            tankHistoryModel.TextSearch = equipment;
            return View("Index", tankHistoryModel); ;
        }

        #region Private Methods

        private DataTable GetTankActivityHistory(bool showColumnsOnly, int locationId, string equipment, string filterExpression)
        {
            // database call                        
            var TANK_usp_rpt_TankActivityHistory_spParams = new TANK_usp_rpt_TankActivityHistory_spParams();
            TANK_usp_rpt_TankActivityHistory_spParams.LocationID = locationId;
            TANK_usp_rpt_TankActivityHistory_spParams.EquipmentAN = equipment;

            DataTable data = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_rpt_TankActivityHistory", TANK_usp_rpt_TankActivityHistory_spParams);

            if (!string.IsNullOrEmpty(filterExpression.Trim()))
            {
                DataRow[] dr = data.Select(filterExpression);
                if (dr.Length > 0)
                    data = dr.CopyToDataTable();
            }

            if (showColumnsOnly)
                data.Clear();

            @ViewBag.TotalRecordsActivityHistory = data.Rows.Count;

            return data;
        }

        private DataTable GetEquipmentInfo(bool showColumnsOnly, int majorLocationID, string equipment)
        {
            // database call                        
            var TANK_usp_sel_EquipmentInfo_spParams = new TANK_usp_sel_EquipmentInfo_spParams();
            //TANK_usp_sel_EquipmentInfo_spParams.EquipmentID = equipmentId;
            TANK_usp_sel_EquipmentInfo_spParams.EquipmentAN = equipment;
            TANK_usp_sel_EquipmentInfo_spParams.MajorLocationID = majorLocationID;

            DataTable data = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_sel_EquipmentInfo", TANK_usp_sel_EquipmentInfo_spParams);

            if (showColumnsOnly)
                data.Clear();

            @ViewBag.TotalRecordsEquipmentInfo = data.Rows.Count;
            return data;
        }

        private DataTable GetEquipmentProduct(bool showColumnsOnly, string equipment)
        {
            // database call                        
            var TANK_usp_sel_EquipmentProduct_spParams = new TANK_usp_sel_EquipmentProduct_spParams();
            //            TANK_usp_sel_EquipmentProduct_spParams.EquipmentID = equipmentId;            
            TANK_usp_sel_EquipmentProduct_spParams.EquipmentAN = equipment;

            DataTable data = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_sel_EquipmentProduct", TANK_usp_sel_EquipmentProduct_spParams);

            if (showColumnsOnly)
                data.Clear();

            @ViewBag.TotalEquipmentProduct = data.Rows.Count;
            return data;
        }

        private DataTable GetOnHireHistory(bool showColumnsOnly, int locationId, string equipment)
        {
            // database call                        
            var TANK_usp_sel_EquipmentOnHireHistory_spParams = new TANK_usp_sel_EquipmentOnHireHistory_spParams();
            TANK_usp_sel_EquipmentOnHireHistory_spParams.LocationID = locationId;
            TANK_usp_sel_EquipmentOnHireHistory_spParams.EquipmentAn = equipment;

            DataTable data = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_sel_EquipmentOnHireHistory", TANK_usp_sel_EquipmentOnHireHistory_spParams);
            data.Columns["OnHireHistoryID"].SetOrdinal(13);
            if (showColumnsOnly)
                data.Clear();

            @ViewBag.TotalOnHireHistory = data.Rows.Count;
            return data;
        }

        #endregion

        #region Add History

        [HttpGet]
        public ActionResult SaveHistory(int? onHireHistoryID)
        {
            LoadAddHistoryDropdowns();
            PopulateSecurityExtended();

            // database call                        
            var TANK_usp_sel_EquipmentOnHireHistory_spParams = new TANK_usp_sel_EquipmentOnHireHistory_spParams();
            TANK_usp_sel_EquipmentOnHireHistory_spParams.OnHireHistoryID = onHireHistoryID;

            var data = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_EquipmentOnHireHistory_spResults>("TANK_usp_sel_EquipmentOnHireHistory", TANK_usp_sel_EquipmentOnHireHistory_spParams).ToList();
            //database call

            var viewModel = new EquipmentOnHireHistoryPostModel();
            if (data.Any())
            {
                var tankHistory = data.FirstOrDefault();
                viewModel.OnHireHistoryID = tankHistory.OnHireHistoryID;
                viewModel.EquipmentAN = tankHistory.EquipmentAN;
                viewModel.ShipmentAN = tankHistory.ShipmentAn;
                viewModel.StatusDt = Convert.ToDateTime(tankHistory.StatusDt).Add(TimeSpan.Parse(tankHistory.StatusTime));
                viewModel.OnHireFL = tankHistory.OnHireFl;
                viewModel.OnHireReasonTypeCDEdit = tankHistory.OnHireReasonTypeCD;
                viewModel.ChargeCodeAn = tankHistory.ChargeCodeAn;
                viewModel.ChargeCodeID = tankHistory.ChargeCodeID;
            }
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SaveHistory(EquipmentOnHireHistoryPostModel model)
        {
            PopulateSecurityExtended();
            LoadAddHistoryDropdowns();
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //database call

            var TANK_usp_insupd_EquipmentOnHireHistory_spParams = new TANK_usp_insupd_EquipmentOnHireHistory_spParams();

            #region add

            if (!model.OnHireHistoryID.HasValue || (model.OnHireHistoryID.Value == 0))
            {
                TANK_usp_insupd_EquipmentOnHireHistory_spParams = new TANK_usp_insupd_EquipmentOnHireHistory_spParams()
                    {
                        LocationID = SecurityExtended.LocationId.Value,
                        OnHireHistoryID = model.OnHireHistoryID,
                        EquipmentAN = model.EquipmentAN.Left(10),
                        ChargeCodeAn = model.ChargeCodeAn,
                        ShipmentAN = model.ShipmentAN,
                        OnHireFL = model.OnHireFL,
                        OnHireReasonTypeCD = model.OnHireReasonTypeCD,
                        StatusDt = model.StatusDt.Value,
                        UpdateUserAn = SecurityExtended.UserName
                    };
            }

            #endregion add

            #region Edit

            if (model.OnHireHistoryID.HasValue && (model.OnHireHistoryID.Value > 0))
            {
                TANK_usp_insupd_EquipmentOnHireHistory_spParams = new TANK_usp_insupd_EquipmentOnHireHistory_spParams()
                {
                    LocationID = SecurityExtended.LocationId.Value,
                    OnHireHistoryID = model.OnHireHistoryID,
                    EquipmentAN = model.EquipmentAN.Left(10),
                    ChargeCodeAn = model.ChargeCodeAn,
                    ShipmentAN = model.ShipmentAN,
                    OnHireFL = model.OnHireFL,
                    OnHireReasonTypeCD = model.OnHireReasonTypeCD,
                    StatusDt = model.StatusDt.Value,
                    UpdateUserAn = SecurityExtended.UserName
                };
            }

            #endregion edit

            _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_EquipmentOnHireHistory", TANK_usp_insupd_EquipmentOnHireHistory_spParams);
            //database call

            Success("Changes Saved Successfully.");

            return RedirectToAction("Index");
        }

        private void LoadAddHistoryDropdowns()
        {
            var list = new List<SelectListItem>();

            //database call
            var TANK_usp_sel_OnHireReasonTypeDDL_spParams = new TANK_usp_sel_OnHireReasonTypeDDL_spParams()
                {
                    IncludeBlank = false
                };

            var response = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_OnHireReasonTypeDDL_spResults>("TANK_usp_sel_OnHireReasonTypeDDL", TANK_usp_sel_OnHireReasonTypeDDL_spParams);

            //database call
            if (response != null && response.Any())
            {
                list.Add(new SelectListItem { Text = "", Value = "" });
                foreach (var item in response)
                {
                    list.Add(new SelectListItem { Text = item.OnHireReasonTypeDS, Value = item.OnHireReasonTypeCD.HasValue ? item.OnHireReasonTypeCD.Value.ToString() : string.Empty });
                }
                ViewBag.OnHireReasonTypeCD = list;
            }

        }

        #endregion Add History

        #region Delete History

        [HttpPost]
        public JsonResult DeleteHistory(int onHireHistoryID)
        {
            PopulateSecurityExtended();
            var TANK_usp_del_OnHireHistory_spParams = new TANK_usp_del_OnHireHistory_spParams()
                {
                    OnHireHistoryID = onHireHistoryID
                };

            _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_del_OnHireHistory", TANK_usp_del_OnHireHistory_spParams);

            return Json(1);
        }

        #endregion Delete History

    }

    public class TankHistoryModel
    {
        public DataTable DataTableActivityHistory { get; set; }
        public DataTable DataTableEquipmentInfo { get; set; }
        public DataTable DataTableEquipmentProduct { get; set; }
        public DataTable DataTableOnHireHistory { get; set; }
        public string TextSearch { get; set; }
        public string TextFilter { get; set; }
        public string SelectedColumn { get; set; }
        public string HfShowResult { get; set; }

    }

}