using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DOWTank.Core.Domain.TANK_usp_sel;
using DOWTank.Core.Service;
using System.Web;
using System.Web.Mvc;

namespace DOWTank.Common
{
    public interface ISharedFunctions
    {
        IEnumerable<TANK_usp_sel_ChargeCodeDDL_spResults> PopulateChargeCode(int iIncludeBlank, string sFilter, int locationId);
        IEnumerable<TANK_usp_sel_LocationTreeFLAT_spResults> PopulateLoadPointLocationTreeFlatOverTheRoad(int majorLocationId);
        IEnumerable<TANK_usp_sel_LocationTreeFLAT_spResults> PopulateLoadPointLocationTreeFlatBlock(int majorLocationId);
        IEnumerable<TANK_usp_sel_LocationTreeGrounded_spResults> PopulateLoadPointLocationGrounded(int majorLocationId);
        IEnumerable<TANK_usp_sel_locationTreeALL_spResults> PopulateLoadPointLocationAll(int majorLocationId);
        IEnumerable<TANK_usp_sel_ProductDDL_spResults> PopulateProduct(bool iIncludeBlank, int locationId, string searchTerm);
        IEnumerable<TANK_usp_sel_FittingDDL_spResults> PopulateFitting(int locationId);
        IEnumerable<TANK_usp_sel_LoadStatusTypeDDL_spResults> PopulateLoadStatusType();
        IEnumerable<TANK_usp_sel_EquipmentSearch2_spResults> PopulateEquipment(Int16 EquipmentClassTypeCD,
                                                                               string EquipmentAn);
        IEnumerable<TANK_usp_sel_ContactDDL_spResults> PopulateContacts(int locationId);
        IEnumerable<TANK_usp_sel_SecurityDDL_spResults> PopulateSecurityDDL(int locationId);
        IEnumerable<TANK_usp_sel_Equipment_spResults> RefreshEquipment(int? equipmentId, string equipmentAN);
        bool SetCrystalDBSource(ReportDocument reportDocument);
        IEnumerable<TANK_usp_sel_DriverDDL_spResults> PopulateDrivers(bool iIncludeBlank);
        IEnumerable<TANK_usp_sel_WasteClassTypeDDL_spResults> PopulateWasteClassTypes(bool iIncludeBlank);
        IEnumerable<TANK_usp_sel_DispatchReasonDDL_spResults> PopulateDispatchReasons(bool iIncludeBlank);
        IEnumerable<TANK_usp_sel_DispatchLastMove_spResults> LoadDispatchLastMove(String strTankNumber, int locationId);
        IEnumerable<TANK_usp_sel_DispatchLastMove_spResults> LoadDispatchLastMove(int equipmentId, int locationId);
        IEnumerable<TANK_usp_sel_ServiceTypeDDL_spResults> PopulateServiceType(bool iIncludeBlank);
        void LoadExcel(DataTable dataTable);
        IEnumerable<TANK_usp_sel_OwnerDDL_spResults> PopulateOwnerDDL();
        IEnumerable<TANK_usp_sel_OperatorDDL_spResults> PopulateOperatorDDL();
        IEnumerable<TANK_usp_sel_EquipmentClassTypeDDL_spResults> PopulateEquipmentClassType();
        IEnumerable<TANK_usp_sel_EquipmentTypeDDL_spResults> PopulateEquipmentType(Int16? equipmentClassTypeCD);
        IEnumerable<TANK_usp_sel_TankGradeTypeDDL_spResults> PopulateTankGrade();
        IEnumerable<TANK_usp_sel_BarrelConditionTypeDDL_spResults> PopulateBarrelCondition();
        IEnumerable<TANK_usp_sel_MoveTypeDDL_spResults> PopulateMoveType();
        IEnumerable<TANK_usp_sel_LocationDDL_spParams_spResults> PopulateGroundedSections(int MajorLocationID, bool iIncludeBlank);
        int? ToNullableInt32(string s);
        List<TANK_usp_sel_SecurityProfilePrivileges_spResults> GetSecuritySettings(int? currentUserProfileId, int? privilegeCategoryId, int? securityProfileId);
        List<TANK_usp_sel_SecurityLocation_spResults> GetLocation(string userAn);
    }

