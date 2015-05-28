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
        public JsonResult GetContacts(int page, int rows, string search, string sidx, string sord)
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
                            Id = p.Field<int>("Key"),
                            LastName = p.Field<string>("Last Name*"),
                            FirstName = p.Field<string>("First Name*"),
                            Phone = p.Field<string>("Phone"),
                            Ext = p.Field<string>("Ext"),
                        }).ToList();

            //# database call

            int totalRecords = data.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
            data = data.Skip(page * rows).Take(rows).ToList();

            var jsonData = new
            {

                total = totalPages,
                page,
                records = totalRecords,
                rows = data

            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ManageContacts(ContactPostModel postModel)
        {
            
            switch (Request.Form["oper"])
            {
                case "add":
                    {
                        #region add

                        var TANK_usp_insupd_Contact_spParams = new TANK_usp_insupd_Contact_spParams()
                            {
                                Key = postModel.Id,
                                Ext = postModel.Ext,
                                FirstName = postModel.FirstName,
                                LastName = postModel.LastName,
                                Phone = postModel.Phone,
                                UpdateUserAN = "SYSTEM",
                                ActiveFL = true
                            };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_Contact",
                                                                          TANK_usp_insupd_Contact_spParams);

                        #endregion add
                    }
                    break;
                case "edit":
                    {
                        #region edit

                        var TANK_usp_insupd_Contact_spParams = new TANK_usp_insupd_Contact_spParams()
                            {
                                Key = _sharedFunctions.ToNullableInt32(Request.Form["id"]),
                                Ext = postModel.Ext,
                                FirstName = postModel.FirstName,
                                LastName = postModel.LastName,
                                Phone = postModel.Phone,
                                UpdateUserAN = "SYSTEM",
                                ActiveFL = true
                            };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_Contact",
                                                                          TANK_usp_insupd_Contact_spParams);

                        #endregion edit
                    }
                    break;
                case "del":
                    {
                        #region delete

                        var TANK_usp_insupd_Contact_spParams = new TANK_usp_insupd_Contact_spParams()
                            {
                                Key = postModel.Id,
                                ActiveFL = false,
                                UpdateUserAN = "SYSTEM"
                            };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_Contact",
                                                                          TANK_usp_insupd_Contact_spParams);

                        #endregion delete
                    }
                    break;
                default:
                    break;
            }

            return Json(true);
        }


        #endregion Contacts
    }

    #region Contacts Model

    public class ContactPostModel
    {
        public int? Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Phone { get; set; }
        public string Ext { get; set; }
    }


    #endregion Contacts Model
}