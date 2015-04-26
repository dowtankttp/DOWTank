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
        //
        // GET: /Report/
        public ActionResult AuditDriverList()
        {
            // database call

            var TANK_usp_rpt_AuditDrivers_spParams = new TANK_usp_rpt_AuditDrivers_spParams()
            {
                //TODO: re-factor it later from hard coded
                LocationID = 1,
                StartDT = DateTime.Now.AddYears(-1),
                EndDT = DateTime.Now
            };
            DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_rpt_AuditDrivers", TANK_usp_rpt_AuditDrivers_spParams);

            DataSet dataSet = new DataSet("DailyDriverActivity");
            dataSet.Tables.Add(dataTable);

            //# database call

            #region Report

            var crDailyDriverActivity = new ReportDocument();
            string reportPath = Path.Combine(Server.MapPath("~/Reports"), "crDailyDriverActivity.rpt");
            crDailyDriverActivity.Load(reportPath);
            var dbServer = System.Web.Configuration.WebConfigurationManager.AppSettings["DBServer"];
            var dbName = System.Web.Configuration.WebConfigurationManager.AppSettings["DBName"];
            var dbUserId = System.Web.Configuration.WebConfigurationManager.AppSettings["DBUserId"];
            var dbPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["DBPassword"];
            crDailyDriverActivity.SetDatabaseLogon(dbUserId, dbPassword, dbServer, dbName, true);
            crDailyDriverActivity.SetDataSource(dataSet);
            //todo: these parameters ll be dynamic, but for proof of concept i kept them hard coded
            crDailyDriverActivity.SetParameterValue("@StartDT", DateTime.Now.AddYears(-1));
            crDailyDriverActivity.SetParameterValue("@EndDT", DateTime.Now);
            crDailyDriverActivity.SetParameterValue("@LocationId", 1);
            crDailyDriverActivity.SetParameterValue("LocationName", "");
            _contentBytes = StreamToBytes(crDailyDriverActivity.ExportToStream(ExportFormatType.PortableDocFormat));

            #endregion

            return File(_contentBytes, "application/pdf");


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

}