    public class SharedFunctions : ISharedFunctions
    {
        private readonly IUtilityService _utilityService;

        public SharedFunctions(IUtilityService utilityService)
        {
            _utilityService = utilityService;
        }

        public List<TANK_usp_sel_SecurityLocation_spResults> GetLocation(string userAn)
        {

            var TANK_usp_sel_SecurityLocation_spParams = new TANK_usp_sel_SecurityLocation_spParams
                ()
            {
                UserAN = userAn
            };
            var listItems =
                _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_SecurityLocation_spResults>(
                    "TANK_usp_sel_SecurityLocation", TANK_usp_sel_SecurityLocation_spParams);

            return listItems.ToList();
        }


        public List<TANK_usp_sel_SecurityProfilePrivileges_spResults> GetSecuritySettings(int? currentUserProfileId, int? privilegeCategoryId, int? securityProfileId)
        {

            var TANK_usp_sel_SecurityProfilePrivileges_spParams = new TANK_usp_sel_SecurityProfilePrivileges_spParams
                ()
            {
                //todo: refactor it later
                CurrentUserProfileID = currentUserProfileId,
                PrivilegeCategoryID = privilegeCategoryId,
                SecurityProfileID = securityProfileId
            };
            var listItems =
                _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_SecurityProfilePrivileges_spResults>(
                    "TANK_usp_sel_SecurityProfilePrivileges", TANK_usp_sel_SecurityProfilePrivileges_spParams);

            return listItems.ToList();
        }

        public IEnumerable<TANK_usp_sel_ChargeCodeDDL_spResults> PopulateChargeCode(int iIncludeBlank, string sFilter, int locationId)
        {
            // database call

            var TANK_usp_sel_ChargeCodeDDL_spParams = new TANK_usp_sel_ChargeCodeDDL_spParams()
            {
                //TODO: re-factor it later from hard coded
                IncludeBlank = false,
                LocationID = locationId
            };
            var data = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_ChargeCodeDDL_spResults>("TANK_usp_sel_ChargeCodeDDL", TANK_usp_sel_ChargeCodeDDL_spParams);

            //# database call

            return data;
        }

        public IEnumerable<TANK_usp_sel_LocationTreeFLAT_spResults> PopulateLoadPointLocationTreeFlatOverTheRoad(int majorLocationId)
        {
            //Case "Over The Road" '-- Over the road
            //   SA.SelectCommand.CommandText = "dbo.TANK_usp_sel_LocationTreeFLAT"
            //   SA.SelectCommand.Parameters.Add("@LocationTypeCD", 4)
            // database call

            var TANK_usp_sel_locationTreeFLAT_spParams = new TANK_usp_sel_locationTreeFLAT_spParams()
            {
                //TODO: re-factor it later from hard coded
                LocationTypeCD = 4
            };
            var data = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_LocationTreeFLAT_spResults>("TANK_usp_sel_LocationTreeFLAT", TANK_usp_sel_locationTreeFLAT_spParams);

            //# database call

            return data;
        }

        public IEnumerable<TANK_usp_sel_LocationTreeFLAT_spResults> PopulateLoadPointLocationTreeFlatBlock(int majorLocationId)
        {

            var TANK_usp_sel_locationTreeFLAT_spParams = new TANK_usp_sel_locationTreeFLAT_spParams()
            {
                //TODO: re-factor it later from hard coded
                MajorLocationID = majorLocationId,
                LocationTypeCD = 3
            };
            var data = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_LocationTreeFLAT_spResults>("TANK_usp_sel_LocationTreeFLAT", TANK_usp_sel_locationTreeFLAT_spParams);

            //# database call

            return data;
        }

        public IEnumerable<TANK_usp_sel_LocationTreeGrounded_spResults> PopulateLoadPointLocationGrounded(int majorLocationId)
        {

            var TANK_usp_sel_LocationTreeGrounded_spParams = new TANK_usp_sel_LocationTreeGrounded_spParams()
            {
                //TODO: re-factor it later from hard coded
                MajorLocationID = majorLocationId
            };
            var data = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_LocationTreeGrounded_spResults>("TANK_usp_sel_LocationTreeGrounded", TANK_usp_sel_LocationTreeGrounded_spParams);

            //# database call

            return data;
        }

