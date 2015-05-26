using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOWTank.Common;
using DOWTank.Core.Domain.TANK_usp_insupd;
using DOWTank.Core.Domain.TANK_usp_sel;
using DOWTank.Core.Service;

namespace DOWTank.Controllers
{
    public class AdminController : Controller
    {

        private readonly IUtilityService _utilityService;
        private readonly ISharedFunctions _sharedFunctions;

        public AdminController(IUtilityService utilityService, ISharedFunctions sharedFunctions)
        {
            _utilityService = utilityService;
            _sharedFunctions = sharedFunctions;
        }

        #region Contacts

        [HttpGet]
        public ActionResult Contacts()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ManageContacts()
        {
            // database call

            var TANK_usp_sel_ContactUPD_spParams = new TANK_usp_sel_ContactUPD_spParams()
            {
                //TODO: re-factor it later from hard coded
                InstallID = 1,
                MajorLocationID = 1
            };
            DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_sel_ContactUPD", TANK_usp_sel_ContactUPD_spParams);

            var data = (from p in dataTable.AsEnumerable()
                        select new
                        {
                            DT_RowId = p.Field<int>("Key"),
                            Id = p.Field<int>("Key"),
                            LastName = p.Field<string>("Last Name*"),
                            FirstName = p.Field<string>("First Name*"),
                            Phone = p.Field<string>("Phone"),
                            Ext = p.Field<string>("Ext"),
                        }).ToList();

            //# database call
            //return Json(new { data = new[] { data } }, JsonRequestBehavior.AllowGet);
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ManageContacts(FormCollection formCollection)
        {
            var action = formCollection["action"];

            #region edit

            if (action == "edit")
            {
                var TANK_usp_insupd_Contact_spParams = new TANK_usp_insupd_Contact_spParams()
                    {
                        Key = _sharedFunctions.ToNullableInt32(formCollection["data[Id]"]),
                        Ext = formCollection["data[Ext]"],
                        FirstName = formCollection["data[FirstName]"],
                        LastName = formCollection["data[LastName]"],
                        Phone = formCollection["data[Phone]"],
                        UpdateUserAN = "SYSTEM"
                    };
                _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_Contact", TANK_usp_insupd_Contact_spParams);
            }

            #endregion edit

            #region delete

            if (action == "remove")
            {
                var TANK_usp_insupd_Contact_spParams = new TANK_usp_insupd_Contact_spParams()
                {
                    Key = _sharedFunctions.ToNullableInt32(formCollection["id[]"]),
                    ActiveFL = false,
                    UpdateUserAN = "SYSTEM"
                };
                _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_Contact", TANK_usp_insupd_Contact_spParams);
            }

            #endregion delete

            #region data

            // database call

            var TANK_usp_sel_ContactUPD_spParams = new TANK_usp_sel_ContactUPD_spParams()
            {
                //TODO: re-factor it later from hard coded
                InstallID = 1,
                MajorLocationID = 1
            };
            DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_sel_ContactUPD", TANK_usp_sel_ContactUPD_spParams);

            var data = (from p in dataTable.AsEnumerable()
                        select new
                        {
                            Id = p.Field<int>("Key"),
                            LastName = p.Field<string>("Last Name*"),
                            FirstName = p.Field<string>("First Name*"),
                            Phone = p.Field<string>("Phone"),
                            Ext = p.Field<string>("Ext"),
                        }).ToList();

            //# database call
            //return Json(new { data = new[] { data } }, JsonRequestBehavior.AllowGet);
            return Json(new { data = data });

            #endregion data
        }


        #endregion Contacts
    }

    #region Contacts Model

    public class ContactPostModel
    {

    }


    #endregion Contacts Model
}