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
    public class TankHistoryController : Controller
    {
        private readonly IUtilityService _utilityService;

        public TankHistoryController(IUtilityService utilityService)
        {
            _utilityService = utilityService;
        }

        // GET: /TankHistory/
        public ActionResult Index(string tankNumber)
        {
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

            int locationId = 1;
            int majorLocationID = 1;

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
            TankHistoryModel tankHistoryModel = new TankHistoryModel();

            int locationId = 1;
            int majorLocationID = 1;
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

            if (showColumnsOnly)
                data.Clear();

            @ViewBag.TotalOnHireHistory = data.Rows.Count;
            return data;
        }

        #endregion

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