        public IEnumerable<TANK_usp_sel_locationTreeALL_spResults> PopulateLoadPointLocationAll(int majorLocationId)
        {

            var TANK_usp_sel_locationTreeALL_spParams = new TANK_usp_sel_locationTreeALL_spParams()
            {
                //TODO: re-factor it later from hard coded
                MajorLocationID = majorLocationId
            };
            var data = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_locationTreeALL_spResults>("TANK_usp_sel_locationTreeALL", TANK_usp_sel_locationTreeALL_spParams);

            //# database call

            return data;
        }

        public IEnumerable<TANK_usp_sel_ProductDDL_spResults> PopulateProduct(bool iIncludeBlank, int locationId, string searchTerm)
        {
            // database call

            var TANK_usp_sel_ProductDDL_spParams = new TANK_usp_sel_ProductDDL_spParams()
            {
                //TODO: re-factor it later from hard coded
                IncludeBlank = false,
                LocationID = locationId,
                Filter = searchTerm
            };
            var data = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_ProductDDL_spResults>("TANK_usp_sel_ProductDDL", TANK_usp_sel_ProductDDL_spParams);

            //# database call

            return data;
        }

        public IEnumerable<TANK_usp_sel_FittingDDL_spResults> PopulateFitting(int locationId)
        {
            // database call

            var TANK_usp_sel_FittingDDL_spParams = new TANK_usp_sel_FittingDDL_spParams()
            {
                //TODO: re-factor it later from hard coded
                IncludeBlank = false,
                LocationID = locationId,
            };
            var data = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_FittingDDL_spResults>("TANK_usp_sel_FittingDDL", TANK_usp_sel_FittingDDL_spParams);

            //# database call

            return data;
        }

        public IEnumerable<TANK_usp_sel_LoadStatusTypeDDL_spResults> PopulateLoadStatusType()
        {
            // database call

            var TANK_usp_sel_LoadStatusTypeDDL_spParams = new TANK_usp_sel_LoadStatusTypeDDL_spParams()
            {
                //TODO: re-factor it later from hard coded
                IncludeBlank = false
            };
            var data = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_LoadStatusTypeDDL_spResults>("TANK_usp_sel_LoadStatusTypeDDL", TANK_usp_sel_LoadStatusTypeDDL_spParams);

            //# database call

            return data;
        }

        public IEnumerable<TANK_usp_sel_EquipmentSearch2_spResults> PopulateEquipment(Int16 EquipmentClassTypeCD, string EquipmentAn)
        {
            // database call

            var TANK_usp_sel_EquipmentSearch2_spParams = new TANK_usp_sel_EquipmentSearch2_spParams()
            {
                //TODO: re-factor it later from hard coded
                EquipmentClassTypeCD = EquipmentClassTypeCD,
                EquipmentAn = EquipmentAn
            };
            var data = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_EquipmentSearch2_spResults>("TANK_usp_sel_EquipmentSearch2", TANK_usp_sel_EquipmentSearch2_spParams);

            //# database call

            return data;
        }

        public IEnumerable<TANK_usp_sel_ContactDDL_spResults> PopulateContacts(int locationId)
        {
            // database call

            var TANK_usp_sel_ContactDDL_spParams = new TANK_usp_sel_ContactDDL_spParams()
            {
                //TODO: re-factor it later from hard coded
                IncludeBlank = false,
                LocationID = locationId
            };
            var data = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_ContactDDL_spResults>("TANK_usp_sel_ContactDDL", TANK_usp_sel_ContactDDL_spParams);

            //# database call

            return data;
        }

