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


        #endregion

        #region Charge Codes

        [HttpGet]
        public ActionResult ChargeCodes()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetChargeCodes(int page, int rows, string search, string sidx, string sord)
        {
            // database call

            var TANK_usp_sel_ChargeCodesUpd_spParams = new TANK_usp_sel_ChargeCodesUpd_spParams()
            {
                //TODO: re-factor it later from hard coded
                InstallID = 1,
                MajorLocationID = 1
            };
            DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_sel_ChargeCodeUPD", TANK_usp_sel_ChargeCodesUpd_spParams);

            var data = (from p in dataTable.AsEnumerable()
                        select new
                        {
                            Id = p.Field<int>("Key"),
                            Code = p.Field<string>("Code*"),
                            Description = p.Field<string>("Description"),
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
        public JsonResult ManageChargeCodes(ChargeCodesPostModel postModel)
        {

            switch (Request.Form["oper"])
            {
                case "add":
                    {
                        #region add

                        var TANK_usp_insupd_ChargeCode_spParams = new TANK_usp_insupd_ChargeCode_spParams()
                        {
                            Key = postModel.Id,
                            Code = postModel.Code,
                            Description = postModel.Description,                            
                            UpdateUserAN = "SYSTEM",
                            MajorLocationID = 1,//todo change location id
                            ActiveFL = true
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_ChargeCode",
                                                                          TANK_usp_insupd_ChargeCode_spParams);

                        #endregion add
                    }
                    break;
                case "edit":
                    {
                        #region edit

                        var TANK_usp_insupd_ChargeCode_spParams = new TANK_usp_insupd_ChargeCode_spParams()
                        {
                            Key = _sharedFunctions.ToNullableInt32(Request.Form["id"]),
                            Code = postModel.Code,
                            Description = postModel.Description,                            
                            UpdateUserAN = "SYSTEM",
                            MajorLocationID = 1,//todo change location id
                            ActiveFL = true
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_ChargeCode",
                                                                          TANK_usp_insupd_ChargeCode_spParams);

                        #endregion edit
                    }
                    break;
                case "del":
                    {
                        #region delete

                        var TANK_usp_insupd_ChargeCode_spParams = new TANK_usp_insupd_ChargeCode_spParams()
                        {
                            Key = postModel.Id,
                            ActiveFL = false,
                            UpdateUserAN = "SYSTEM"
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_ChargeCode",
                                                                          TANK_usp_insupd_ChargeCode_spParams);

                        #endregion delete
                    }
                    break;
                default:
                    break;
            }

            return Json(true);
        }


        #endregion 

        #region DispatchReasonType

        [HttpGet]
        public ActionResult DispatchReasonType()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetDispatchReasonType(int page, int rows, string search, string sidx, string sord)
        {
            // database call

            var TANK_usp_sel_DispatchReasonTypeUpd_spParams = new TANK_usp_sel_DispatchReasonTypeUpd_spParams
            {
                //TODO: re-factor it later from hard coded
                InstallID = 1
            };
            DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_sel_DispatchReasonTypeUPD", TANK_usp_sel_DispatchReasonTypeUpd_spParams);

            var data = (from p in dataTable.AsEnumerable()
                        select new
                        {
                            Id = p.Field<Int16>("Key"),
                            Description = p.Field<string>("Description*"),                          
                        }).ToList();

            //# database call

            int totalRecords = data.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
            data = data.Skip(page * rows).Take(rows).ToList();// bug in paging

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
        public JsonResult ManageDispatchReasonType(DispatchReasonTypePostModel postModel)
        {

            switch (Request.Form["oper"])
            {
                case "add":
                    {
                        #region add

                        var TANK_usp_insupd_DispatchReasonType_spParams = new TANK_usp_insupd_DispatchReasonType_spParams()
                        {
                            Key = postModel.Id,
                            Description = postModel.Description,                            
                            UpdateUserAN = "SYSTEM",
                            LocationId = 1,//todo
                            ActiveFL = true
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_DispatchReasonType",
                                                                          TANK_usp_insupd_DispatchReasonType_spParams);

                        #endregion add
                    }
                    break;
                case "edit":
                    {
                        #region edit

                        var TANK_usp_insupd_DispatchReasonType_spParams = new TANK_usp_insupd_DispatchReasonType_spParams()
                        {
                            Key = _sharedFunctions.ToNullableInt32(Request.Form["id"]),
                            Description = postModel.Description,   
                            UpdateUserAN = "SYSTEM",
                            LocationId = 1,//todo
                            ActiveFL = true
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_DispatchReasonType",
                                                                          TANK_usp_insupd_DispatchReasonType_spParams);

                        #endregion edit
                    }
                    break;
                case "del":
                    {
                        #region delete

                        var TANK_usp_insupd_DispatchReasonType_spParams = new TANK_usp_insupd_DispatchReasonType_spParams()
                        {
                            Key = postModel.Id,
                            ActiveFL = false,
                            UpdateUserAN = "SYSTEM"
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_DispatchReasonType",
                                                                          TANK_usp_insupd_DispatchReasonType_spParams);

                        #endregion delete
                    }
                    break;
                default:
                    break;
            }

            return Json(true);
        }


        #endregion 

        #region Drivers

        [HttpGet]
        public ActionResult Drivers()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetDrivers(int page, int rows, string search, string sidx, string sord)
        {
            // database call

            var TANK_usp_sel_DriversUpd_spParams = new TANK_usp_sel_DriversUpd_spParams()
            {
                //TODO: re-factor it later from hard coded
                InstallID = 1                
            };
            DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_sel_DriverUPD", TANK_usp_sel_DriversUpd_spParams);

            var data = (from p in dataTable.AsEnumerable()
                        select new
                        {
                            Id = p.Field<int>("Key"),
                            LastName = p.Field<string>("Last Name*"),
                            FirstName = p.Field<string>("First Name*"),
                            Trucker = p.Field<string>("Trucker #"),
                            Unit = p.Field<string>("Unit #"),
                            Radio = p.Field<string>("Radio"),
                            Phone = p.Field<string>("Phone"),
                            Mobile = p.Field<string>("Mobile"),                            
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
        public JsonResult ManageDrivers(DriversPostModel postModel)
        {

            switch (Request.Form["oper"])
            {
                case "add":
                    {
                        #region add

                        var TANK_usp_insupd_Driver_spParams = new TANK_usp_insupd_Driver_spParams()
                        {
                            Key = postModel.Id,
                            LastName = postModel.LastName,
                            FirstName = postModel.FirstName,
                            Trucker = postModel.Trucker,
                            Unit = postModel.Unit,
                            Radio= postModel.Radio,
                            Phone = postModel.Phone,
                            Mobile= postModel.Mobile,
                            UpdateUserAN = "SYSTEM",
                            ActiveFL = true
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_Driver",
                                                                          TANK_usp_insupd_Driver_spParams);

                        #endregion add
                    }
                    break;
                case "edit":
                    {
                        #region edit

                        var TANK_usp_insupd_Driver_spParams = new TANK_usp_insupd_Driver_spParams()
                        {
                            Key = _sharedFunctions.ToNullableInt32(Request.Form["id"]),
                            LastName = postModel.LastName,
                            FirstName = postModel.FirstName,
                            Trucker = postModel.Trucker,
                            Unit = postModel.Unit,
                            Radio = postModel.Radio,
                            Phone = postModel.Phone,
                            Mobile = postModel.Mobile,
                            UpdateUserAN = "SYSTEM",
                            ActiveFL = true
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_Driver",
                                                                          TANK_usp_insupd_Driver_spParams);

                        #endregion edit
                    }
                    break;
                case "del":
                    {
                        #region delete

                        var TANK_usp_insupd_Driver_spParams = new TANK_usp_insupd_Driver_spParams()
                        {
                            Key = postModel.Id,
                            ActiveFL = false,
                            UpdateUserAN = "SYSTEM"
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_Driver",
                                                                          TANK_usp_insupd_Driver_spParams);

                        #endregion delete
                    }
                    break;
                default:
                    break;
            }

            return Json(true);
        }


        #endregion 

        #region EquipmentTypes

        [HttpGet]
        public ActionResult EquipmentTypes()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetEquipmentTypes(int page, int rows, string search, string sidx, string sord)
        {
            // database call

            var TANK_usp_sel_EquipmentTypesUpd_spParams = new TANK_usp_sel_EquipmentTypesUpd_spParams()
            {
                //TODO: re-factor it later from hard coded
                InstallID = 1
               
            };
            DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_sel_EquipmentTypeUPD", TANK_usp_sel_EquipmentTypesUpd_spParams);

            var data = (from p in dataTable.AsEnumerable()
                        select new
                        {
                            Id = p.Field<int>("Key"),
                            Class = p.Field<string>("Class*"),
                            Description = p.Field<string>("Description*"),
                            Code = p.Field<string>("Code"),
                            Length = p.Field<string>("Length"),
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
        public JsonResult ManageEquipmentTypes(EquipmentTypesPostModel postModel)
        {

            switch (Request.Form["oper"])
            {
                case "add":
                    {
                        #region add

                        var TANK_usp_insupd_EquipmentType_spParams = new TANK_usp_insupd_EquipmentType_spParams()
                        {
                            Key = postModel.Id,
                            Class = postModel.Class,
                            Description = postModel.Description,
                            Code = postModel.Code,
                            Length = postModel.Length,
                            LocationID = 1,
                            UpdateUserAN = "SYSTEM",
                            ActiveFL = true
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_EquipmentType",
                                                                          TANK_usp_insupd_EquipmentType_spParams);

                        #endregion add
                    }
                    break;
                case "edit":
                    {
                        #region edit

                        var TANK_usp_insupd_EquipmentType_spParams = new TANK_usp_insupd_EquipmentType_spParams()
                        {
                            Key = _sharedFunctions.ToNullableInt32(Request.Form["id"]),
                            Class = postModel.Class,
                            Description = postModel.Description,
                            Code = postModel.Code,
                            Length = postModel.Length,
                            LocationID = 1,
                            UpdateUserAN = "SYSTEM",
                            ActiveFL = true
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_EquipmentType",
                                                                          TANK_usp_insupd_EquipmentType_spParams);

                        #endregion edit
                    }
                    break;
                case "del":
                    {
                        #region delete

                        var TANK_usp_insupd_EquipmentType_spParams = new TANK_usp_insupd_EquipmentType_spParams()
                        {
                            Key = postModel.Id,
                            ActiveFL = false,
                            UpdateUserAN = "SYSTEM"
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_EquipmentType",
                                                                          TANK_usp_insupd_EquipmentType_spParams);

                        #endregion delete
                    }
                    break;
                default:
                    break;
            }

            return Json(true);
        }


        #endregion

        #region FacilityParameters

        [HttpGet]
        public ActionResult FacilityParameters()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetFacilityParameters(int page, int rows, string search, string sidx, string sord)
        {
            // database call

            var TANK_usp_sel_FacilityParametersUpd_spParams = new TANK_usp_sel_FacilityParametersUpd_spParams()
            {
                //TODO: re-factor it later from hard coded
                InstallID = 1,
                MajorLocationID = 1
            };
            DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_sel_FacilityParameterUPD", TANK_usp_sel_FacilityParametersUpd_spParams);

            var data = (from p in dataTable.AsEnumerable()
                        select new
                        {
                            Id = p.Field<int>("Key"),
                            Name = p.Field<string>("Name*"),
                            Value = p.Field<string>("Value*"),
                            Description = p.Field<string>("Description"),
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
        public JsonResult ManageFacilityParameters(FacilityParametersPostModel postModel)
        {

            switch (Request.Form["oper"])
            {
                case "add":
                    {
                        #region add

                        var TANK_usp_insupd_FacilityParameter_spParams = new TANK_usp_insupd_FacilityParameter_spParams()
                        {
                            KEY = postModel.Id,
                            Name = postModel.Name,
                            InstallID = 1,
                            Value = postModel.Value,
                            Description = postModel.Description,
                            MajorLocationID = 1,
                            UpdateDT = DateTime.Now,
                            UpdateUserAN = "SYSTEM",
                            ActiveFL = true
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_FacilityParameter",
                                                                          TANK_usp_insupd_FacilityParameter_spParams);

                        #endregion add
                    }
                    break;
                case "edit":
                    {
                        #region edit

                        var TANK_usp_insupd_FacilityParameter_spParams = new TANK_usp_insupd_FacilityParameter_spParams()
                        {
                            KEY = _sharedFunctions.ToNullableInt32(Request.Form["id"]),
                            Name = postModel.Name,
                            InstallID = 1,
                            Value = postModel.Value,
                            Description = postModel.Description,
                            MajorLocationID = 1,
                            UpdateDT = DateTime.Now,
                            UpdateUserAN = "SYSTEM",
                            ActiveFL = true
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_FacilityParameter",
                                                                          TANK_usp_insupd_FacilityParameter_spParams);

                        #endregion edit
                    }
                    break;
                case "del":
                    {
                        #region delete

                        var TANK_usp_insupd_FacilityParameter_spParams = new TANK_usp_insupd_FacilityParameter_spParams()
                        {
                            KEY = postModel.Id,
                            ActiveFL = false,
                            UpdateUserAN = "SYSTEM"
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_FacilityParameter",
                                                                          TANK_usp_insupd_FacilityParameter_spParams);

                        #endregion delete
                    }
                    break;
                default:
                    break;
            }

            return Json(true);
        }
        
        #endregion

        #region Fittings

        [HttpGet]
        public ActionResult Fittings()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetFittings(int page, int rows, string search, string sidx, string sord)
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
        public JsonResult ManageFittings(FittingsPostModel postModel)
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


        #endregion

        #region Locations

        [HttpGet]
        public ActionResult Locations()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetLocations(int page, int rows, string search, string sidx, string sord)
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
        public JsonResult ManageLocations(LocationsPostModel postModel)
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


        #endregion

        #region MaintenanceCondition

        [HttpGet]
        public ActionResult MaintenanceCondition()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetMaintenanceCondition(int page, int rows, string search, string sidx, string sord)
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
        public JsonResult ManageMaintenanceCondition(MaintenanceConditionPostModel postModel)
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


        #endregion

        #region MovementType

        [HttpGet]
        public ActionResult MovementType()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetMovementType(int page, int rows, string search, string sidx, string sord)
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
        public JsonResult ManageMovementType(MovementTypePostModel postModel)
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


        #endregion

        #region HireReason

        [HttpGet]
        public ActionResult HireReason()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetHireReason(int page, int rows, string search, string sidx, string sord)
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
        public JsonResult ManageHireReason(HireReasonPostModel postModel)
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


        #endregion

        #region Operator

        [HttpGet]
        public ActionResult Operator()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetOperator(int page, int rows, string search, string sidx, string sord)
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
        public JsonResult ManageOperator(OperatorPostModel postModel)
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


        #endregion

        #region Owners

        [HttpGet]
        public ActionResult Owners()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetOwners(int page, int rows, string search, string sidx, string sord)
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
        public JsonResult ManageOwners(OwnersPostModel postModel)
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


        #endregion

        #region TankConstruction

        [HttpGet]
        public ActionResult TankConstruction()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetTankConstruction(int page, int rows, string search, string sidx, string sord)
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
        public JsonResult ManageTankConstruction(TankConstructionPostModel postModel)
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


        #endregion

        #region Vendor

        [HttpGet]
        public ActionResult Vendor()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetVendor(int page, int rows, string search, string sidx, string sord)
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
        public JsonResult ManageVendor(VendorPostModel postModel)
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


        #endregion

        #region WasteClass

        [HttpGet]
        public ActionResult WasteClass()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetWasteClass(int page, int rows, string search, string sidx, string sord)
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
        public JsonResult ManageWasteClass(WasteClassPostModel postModel)
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

        #endregion

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


    #endregion Model

    #region ChargeCode Model

    public class ChargeCodesPostModel
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }


    #endregion

    #region DispatchReasonType  Model

    public class DispatchReasonTypePostModel
    {
        public int? Id { get; set; }
        public string Description { get; set; }
    }


    #endregion 

    #region Drivers Model

    public class DriversPostModel
    {
        public int? Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Trucker { get; set; }
        public string Unit { get; set; }
        public string Radio { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
    }


    #endregion

    #region EquipmentTypes Model

    public class EquipmentTypesPostModel
    {
        public int? Id { get; set; }
        public int? Class { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public float Length { get; set; }
        public int LocationID { get; set; }
    }


    #endregion

    #region FacilityParameters Model

    public class FacilityParametersPostModel
    {
        public int? Id { get; set; }
        public int InstallID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public int? MajorLocationID { get; set; }
        public Boolean ActiveFL { get; set; }
        public DateTime UpdateDT { get; set; }
        public string UpdateUserAN { get; set; }
    }


    #endregion 

    #region Fittings Model

    public class FittingsPostModel
    {
        public int? Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Phone { get; set; }
        public string Ext { get; set; }
    }


    #endregion  

    #region Locations Model

    public class LocationsPostModel
    {
        public int? Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Phone { get; set; }
        public string Ext { get; set; }
    }


    #endregion  

    #region MaintenanceCondition Model

    public class MaintenanceConditionPostModel
    {
        public int? Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Phone { get; set; }
        public string Ext { get; set; }
    }


    #endregion 

    #region MovementType Model

    public class MovementTypePostModel
    {
        public int? Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Phone { get; set; }
        public string Ext { get; set; }
    }


    #endregion  

    #region HireReason Model

    public class HireReasonPostModel
    {
        public int? Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Phone { get; set; }
        public string Ext { get; set; }
    }


    #endregion  

    #region Operator Model

    public class OperatorPostModel
    {
        public int? Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Phone { get; set; }
        public string Ext { get; set; }
    }


    #endregion 

    #region Owners Model

    public class OwnersPostModel
    {
        public int? Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Phone { get; set; }
        public string Ext { get; set; }
    }


    #endregion 

    #region TankConstruction Model

    public class TankConstructionPostModel
    {
        public int? Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Phone { get; set; }
        public string Ext { get; set; }
    }


    #endregion  

    #region Vendor Model

    public class VendorPostModel
    {
        public int? Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Phone { get; set; }
        public string Ext { get; set; }
    }


    #endregion  

    #region WasteClass Model

    public class WasteClassPostModel
    {
        public int? Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Phone { get; set; }
        public string Ext { get; set; }
    }


    #endregion  

}