using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOWTank.Common;
using DOWTank.Core.Domain.TANK_usp_rpt;
using DOWTank.Core.Enum;
using DOWTank.Core.Service;
using DOWTank.Custom;

namespace DOWTank.Controllers
{
    [ClaimsAuthorize(Roles = "Invoices")]
    public class InvoiceListController : BaseController
    {
        private readonly IUtilityService _utilityService;
        private readonly ISharedFunctions _sharedFunctions;

        public InvoiceListController(IUtilityService utilityService, ISharedFunctions sharedFunctions)
        {
            _utilityService = utilityService;
            _sharedFunctions = sharedFunctions;
        }
        
        // GET: /InvoiceList/
        public ActionResult Index()
        {
            PopulateSecurityExtended();
            int securityProfileId = SecurityExtended.SecurityProfileId;
            var permissionList = _sharedFunctions.GetSecuritySettings(securityProfileId, (int)SecurityCatEnum.RequireService, null);
           
            InvoiceListModel invoiceListModel = new InvoiceListModel();

            invoiceListModel.DataTableMoveHistory = new DataTable();
            invoiceListModel.DataTableEquipmentInfo = new DataTable();
            invoiceListModel.DataTableEquipmentProduct =  new DataTable();
            invoiceListModel.DataTableInvoiceHistory =  new DataTable();
            

            invoiceListModel.HfShowResult = "false";
            return View(invoiceListModel);
        }

        public ActionResult Search(InvoiceListModel postModel)
        {
            PopulateSecurityExtended();
            if (string.IsNullOrEmpty(postModel.TextSearch))
                return RedirectToAction("Index");

            InvoiceListModel invoiceListModel = new InvoiceListModel();           

            int locationId = SecurityExtended.LocationId.Value;            
            string equipment = postModel.TextSearch;

            invoiceListModel.DataTableMoveHistory = GetTankMoveHistory(locationId, equipment, "");
            invoiceListModel.DataTableEquipmentInfo = GetEquipmentInfo( equipment);
            invoiceListModel.DataTableEquipmentProduct = GetEquipmentProduct(equipment);
            invoiceListModel.DataTableInvoiceHistory = GetInvoiceHistory( equipment);
            TempData["Equipment"] = equipment;
            invoiceListModel.HfShowResult = "true";
            return View("Index", invoiceListModel);
        }

        public ActionResult Filter(InvoiceListModel postModel)
        {
            PopulateSecurityExtended();
            int securityProfileId = SecurityExtended.SecurityProfileId;
            var permissionList = _sharedFunctions.GetSecuritySettings(securityProfileId, (int)SecurityCatEnum.RequireService, null);
            
            if (TempData["Equipment"] == null)
                return null;

            InvoiceListModel invoiceListModel = new InvoiceListModel();

            int locationId = SecurityExtended.LocationId.Value;            
            string equipment = TempData["Equipment"].ToString();
            string filter = postModel.TextFilter;
            string filterColumn = postModel.SelectedColumn;
            string filterExpression = "";

            if (!string.IsNullOrEmpty(filter.Trim()))
            {
                filterExpression = filterColumn + " like '" + filter + "%'";
            }

            invoiceListModel.DataTableMoveHistory = GetTankMoveHistory(locationId, equipment, filterExpression);
            invoiceListModel.DataTableEquipmentInfo = GetEquipmentInfo( equipment);
            invoiceListModel.DataTableEquipmentProduct = GetEquipmentProduct( equipment);
            invoiceListModel.DataTableInvoiceHistory = GetInvoiceHistory( equipment);

            invoiceListModel.HfShowResult = "true";
            invoiceListModel.TextSearch = equipment;
            return View("Index", invoiceListModel); ;
        }

        #region Private Methods

        private DataTable GetTankMoveHistory( int locationId, string equipment, string filterExpression)
        {
            // database call                        
            var TANK_usp_rpt_TankMaintMoveHistory_spParams = new TANK_usp_rpt_TankMaintMoveHistory_spParams();
            TANK_usp_rpt_TankMaintMoveHistory_spParams.LocationID = locationId;
            TANK_usp_rpt_TankMaintMoveHistory_spParams.EquipmentAN = equipment;

            DataTable data = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_rpt_TankMaintMoveHistory", TANK_usp_rpt_TankMaintMoveHistory_spParams);

            if (!string.IsNullOrEmpty(filterExpression.Trim()))
            {
                DataRow[] dr = data.Select(filterExpression);
                if (dr.Length > 0)
                    data = dr.CopyToDataTable();
            }

            @ViewBag.TotalRecordsMoveHistory = data.Rows.Count;

            return data;
        }

        private DataTable GetEquipmentInfo( string equipment)
        {
            // database call                        
            var TANK_usp_sel_EquipmentInfo_spParams = new TANK_usp_sel_EquipmentInfo_spParams();            
            TANK_usp_sel_EquipmentInfo_spParams.EquipmentAN = equipment;                     

            DataTable data = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_sel_EquipmentInfo", TANK_usp_sel_EquipmentInfo_spParams);

            @ViewBag.TotalRecordsEquipmentInfo = data.Rows.Count;
            return data;
        }

        private DataTable GetEquipmentProduct(string equipment)
        {
            // database call                        
            var TANK_usp_sel_EquipmentProduct_spParams = new TANK_usp_sel_EquipmentProduct_spParams();            
            TANK_usp_sel_EquipmentProduct_spParams.EquipmentAN = equipment;

            DataTable data = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_sel_EquipmentProduct", TANK_usp_sel_EquipmentProduct_spParams);

            @ViewBag.TotalEquipmentProduct = data.Rows.Count;
            return data;
        }

        private DataTable GetInvoiceHistory(string equipment)
        {
            // database call                        
            var TANK_usp_sel_InvoiceList_spParams = new TANK_usp_sel_InvoiceList_spParams();
            TANK_usp_sel_InvoiceList_spParams.EquipmentAN = equipment;

            DataTable data = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_sel_InvoiceList", TANK_usp_sel_InvoiceList_spParams);

            @ViewBag.TotalInvoiceHistory = data.Rows.Count;
            return data;
        }

        #endregion

	}

    public class InvoiceListModel
    {
        public DataTable DataTableMoveHistory { get; set; }
        public DataTable DataTableEquipmentInfo { get; set; }
        public DataTable DataTableEquipmentProduct { get; set; }
        public DataTable DataTableInvoiceHistory { get; set; }
        public string TextSearch { get; set; }
        public string TextFilter { get; set; }
        public string SelectedColumn { get; set; }
        public string HfShowResult { get; set; }
    }
}