        public IEnumerable<TANK_usp_sel_SecurityDDL_spResults> PopulateSecurityDDL(int locationId)
        {
            // database call

            var TANK_usp_sel_SecurityDDL_spParams = new TANK_usp_sel_SecurityDDL_spParams()
            {
                //TODO: re-factor it later from hard coded
                IncludeBlank = false,
                LocationID = locationId
            };
            var data = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_SecurityDDL_spResults>("TANK_usp_sel_SecurityDDL", TANK_usp_sel_SecurityDDL_spParams);

            //# database call

            return data;
        }

        public IEnumerable<TANK_usp_sel_OwnerDDL_spResults> PopulateOwnerDDL()
        {
            // database call

            var TANK_usp_sel_OwnerDDL_spParams = new TANK_usp_sel_OwnerDDL_spParams()
            {
                //TODO: re-factor it later from hard coded
                IncludeBlank = false
            };
            var data = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_OwnerDDL_spResults>("TANK_usp_sel_OwnerDDL", TANK_usp_sel_OwnerDDL_spParams);

            //# database call

            return data;
        }

        public IEnumerable<TANK_usp_sel_OperatorDDL_spResults> PopulateOperatorDDL()
        {
            // database call

            var TANK_usp_sel_OperatorDDL_spParams = new TANK_usp_sel_OperatorDDL_spParams()
            {
                //TODO: re-factor it later from hard coded
                IncludeBlank = false
            };
            var data = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_OperatorDDL_spResults>("TANK_usp_sel_OperatorDDL", TANK_usp_sel_OperatorDDL_spParams);

            //# database call

            return data;
        }

        public IEnumerable<TANK_usp_sel_EquipmentClassTypeDDL_spResults> PopulateEquipmentClassType()
        {
            // database call

            var TANK_usp_sel_EquipmentClassTypeDDL_spParams = new TANK_usp_sel_EquipmentClassTypeDDL_spParams()
            {
                //TODO: re-factor it later from hard coded
                IncludeBlank = false
            };
            var data = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_EquipmentClassTypeDDL_spResults>("TANK_usp_sel_EquipmentClassTypeDDL", TANK_usp_sel_EquipmentClassTypeDDL_spParams);

            //# database call

            return data;
        }

        public IEnumerable<TANK_usp_sel_TankGradeTypeDDL_spResults> PopulateTankGrade()
        {
            // database call

            var TANK_usp_sel_TankGradeTypeDDL_spParams = new TANK_usp_sel_TankGradeTypeDDL_spParams()
            {
                //TODO: re-factor it later from hard coded
                IncludeBlank = false
            };
            var data = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_TankGradeTypeDDL_spResults>("TANK_usp_sel_TankGradeTypeDDL", TANK_usp_sel_TankGradeTypeDDL_spParams);

            //# database call

            return data;
        }

        public IEnumerable<TANK_usp_sel_BarrelConditionTypeDDL_spResults> PopulateBarrelCondition()
        {
            // database call

            var TANK_usp_sel_BarrelConditionTypeDDL_spParams = new TANK_usp_sel_BarrelConditionTypeDDL_spParams()
            {
                //TODO: re-factor it later from hard coded
                IncludeBlank = false
            };
            var data = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_BarrelConditionTypeDDL_spResults>("TANK_usp_sel_BarrelConditionTypeDDL", TANK_usp_sel_BarrelConditionTypeDDL_spParams);

            //# database call

            return data;
        }
        public IEnumerable<TANK_usp_sel_MoveTypeDDL_spResults> PopulateMoveType()
        {
            // database call

            var TANK_usp_sel_MoveTypeDDL_spParams = new TANK_usp_sel_MoveTypeDDL_spParams()
            {
                //TODO: re-factor it later from hard coded
                IncludeBlank = false
            };
            var data = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_MoveTypeDDL_spResults>("TANK_usp_sel_MoveTypeDDL", TANK_usp_sel_MoveTypeDDL_spParams);

            //# database call

            return data;
        }

        public IEnumerable<TANK_usp_sel_EquipmentTypeDDL_spResults> PopulateEquipmentType(Int16? equipmentClassTypeCD)
        {
            // database call

            var TANK_usp_sel_EquipmentTypeDDL_spParams = new TANK_usp_sel_EquipmentTypeDDL_spParams()
            {
                //TODO: re-factor it later from hard coded
                IncludeBlank = false,
                EquipmentClassTypeCD = equipmentClassTypeCD
            };
            var data = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_EquipmentTypeDDL_spResults>("TANK_usp_sel_EquipmentTypeDDL", TANK_usp_sel_EquipmentTypeDDL_spParams);

            //# database call

            return data;
        }

        public IEnumerable<TANK_usp_sel_Equipment_spResults> RefreshEquipment(int? equipmentId, string equipmentAN)
        {
            // database call

            var TANK_usp_sel_Equipment_spParams = new TANK_usp_sel_Equipment_spParams()
            {
                //TODO: re-factor it later from hard coded
                EquipmentID = equipmentId,
                EquipmentAN = equipmentAN
            };
            var data = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_Equipment_spResults>("TANK_usp_sel_Equipment", TANK_usp_sel_Equipment_spParams);

            //# database call

            return data;
        }

        public bool SetCrystalDBSource(ReportDocument reportDocument)
        {
            var connectionInfo = new ConnectionInfo();
            SubreportObject subreportObject;
            connectionInfo.ServerName = System.Web.Configuration.WebConfigurationManager.AppSettings["DBServer"];
            connectionInfo.DatabaseName = System.Web.Configuration.WebConfigurationManager.AppSettings["DBName"];
            connectionInfo.UserID = System.Web.Configuration.WebConfigurationManager.AppSettings["DBUserId"];
            connectionInfo.Password = System.Web.Configuration.WebConfigurationManager.AppSettings["DBPassword"];

            if (!ApplyLogon(reportDocument, connectionInfo))
            {
                return false;
            }

            return true;
        }

        public bool ApplyLogon(ReportDocument reportDocument, ConnectionInfo connectionInfo)
        {
            TableLogOnInfo tableLogOnInfo;
            for (int tbl = 0; tbl < reportDocument.Database.Tables.Count - 1; tbl++)
            {
                tableLogOnInfo = reportDocument.Database.Tables[tbl].LogOnInfo;
                tableLogOnInfo.ConnectionInfo = connectionInfo;
                reportDocument.Database.Tables[tbl].ApplyLogOnInfo(tableLogOnInfo);
                //' check if logon was successful 
                //' if TestConnectivity returns false, 
                //' check logon credentials 

                if (reportDocument.Database.Tables[tbl].TestConnectivity())
                {
                    //drop fully qualified table location 
                    if (reportDocument.Database.Tables[tbl].Location.IndexOf(".") > 0)
                    {
                        reportDocument.Database.Tables[tbl].Location =
                            reportDocument.Database.Tables[tbl].Location.Substring(
                                reportDocument.Database.Tables[tbl].Location.LastIndexOf(".") + 1);
                    }
                    else
                    {
                        reportDocument.Database.Tables[tbl].Location = reportDocument.Database.Tables[tbl].Location;
                    }
                }
                else
                {
                    return false;
                }
                return true;
                //end of for
            }
            return true;
        }
        public IEnumerable<TANK_usp_sel_DriverDDL_spResults> PopulateDrivers(bool iIncludeBlank)
        {
            // database call

            var TANK_usp_sel_DriverDDL_spParams = new TANK_usp_sel_DriverDDL_spParams()
            {
                //TODO: re-factor it later from hard coded
                IncludeBlank = iIncludeBlank
            };
            var data = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_DriverDDL_spResults>("TANK_usp_sel_DriverDDL", TANK_usp_sel_DriverDDL_spParams);

            //# database call

            return data;
        }

        public IEnumerable<TANK_usp_sel_WasteClassTypeDDL_spResults> PopulateWasteClassTypes(bool iIncludeBlank)
        {
            // database call

            var TANK_usp_sel_WasteClassTypeDDL_spParams = new TANK_usp_sel_WasteClassTypeDDL_spParams()
            {
                //TODO: re-factor it later from hard coded
                IncludeBlank = iIncludeBlank
            };
            var data = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_WasteClassTypeDDL_spResults>("TANK_usp_sel_WasteClassTypeDDL", TANK_usp_sel_WasteClassTypeDDL_spParams);

            //# database call

            return data;
        }

        public IEnumerable<TANK_usp_sel_DispatchReasonDDL_spResults> PopulateDispatchReasons(bool iIncludeBlank)
        {
            // database call

            var TANK_usp_sel_DispatchReasonDDL_spParams = new TANK_usp_sel_DispatchReasonDDL_spParams()
            {
                //TODO: re-factor it later from hard coded
                IncludeBlank = iIncludeBlank
            };
            var data = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_DispatchReasonDDL_spResults>("TANK_usp_sel_DispatchReasonTypeDDL", TANK_usp_sel_DispatchReasonDDL_spParams);

            //# database call

            return data;
        }

        public IEnumerable<TANK_usp_sel_DispatchLastMove_spResults> LoadDispatchLastMove(String strTankNumber, int locationId)
        {
            // database call

            var TANK_usp_sel_DispatchLastMove_spParams = new TANK_usp_sel_DispatchLastMove_spParams()
            {
                //TODO: re-factor it later from hard coded
                EquipmentAN = strTankNumber,
                FacilityLocationID = locationId
            };
            var data = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_DispatchLastMove_spResults>("TANK_usp_sel_EquipmentLastDispatch", TANK_usp_sel_DispatchLastMove_spParams);

            //# database call

            return data;
        }
        public IEnumerable<TANK_usp_sel_DispatchLastMove_spResults> LoadDispatchLastMove(int equipmentId, int locationId)
        {
            // database call

            var TANK_usp_sel_DispatchLastMove_spParams = new TANK_usp_sel_DispatchLastMove_spParams()
            {
                //TODO: re-factor it later from hard coded
                EquipmentID = equipmentId,
                FacilityLocationID = locationId
            };
            var data = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_DispatchLastMove_spResults>("TANK_usp_sel_EquipmentLastDispatch", TANK_usp_sel_DispatchLastMove_spParams);

            //# database call

            return data;
        }

        public IEnumerable<TANK_usp_sel_ServiceTypeDDL_spResults> PopulateServiceType(bool iIncludeBlank)
        {
            // database call

            var TANK_usp_sel_DispatchReasonDDL_spParams = new TANK_usp_sel_DispatchReasonDDL_spParams()
            {
                //TODO: re-factor it later from hard coded
                IncludeBlank = iIncludeBlank
            };
            var data = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_ServiceTypeDDL_spResults>("TANK_usp_sel_ServiceTypeDDL", TANK_usp_sel_DispatchReasonDDL_spParams);

            //# database call

            return data;
        }

        public IEnumerable<TANK_usp_sel_LocationDDL_spParams_spResults> PopulateGroundedSections(int MajorLocationID, bool iIncludeBlank)
        {
            // database call

            var TANK_usp_sel_LocationDDL_spParams = new TANK_usp_sel_LocationDDL_spParams()
            {
                //TODO: re-factor it later from hard coded
                IncludeBlank = iIncludeBlank,
                MajorLocationID = MajorLocationID,
                LocationTypeCD = 7
            };
            var data = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_LocationDDL_spParams_spResults>("TANK_usp_sel_LocationDDL", TANK_usp_sel_LocationDDL_spParams);
            return data;
        }

        #region export to excel

        public void LoadExcel(DataTable dataTable)
        {
            string fileName = DateTime.Now.ToString("hh:mm:ss");
            // This actually makes your HTML output to be downloaded as .xls file
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ClearContent();
            System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".xls");

            // Create a dynamic control, populate and render it
            GridView excel = new GridView();
            if (dataTable.Rows.Count == 0)
            {
                DataRow row = dataTable.NewRow();
                dataTable.Rows.Add(row);
            }
            excel.DataSource = dataTable;
            excel.DataBind();
            excel.RenderControl(new HtmlTextWriter(System.Web.HttpContext.Current.Response.Output));

            System.Web.HttpContext.Current.Response.Flush();
            System.Web.HttpContext.Current.Response.End();
        }

        #endregion export to excel

        public int? ToNullableInt32(string s)
        {
            int i;
            if (Int32.TryParse(s, out i)) return i;
            return null;
        }
    }


}