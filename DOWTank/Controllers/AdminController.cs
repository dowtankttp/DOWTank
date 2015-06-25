using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DOWTank.Common;
using DOWTank.Core.Domain.TANK_usp_insupd;
using DOWTank.Core.Domain.TANK_usp_sel;
using DOWTank.Core.Domain.TANK_usp_upd;
using DOWTank.Core.Service;
using DOWTank.Custom;
using DOWTank.Models;
using DOWTank.Utility;

namespace DOWTank.Controllers
{
    [ClaimsAuthorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        private readonly IUtilityService _utilityService;
        private readonly ISharedFunctions _sharedFunctions;

        public AdminController(IUtilityService utilityService, ISharedFunctions sharedFunctions)
        {
            _utilityService = utilityService;
            _sharedFunctions = sharedFunctions;
        }

        #region User

        public ActionResult UserIndex()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetUserData(int page, int rows, string search, string sidx, string sord, string userName, string loginId, string profile, string status)
        {
            PopulateSecurityExtended();
            // database call

            var spParams = new TANK_usp_sel_Security_spParams()
            {
                LocationID = SecurityExtended.LocationId
            };
            var data = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_Security_spResults>("[TANK_usp_sel_Security]", spParams);

            #region filter

            data = data.Where(s => s.UserAN != "" && s.UserAN != null).ToList();
            if (!string.IsNullOrEmpty(userName))
            {
                userName = userName.ToUpper();
                data = data.Where(s => s.FullName.ToUpper().Contains(userName)).ToList();
            }
            else if (!string.IsNullOrEmpty(loginId))
            {
                loginId = loginId.ToUpper();
                data = data.Where(s => s.UserAN.ToUpper().Contains(loginId)).ToList();
            }
            else if (!string.IsNullOrEmpty(profile))
            {
                profile = profile.ToUpper();
                data = data.Where(s => s.SecurityProfileDS.ToUpper().Contains(profile)).ToList();
            }
            else if (!string.IsNullOrEmpty(status))
            {
                status = status.ToUpper();
                data = data.Where(s => s.ActiveDS.ToUpper().Contains(status)).ToList();
            }

            #endregion filter

            //# database call

            int totalRecords = data.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
            int pageNumber = page - 1;
            data = data.Skip(pageNumber * rows).Take(rows).ToList();

            var jsonData = new
            {

                total = totalPages,
                page,
                records = totalRecords,
                rows = data

            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult LoadSecurityLocations(int page, int rows, string search, string sidx, string sord, int securityId)
        {
            var TANK_usp_sel_SecurityLocationALL_spParams = new TANK_usp_sel_SecurityLocationALL_spParams()
              {
                  SecurityID = securityId
              };
        
            var data = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_SecurityLocationALL_spResults>("TANK_usp_sel_SecurityLocationALL", TANK_usp_sel_SecurityLocationALL_spParams).ToList();

            int totalRecords = data.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
            int pageNumber = page - 1;
            data = data.Skip(pageNumber * rows).Take(rows).ToList();

            var jsonData = new
            {

                total = totalPages,
                page,
                records = totalRecords,
                rows = data

            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }
        
        [HttpGet]
        public ActionResult EditUser(int id)
        {
            PopulateEditUserDropdowns();
            // database call

            var spParams = new TANK_usp_sel_Security_spParams()
            {
                SecurityID = id
            };
            var data = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_Security_spResults>("[TANK_usp_sel_Security]", spParams).FirstOrDefault();
            //database call

            var viewModel = new EditUserViewModel();
            viewModel.SecurityID = id;
            viewModel.FullName = data.FullName;
            viewModel.ActiveFL = data.ActiveFL;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EditUser(EditUserViewModel postModel)
        {
            PopulateEditUserDropdowns();

            var TANK_usp_insupd_Security_spParams = new TANK_usp_insupd_Security_spParams()
            {
                SecurityID = postModel.SecurityID,
                ActiveFL = postModel.ActiveFL
            };

            _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_Security", TANK_usp_insupd_Security_spParams);


            return RedirectToAction("UserIndex", "Admin");
        }

        [HttpPost]
        public JsonResult DeleteProfile(DeleteProfilePostModel postModel)
        {
            var TANK_usp_del_SecurityLocation_spParams = new TANK_usp_del_SecurityLocation_spParams()
            {
                SecurityID = postModel.SecurityId,
                LocationID = postModel.LocationId
            };

            _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_del_SecurityLocation", TANK_usp_del_SecurityLocation_spParams);
            
            return Json(postModel.LocationId);
        }

        [HttpPost]
        public JsonResult SaveSecurityLocation(SecurityLocationPostModel postModel)
        {
            var TANK_usp_insupd_SecurityLocation_spParams = new TANK_usp_insupd_SecurityLocation_spParams()
                {
                    SecurityID = postModel.SecurityID,
                    DefaultFL = postModel.DefaultFL,
                    LocationID = postModel.LocationID,
                    SecurityProfileId = postModel.SecurityProfileId
                };

            _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_SecurityLocation", TANK_usp_insupd_SecurityLocation_spParams);

            return Json(postModel.LocationID);
        }

        private void PopulateEditUserDropdowns()
        {
            PopulateSecurityExtended();

            #region FacilityLocations

            {
                var TANK_usp_sel_LocationDDL_spParams = new TANK_usp_sel_LocationDDL_spParams()
                {
                    IncludeBlank = false,
                    LocationTypeCD = 2
                };
                var response = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_LocationDDL_spParams_spResults>("TANK_usp_sel_LocationDDL", TANK_usp_sel_LocationDDL_spParams);

                var list = new List<SelectListItem>();
                if (response != null && response.Any())
                {
                    foreach (var item in response)
                    {
                        list.Add(new SelectListItem
                        {
                            Text = item.LocationDS,
                            Value = item.LocationID.ToString()
                        });
                    }
                    ViewBag.FacilityLocations = list;
                }
            }

            #endregion FacilityLocations

            #region SecurityProfile

            {
                var TANK_usp_sel_SecurityProfileDDL_spParams = new TANK_usp_sel_SecurityProfileDDL_spParams()
                {
                    IncludeBlank = false,
                    LocationID = SecurityExtended.LocationId
                };
                var response = _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_SecurityProfileDDL_spResults>("TANK_usp_sel_SecurityProfileDDL", TANK_usp_sel_SecurityProfileDDL_spParams);

                var list = new List<SelectListItem>();
                if (response != null && response.Any())
                {
                    foreach (var item in response)
                    {
                        list.Add(new SelectListItem
                        {
                            Text = item.SecurityProfileDS,
                            Value = item.SecurityProfileID.ToString()
                        });
                    }
                    ViewBag.SecurityProfile = list;
                }
            }

            #endregion SecurityProfile
        }

        #endregion User

        #region User Profile

        public ActionResult UserProfile()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetUserProfileData(int page, int rows, string search, string sidx, string sord)
        {
            PopulateSecurityExtended();
            // database call

            var TANK_usp_sel_SecurityProfile_spParams = new TANK_usp_sel_SecurityProfile_spParams()
            {
                //TODO: re-factor it later from hard coded
                LocationID = SecurityExtended.LocationId
            };
            DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_sel_SecurityProfile", TANK_usp_sel_SecurityProfile_spParams);

            var data = (from p in dataTable.AsEnumerable()
                        select new
                        {
                            Id = p.Field<int>("SecurityProfileID"),
                            SecurityProfileDS = p.Field<string>("SecurityProfileDS"),
                            ActiveDS = p.Field<string>("ActiveDS"),
                            CreateUserNM = p.Field<string>("CreateUserNM"),
                            CreateDT = p.Field<DateTime>("CreateDT").ToShortDateString(),
                            UpdateUserNM = p.Field<string>("UpdateUserNM"),
                            UpdateDT = p.Field<DateTime>("UpdateDT").ToShortDateString(),
                        }).ToList();

            //# database call

            #region sort

            data = data.OrderBy(s => s.SecurityProfileDS).ToList();
            if (sidx == "SecurityProfileDS" && sord == "desc")
            {
                data = data.OrderByDescending(s => s.SecurityProfileDS).ToList();
            }

            #endregion sort



            int totalRecords = data.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
            int pageNumber = page - 1;
            data = data.Skip(pageNumber * rows).Take(rows).ToList();

            var jsonData = new
            {

                total = totalPages,
                page,
                records = totalRecords,
                rows = data

            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SecurityProfile(int? id)
        {
            PopulateSecurityExtended();
            var viewModel = new UserSecurityProfileViewModel();
            viewModel.Id = id ?? 0;
            #region TANK_usp_sel_SecurityProfile

            if (id.HasValue)
            {
                var TANK_usp_sel_SecurityProfile_spParams = new TANK_usp_sel_SecurityProfile_spParams()
                    {
                        //TODO: re-factor it later from hard coded
                        LocationID = SecurityExtended.LocationId,
                        SecurityProfileID = id
                    };
                DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_sel_SecurityProfile",
                                                                                      TANK_usp_sel_SecurityProfile_spParams);

                var userProfile = (from p in dataTable.AsEnumerable()
                                   select new
                                       {
                                           Id = p.Field<int>("SecurityProfileID"),
                                           SecurityProfileDS = p.Field<string>("SecurityProfileDS"),
                                           ActiveDS = p.Field<string>("ActiveDS"),
                                           CreateUserNM = p.Field<string>("CreateUserNM"),
                                           CreateDT = p.Field<DateTime>("CreateDT").ToShortDateString(),
                                           UpdateUserNM = p.Field<string>("UpdateUserNM"),
                                           UpdateDT = p.Field<DateTime>("UpdateDT").ToShortDateString(),
                                           ActiveFL = p.Field<bool>("ActiveFL")
                                       }).FirstOrDefault();

                viewModel.Id = userProfile.Id;
                viewModel.ProfileName = userProfile.SecurityProfileDS;
                viewModel.IsActive = userProfile.ActiveFL;
            }

            #endregion TANK_usp_sel_SecurityProfile

            #region Dashboard menu

            {
                TANK_usp_sel_SecurityProfilePrivileges_spParams TANK_usp_sel_SecurityProfilePrivileges_spParams = new TANK_usp_sel_SecurityProfilePrivileges_spParams
                    ()
                    {
                        CurrentUserProfileID = SecurityExtended.LocationId,
                        PrivilegeCategoryID = 2,
                        SecurityProfileID = id
                    };
                var dashboardMenuItems =
                    _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_SecurityProfilePrivileges_spResults>(
                        "TANK_usp_sel_SecurityProfilePrivileges", TANK_usp_sel_SecurityProfilePrivileges_spParams);
                if (dashboardMenuItems != null && dashboardMenuItems.Any())
                {
                    var dashboardMenuList = new List<UserSecurityListViewModel>();
                    foreach (var dashboardMenuItem in dashboardMenuItems.ToList())
                    {
                        var item = new UserSecurityListViewModel();
                        item.PrivilegeID = dashboardMenuItem.PrivilegeID.Value;
                        item.PrivilegeDS = dashboardMenuItem.PrivilegeDS;
                        item.GrantedFL = dashboardMenuItem.GrantedFL;
                        dashboardMenuList.Add(item);
                    }
                    viewModel.DashboardMenu = dashboardMenuList;
                }
            }

            #endregion Dashboard menu

            #region Dashboard List

            {
                var TANK_usp_sel_SecurityProfilePrivileges_spParams = new TANK_usp_sel_SecurityProfilePrivileges_spParams
                    ()
                    {
                        //todo: refactor it later
                        CurrentUserProfileID = SecurityExtended.LocationId,
                        PrivilegeCategoryID = 1,
                        SecurityProfileID = id
                    };
                var dashboardListItems =
                    _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_SecurityProfilePrivileges_spResults>(
                        "TANK_usp_sel_SecurityProfilePrivileges", TANK_usp_sel_SecurityProfilePrivileges_spParams);
                if (dashboardListItems != null && dashboardListItems.Any())
                {
                    var dashboardMenuList = new List<UserSecurityListViewModel>();
                    foreach (var dashboardMenuItem in dashboardListItems.ToList())
                    {
                        var item = new UserSecurityListViewModel();
                        item.PrivilegeID = dashboardMenuItem.PrivilegeID.Value;
                        item.PrivilegeDS = dashboardMenuItem.PrivilegeDS;
                        item.GrantedFL = dashboardMenuItem.GrantedFL;
                        dashboardMenuList.Add(item);
                    }
                    viewModel.DashboardList = dashboardMenuList;
                }
            }

            #endregion Dashboard List

            #region Dispatch Screen

            {
                var TANK_usp_sel_SecurityProfilePrivileges_spParams = new TANK_usp_sel_SecurityProfilePrivileges_spParams
                    ()
                {
                    CurrentUserProfileID = SecurityExtended.LocationId,
                    PrivilegeCategoryID = 3,
                    SecurityProfileID = id
                };
                var listItems =
                    _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_SecurityProfilePrivileges_spResults>(
                        "TANK_usp_sel_SecurityProfilePrivileges", TANK_usp_sel_SecurityProfilePrivileges_spParams);
                if (listItems != null && listItems.Any())
                {
                    var menuList = new List<UserSecurityListViewModel>();
                    foreach (var dashboardMenuItem in listItems.ToList())
                    {
                        var item = new UserSecurityListViewModel();
                        item.PrivilegeID = dashboardMenuItem.PrivilegeID.Value;
                        item.PrivilegeDS = dashboardMenuItem.PrivilegeDS;
                        item.GrantedFL = dashboardMenuItem.GrantedFL;
                        menuList.Add(item);
                    }
                    viewModel.DispatchScreen = menuList;
                }
            }

            #endregion DispatchScreen

            #region prep Screen

            {
                var TANK_usp_sel_SecurityProfilePrivileges_spParams = new TANK_usp_sel_SecurityProfilePrivileges_spParams
                    ()
                {
                    CurrentUserProfileID = SecurityExtended.LocationId,
                    PrivilegeCategoryID = 4,
                    SecurityProfileID = id
                };
                var listItems =
                    _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_SecurityProfilePrivileges_spResults>(
                        "TANK_usp_sel_SecurityProfilePrivileges", TANK_usp_sel_SecurityProfilePrivileges_spParams);
                if (listItems != null && listItems.Any())
                {
                    var menuList = new List<UserSecurityListViewModel>();
                    foreach (var dashboardMenuItem in listItems.ToList())
                    {
                        var item = new UserSecurityListViewModel();
                        item.PrivilegeID = dashboardMenuItem.PrivilegeID.Value;
                        item.PrivilegeDS = dashboardMenuItem.PrivilegeDS;
                        item.GrantedFL = dashboardMenuItem.GrantedFL;
                        menuList.Add(item);
                    }
                    viewModel.PrepScreen = menuList;
                }
            }

            #endregion prep screen

            #region Require Cleaning

            {
                var TANK_usp_sel_SecurityProfilePrivileges_spParams = new TANK_usp_sel_SecurityProfilePrivileges_spParams
                    ()
                {
                    //todo: refactor it later
                    CurrentUserProfileID = SecurityExtended.LocationId,
                    PrivilegeCategoryID = 5,
                    SecurityProfileID = id
                };
                var listItems =
                    _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_SecurityProfilePrivileges_spResults>(
                        "TANK_usp_sel_SecurityProfilePrivileges", TANK_usp_sel_SecurityProfilePrivileges_spParams);
                if (listItems != null && listItems.Any())
                {
                    var menuList = new List<UserSecurityListViewModel>();
                    foreach (var dashboardMenuItem in listItems.ToList())
                    {
                        var item = new UserSecurityListViewModel();
                        item.PrivilegeID = dashboardMenuItem.PrivilegeID.Value;
                        item.PrivilegeDS = dashboardMenuItem.PrivilegeDS;
                        item.GrantedFL = dashboardMenuItem.GrantedFL;
                        menuList.Add(item);
                    }
                    viewModel.RequireCleaning = menuList;
                }
            }

            #endregion Require Cleaning

            #region Require Serivce

            {
                var TANK_usp_sel_SecurityProfilePrivileges_spParams = new TANK_usp_sel_SecurityProfilePrivileges_spParams
                    ()
                {
                    //todo: refactor it later
                    CurrentUserProfileID = SecurityExtended.LocationId,
                    PrivilegeCategoryID = 6,
                    SecurityProfileID = id
                };
                var listItems =
                    _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_SecurityProfilePrivileges_spResults>(
                        "TANK_usp_sel_SecurityProfilePrivileges", TANK_usp_sel_SecurityProfilePrivileges_spParams);
                if (listItems != null && listItems.Any())
                {
                    var menuList = new List<UserSecurityListViewModel>();
                    foreach (var dashboardMenuItem in listItems.ToList())
                    {
                        var item = new UserSecurityListViewModel();
                        item.PrivilegeID = dashboardMenuItem.PrivilegeID.Value;
                        item.PrivilegeDS = dashboardMenuItem.PrivilegeDS;
                        item.GrantedFL = dashboardMenuItem.GrantedFL;
                        menuList.Add(item);
                    }
                    viewModel.RequireService = menuList;
                }
            }

            #endregion Require Service

            #region Tank Search Screen

            {
                var TANK_usp_sel_SecurityProfilePrivileges_spParams = new TANK_usp_sel_SecurityProfilePrivileges_spParams
                    ()
                {
                    //todo: refactor it later
                    CurrentUserProfileID = SecurityExtended.LocationId,
                    PrivilegeCategoryID = 7,
                    SecurityProfileID = id
                };
                var listItems =
                    _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_SecurityProfilePrivileges_spResults>(
                        "TANK_usp_sel_SecurityProfilePrivileges", TANK_usp_sel_SecurityProfilePrivileges_spParams);
                if (listItems != null && listItems.Any())
                {
                    var menuList = new List<UserSecurityListViewModel>();
                    foreach (var dashboardMenuItem in listItems.ToList())
                    {
                        var item = new UserSecurityListViewModel();
                        item.PrivilegeID = dashboardMenuItem.PrivilegeID.Value;
                        item.PrivilegeDS = dashboardMenuItem.PrivilegeDS;
                        item.GrantedFL = dashboardMenuItem.GrantedFL;
                        menuList.Add(item);
                    }
                    viewModel.TankSearchScreen = menuList;
                }
            }

            #endregion Tank Search Screen

            #region Tank History Screen

            {
                var TANK_usp_sel_SecurityProfilePrivileges_spParams = new TANK_usp_sel_SecurityProfilePrivileges_spParams
                    ()
                {
                    CurrentUserProfileID = SecurityExtended.LocationId,
                    PrivilegeCategoryID = 8,
                    SecurityProfileID = id
                };
                var listItems =
                    _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_SecurityProfilePrivileges_spResults>(
                        "TANK_usp_sel_SecurityProfilePrivileges", TANK_usp_sel_SecurityProfilePrivileges_spParams);
                if (listItems != null && listItems.Any())
                {
                    var menuList = new List<UserSecurityListViewModel>();
                    foreach (var dashboardMenuItem in listItems.ToList())
                    {
                        var item = new UserSecurityListViewModel();
                        item.PrivilegeID = dashboardMenuItem.PrivilegeID.Value;
                        item.PrivilegeDS = dashboardMenuItem.PrivilegeDS;
                        item.GrantedFL = dashboardMenuItem.GrantedFL;
                        menuList.Add(item);
                    }
                    viewModel.TankHistoryScreen = menuList;
                }
            }

            #endregion Tank History Screen

            #region Grounded Matrix

            {
                var TANK_usp_sel_SecurityProfilePrivileges_spParams = new TANK_usp_sel_SecurityProfilePrivileges_spParams
                    ()
                {
                    CurrentUserProfileID = SecurityExtended.LocationId,
                    PrivilegeCategoryID = 9,
                    SecurityProfileID = id
                };
                var listItems =
                    _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_SecurityProfilePrivileges_spResults>(
                        "TANK_usp_sel_SecurityProfilePrivileges", TANK_usp_sel_SecurityProfilePrivileges_spParams);
                if (listItems != null && listItems.Any())
                {
                    var menuList = new List<UserSecurityListViewModel>();
                    foreach (var dashboardMenuItem in listItems.ToList())
                    {
                        var item = new UserSecurityListViewModel();
                        item.PrivilegeID = dashboardMenuItem.PrivilegeID.Value;
                        item.PrivilegeDS = dashboardMenuItem.PrivilegeDS;
                        item.GrantedFL = dashboardMenuItem.GrantedFL;
                        menuList.Add(item);
                    }
                    viewModel.GroundedMatrix = menuList;
                }
            }

            #endregion Grounded Matrix

            #region ReportsMenuOptions

            {
                var TANK_usp_sel_SecurityProfilePrivileges_spParams = new TANK_usp_sel_SecurityProfilePrivileges_spParams
                    ()
                {
                    CurrentUserProfileID = SecurityExtended.LocationId,
                    PrivilegeCategoryID = 10,
                    SecurityProfileID = id
                };
                var listItems =
                    _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_SecurityProfilePrivileges_spResults>(
                        "TANK_usp_sel_SecurityProfilePrivileges", TANK_usp_sel_SecurityProfilePrivileges_spParams);
                if (listItems != null && listItems.Any())
                {
                    var menuList = new List<UserSecurityListViewModel>();
                    foreach (var dashboardMenuItem in listItems.ToList())
                    {
                        var item = new UserSecurityListViewModel();
                        item.PrivilegeID = dashboardMenuItem.PrivilegeID.Value;
                        item.PrivilegeDS = dashboardMenuItem.PrivilegeDS;
                        item.GrantedFL = dashboardMenuItem.GrantedFL;
                        menuList.Add(item);
                    }
                    viewModel.ReportsMenuOptions = menuList;
                }
            }

            #endregion ReportsMenuOptions

            #region AdminMenuOptions

            {
                var TANK_usp_sel_SecurityProfilePrivileges_spParams = new TANK_usp_sel_SecurityProfilePrivileges_spParams
                    ()
                {
                    CurrentUserProfileID = SecurityExtended.LocationId,
                    PrivilegeCategoryID = 11,
                    SecurityProfileID = id
                };
                var listItems =
                    _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_SecurityProfilePrivileges_spResults>(
                        "TANK_usp_sel_SecurityProfilePrivileges", TANK_usp_sel_SecurityProfilePrivileges_spParams);
                if (listItems != null && listItems.Any())
                {
                    var menuList = new List<UserSecurityListViewModel>();
                    foreach (var dashboardMenuItem in listItems.ToList())
                    {
                        var item = new UserSecurityListViewModel();
                        item.PrivilegeID = dashboardMenuItem.PrivilegeID.Value;
                        item.PrivilegeDS = dashboardMenuItem.PrivilegeDS;
                        item.GrantedFL = dashboardMenuItem.GrantedFL;
                        menuList.Add(item);
                    }
                    viewModel.AdminMenuOptions = menuList;
                }
            }

            #endregion AdminMenuOptions

            #region ProductMaster

            {
                var TANK_usp_sel_SecurityProfilePrivileges_spParams = new TANK_usp_sel_SecurityProfilePrivileges_spParams
                    ()
                {
                    CurrentUserProfileID = SecurityExtended.LocationId,
                    PrivilegeCategoryID = 12,
                    SecurityProfileID = id
                };
                var listItems =
                    _utilityService.ExecStoredProcedureWithResults<TANK_usp_sel_SecurityProfilePrivileges_spResults>(
                        "TANK_usp_sel_SecurityProfilePrivileges", TANK_usp_sel_SecurityProfilePrivileges_spParams);
                if (listItems != null && listItems.Any())
                {
                    var menuList = new List<UserSecurityListViewModel>();
                    foreach (var dashboardMenuItem in listItems.ToList())
                    {
                        var item = new UserSecurityListViewModel();
                        item.PrivilegeID = dashboardMenuItem.PrivilegeID.Value;
                        item.PrivilegeDS = dashboardMenuItem.PrivilegeDS;
                        item.GrantedFL = dashboardMenuItem.GrantedFL;
                        menuList.Add(item);
                    }
                    viewModel.ProductMaster = menuList;
                }
            }

            #endregion ProductMaster

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SecurityProfile(UserSecurityProfileViewModel postModel)
        {
            PopulateSecurityExtended();
            if (!ModelState.IsValid)
            {
                return View(postModel);
            }

            #region SecurityProfile

            //ADD
            if (postModel.Id == 0)
            {
                var TANK_usp_insupd_SecurityProfile_spParams = new TANK_usp_insupd_SecurityProfile_spParams()
                {
                    LocationID = SecurityExtended.LocationId,
                    SecurityProfileDS = postModel.ProfileName,
                    ActiveFL = postModel.IsActive,
                    UpdateUserAN = SecurityExtended.UserName
                };
                var id = _utilityService.ExecStoredProcedureWithResults<int>("TANK_usp_insupd_SecurityProfile",
                                                                   TANK_usp_insupd_SecurityProfile_spParams);
                postModel.Id = id.FirstOrDefault();
            }
            else
            {
                var TANK_usp_insupd_SecurityProfile_spParams = new TANK_usp_insupd_SecurityProfile_spParams()
                {
                    LocationID = SecurityExtended.LocationId,
                    SecurityProfileDS = postModel.ProfileName,
                    SecurityProfileID = postModel.Id,
                    ActiveFL = postModel.IsActive,
                    UpdateUserAN = SecurityExtended.UserName
                };
                _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_SecurityProfile",
                                                                  TANK_usp_insupd_SecurityProfile_spParams);
            }

            #endregion SecurityProfile

            #region SecurityProfilePrivilege

            if (postModel.Id != 0)
            {
                TANK_usp_insupd_SecurityProfilePrivilege_spParams TANK_usp_insupd_SecurityProfilePrivilege_spParams = new TANK_usp_insupd_SecurityProfilePrivilege_spParams
                    ()
                    {
                        SecurityProfileID = postModel.Id,
                        PrivilegeList = GetPrivilegeList(postModel)
                    };

                _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_SecurityProfilePrivilege",
                                                                  TANK_usp_insupd_SecurityProfilePrivilege_spParams);
            }

            #endregion SecurityProfilePrivilege

            Success("User Profile Changes Saved Successfully.");
            return RedirectToAction("SecurityProfile", "Admin", new { @id = postModel.Id });
        }

        private string GetPrivilegeList(UserSecurityProfileViewModel postModel)
        {

            string returnList = string.Empty;
            if (postModel.DashboardMenu.Any())
            {
                var dashboardMenu = postModel.DashboardMenu.Where(s => s.PrivilegeID != null).Select(s => s.PrivilegeID).ToList();
                returnList = string.Join(",", dashboardMenu);
            }
            if (postModel.DashboardList.Any())
            {
                var dashboardList = postModel.DashboardList.Where(s => s.PrivilegeID != null).Select(s => s.PrivilegeID).ToList();
                returnList += "," + string.Join(",", dashboardList);
            }
            if (postModel.DispatchScreen.Any())
            {
                var dispatchScreen = postModel.DispatchScreen.Where(s => s.PrivilegeID != null).Select(s => s.PrivilegeID).ToList();
                returnList += "," + string.Join(",", dispatchScreen);
            }
            if (postModel.PrepScreen.Any())
            {
                var list = postModel.PrepScreen.Where(s => s.PrivilegeID != null).Select(s => s.PrivilegeID).ToList();
                returnList += "," + string.Join(",", list);
            }
            if (postModel.RequireCleaning.Any())
            {
                var list = postModel.RequireCleaning.Where(s => s.PrivilegeID != null).Select(s => s.PrivilegeID).ToList();
                returnList += "," + string.Join(",", list);
            }
            if (postModel.RequireService.Any())
            {
                var list = postModel.RequireService.Where(s => s.PrivilegeID != null).Select(s => s.PrivilegeID).ToList();
                returnList += "," + string.Join(",", list);
            }
            if (postModel.TankSearchScreen.Any())
            {
                var list = postModel.TankSearchScreen.Where(s => s.PrivilegeID != null).Select(s => s.PrivilegeID).ToList();
                returnList += "," + string.Join(",", list);
            }
            if (postModel.TankHistoryScreen.Any())
            {
                var list = postModel.TankHistoryScreen.Where(s => s.PrivilegeID != null).Select(s => s.PrivilegeID).ToList();
                returnList += "," + string.Join(",", list);
            }
            if (postModel.GroundedMatrix.Any())
            {
                var list = postModel.GroundedMatrix.Where(s => s.PrivilegeID != null).Select(s => s.PrivilegeID).ToList();
                returnList += "," + string.Join(",", list);
            }
            if (postModel.ReportsMenuOptions.Any())
            {
                var list = postModel.ReportsMenuOptions.Where(s => s.PrivilegeID != null).Select(s => s.PrivilegeID).ToList();
                returnList += "," + string.Join(",", list);
            }
            if (postModel.AdminMenuOptions.Any())
            {
                var list = postModel.AdminMenuOptions.Where(s => s.PrivilegeID != null).Select(s => s.PrivilegeID).ToList();
                returnList += "," + string.Join(",", list);
            }
            if (postModel.ProductMaster.Any())
            {
                var list = postModel.ProductMaster.Where(s => s.PrivilegeID != null).Select(s => s.PrivilegeID).ToList();
                returnList += "," + string.Join(",", list);
            }

            return returnList;

        }

        #endregion User Profile

        #region product master

        [HttpGet]
        public ActionResult ProductMasterIndex()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetProductsData(int page, int rows, string search, string sidx, string sord, string productName, string productCode)
        {
            PopulateSecurityExtended();
            // database call

            var TANK_usp_sel_ProductList_spParams = new TANK_usp_sel_ProductList_spParams()
            {
                //TODO: re-factor it later from hard coded
                MajorLocationID = SecurityExtended.LocationId.Value
            };
            DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_sel_ProductList", TANK_usp_sel_ProductList_spParams);

            var data = (from p in dataTable.AsEnumerable()
                        select new
                        {
                            Id = p.Field<Int16>("ProductID"),
                            ProductDS = p.Field<string>("ProductDS"),
                            ProductCodeAN = p.Field<string>("ProductCodeAN"),
                            Status = p.Field<string>("ActiveDS"),
                            CreateDT = p.Field<DateTime>("CreateDT").ToShortDateString(),
                            GrantedFL = p.Field<int>("GrantedFL")
                        }).ToList();

            //# database call

            #region sort

            data = data.OrderBy(s => s.ProductDS).ToList();
            if (sidx == "ProductDS" && sord == "desc")
            {
                data = data.OrderByDescending(s => s.ProductDS).ToList();
            }

            #endregion sort

            #region filter

            if (!string.IsNullOrEmpty(productName))
            {
                data = data.Where(s => s.ProductDS.Contains(productName)).ToList();
            }
            else if (!string.IsNullOrEmpty(productCode))
            {
                data = data.Where(s => s.ProductCodeAN.Contains(productCode)).ToList();
            }

            #endregion filter

            int totalRecords = data.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
            int pageNumber = page - 1;
            data = data.Skip(pageNumber * rows).Take(rows).ToList();

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
        public JsonResult UpdateProductLocation(UpdateProductionLocationPostModel postModel)
        {
            PopulateSecurityExtended();
            if (postModel == null || postModel.ProductId == 0)
            {
                return Json(0);
            }

            TANK_usp_insupd_ProductLocation_spParams TANK_usp_insupd_ProductLocation_spParams = new TANK_usp_insupd_ProductLocation_spParams()
                {
                    ProductID = postModel.ProductId,
                    ActiveFL = postModel.ActiveFL,
                    LocationID = SecurityExtended.LocationId.Value
                };

            _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_ProductLocation", TANK_usp_insupd_ProductLocation_spParams);


            return Json(1);
        }


        [HttpGet]
        public ActionResult ProductMaster()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProductMaster(AdminProductModel postModel)
        {
            if (!ModelState.IsValid)
            {
                return View(postModel);
            }
            _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_product", postModel);
            Success("Product Saved Successfully.");
            //return appropriate message
            return View(postModel);
        }

        //PopulateHazardClassDDL
        [HttpGet]
        public JsonResult PopulateHazardClassDDL(string searchTerm)
        {
            var response = _sharedFunctions.PopulateHazardClass(false);
            if (response != null && response.Any())
            {
                var data = response.Where(r => r.HazardClassTypeDS != null).Select(r => new { id = r.HazardClassTypeCD, text = r.HazardClassTypeDS }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        //PopulateTankConstructionTypeDDL
        [HttpGet]
        public JsonResult PopulateTankConstructionTypeDDL(string searchTerm)
        {
            searchTerm = searchTerm.Trim();
            //todo: re-factor it later as required
            var response = _sharedFunctions.PopulateTankConstructionType(false);
            if (response != null && response.Any())
            {
                var data = response.Where(r => r.TankConstructionTypeDS != null).Select(r => new { id = r.TankConstructionTypeCD, text = r.TankConstructionTypeDS }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        #endregion product master

        #region Contacts

        [HttpGet]
        public ActionResult Contacts()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetContacts(int page, int rows, string search, string sidx, string sord)
        {
            PopulateSecurityExtended();
            // database call

            var TANK_usp_sel_ContactUPD_spParams = new TANK_usp_sel_ContactUPD_spParams()
            {
                //TODO: re-factor it later from hard coded
                InstallID = 1,
                MajorLocationID = SecurityExtended.LocationId.Value
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
                            UpdateUserAN = SecurityExtended.UserName,
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
                            UpdateUserAN = SecurityExtended.UserName,
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
                            UpdateUserAN = SecurityExtended.UserName
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

        #region Deleted Moves

        [HttpGet]
        public ActionResult RecoverDeletedMoves()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetDeletedMoves(int page, int rows, string search, string sidx, string sord, string tankNumber, string from, string to, string product, string chargeNumber, string shipment)
        {
            PopulateSecurityExtended();
            // database call

            var TANK_usp_sel_DeletedMoves_spParams = new TANK_usp_sel_DeletedMoves_spParams()
            {
                //TODO: re-factor it later from hard coded
                LocationID = SecurityExtended.LocationId.Value
            };
            DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_sel_DeletedMoves", TANK_usp_sel_DeletedMoves_spParams);

            var data = (from p in dataTable.AsEnumerable()
                        select new
                        {
                            Id = p.Field<int>("DispatchID"),
                            Tank = p.Field<string>("Tank #"),
                            From = p.Field<string>("From"),
                            To = p.Field<string>("To"),
                            Product = p.Field<string>("Product"),
                            ChargeNumber = p.Field<string>("Charge #"),
                            Shipment = p.Field<string>("Shipment"),
                            Status = p.Field<string>("Status"),
                            Start = p.Field<string>("Start"),
                            End = p.Field<string>("End"),
                            Chassis = p.Field<string>("Chassis"),
                            Driver = p.Field<string>("Driver"),
                        }).ToList();

            //# database call

            #region sort

            data = data.OrderBy(s => s.Tank).ToList();
            if (sidx == "Tank" && sord == "desc")
            {
                data = data.OrderByDescending(s => s.Tank).ToList();
            }

            #endregion sort

            #region filter

            if (!string.IsNullOrEmpty(tankNumber))
            {
                data = data.Where(s => s.Tank.Contains(tankNumber)).ToList();
            }
            else if (!string.IsNullOrEmpty(from))
            {
                data = data.Where(s => s.From.Contains(from)).ToList();
            }
            else if (!string.IsNullOrEmpty(to))
            {
                data = data.Where(s => s.To.Contains(to)).ToList();
            }
            else if (!string.IsNullOrEmpty(product))
            {
                data = data.Where(s => s.To.Contains(product)).ToList();
            }
            else if (!string.IsNullOrEmpty(chargeNumber))
            {
                data = data.Where(s => s.To.Contains(chargeNumber)).ToList();
            }
            else if (!string.IsNullOrEmpty(shipment))
            {
                data = data.Where(s => s.To.Contains(shipment)).ToList();
            }

            #endregion filter

            int totalRecords = data.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
            page = page - 1;
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



        #endregion Deleted Moves

        #region Equipment

        public ActionResult Equipment(string EquipmentAN)
        {
            var viewModel = new AdminEquipmentModel();
            if (!string.IsNullOrEmpty(EquipmentAN))
            {
                //load data
                var data = _sharedFunctions.RefreshEquipment(null, EquipmentAN).FirstOrDefault();
                viewModel.EquipmentAN = data.EquipmentAN;
                viewModel.EquipmentTypeCDSelected = data.EquipmentTypeCD;
                viewModel.EquipmentClassTypeCD = data.EquipmentClassTypeCD;
                viewModel.PoolFL = data.PoolFL;
                viewModel.OwnerIDSelected = data.OwnerID;
                viewModel.OperatorIDSelected = data.OperatorID;
                viewModel.LoadStatusTypeCDSelected = data.LoadStatusTypeCD;
                viewModel.TankGradeTypeCDSelected = data.TankGradeTypeCD;
                viewModel.MoveTypeCDSelected = data.MoveTypeCD;
                viewModel.Tank5YRTestDT = string.IsNullOrEmpty(data.TankLastTestDT) ? (DateTime?)null : Convert.ToDateTime(data.TankLastTestDT);
                viewModel.DOTInspectedDT = string.IsNullOrEmpty(data.DOTInspectedDT) ? (DateTime?)null : Convert.ToDateTime(data.DOTInspectedDT);
                viewModel.LastTestHydroFL = data.LastTestHydroFL;
                viewModel.TankCapacity = data.TankCapacity;
                viewModel.ActiveFL = data.ActiveFL;
                viewModel.ProductIDSelected = data.ProductID;
                viewModel.ProductDS = data.ProductDS;
                viewModel.DedicatedProductIDSelected = data.DedicatedProductID;
                viewModel.DedicatedProductID = data.DedicatedProductID;
                viewModel.DedicatedLocationID = data.DedicatedLocationID;
                viewModel.DedicatedLocationIDSelected = data.DedicatedLocationID;
                viewModel.LocationID = data.LocationID;
                viewModel.LocationIDSelected = data.LocationID;
                viewModel.LocationDS = data.LocationDS;
            }

            LoadAdminEquipmentDropdowns();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Equipment(AdminEquipmentModel postModel)
        {
            PopulateSecurityExtended();
            LoadAdminEquipmentDropdowns();
            if (!ModelState.IsValid)
            {
                return View(postModel);
            }

            postModel.EquipmentAN = postModel.EquipmentAN.Left(10);
            if (postModel.EquipmentAN.Trim().Length == 11)
                postModel.CheckDigitAN = postModel.EquipmentAN.Substring(10, 11)[0];
            TANK_usp_insupd_equipment_spParams TANK_usp_insupd_equipment_spParams = new TANK_usp_insupd_equipment_spParams()
                {
                    EquipmentAN = postModel.EquipmentAN,
                    ActiveFL = postModel.ActiveFL,
                    BarrelConditionTypeCD = postModel.BarrelConditionTypeCD,
                    DedicatedLocationID = postModel.DedicatedLocationID,
                    DedicatedProductID = postModel.DedicatedProductID,
                    EquipmentTypeCD = postModel.EquipmentTypeCD,
                    LastTestHydroFL = postModel.LastTestHydroFL,
                    DOTInspectedDT = postModel.DOTInspectedDT,
                    LocationID = postModel.LocationID,
                    PoolFL = postModel.PoolFL,
                    OwnerID = postModel.OwnerID,
                    OperatorID = postModel.OperatorID,
                    LoadStatusTypeCD = postModel.LoadStatusTypeCD,
                    TankGradeTypeCD = postModel.TankGradeTypeCD,
                    ProductID = postModel.ProductID,
                    MoveTypeCD = postModel.MoveTypeCD,
                    Tank5YRTestDT = postModel.Tank5YRTestDT,
                    TankCapacity = postModel.TankCapacity,
                    UpdateUserAN = SecurityExtended.UserName
                };
            _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_equipment", TANK_usp_insupd_equipment_spParams);
            Success("Equipment Saved Successfully.");
            //return appropriate message
            return View(postModel);
        }

        private void LoadAdminEquipmentDropdowns()
        {
            PopulateSecurityExtended();
            #region equipmentTypeList

            {
                var equipmentTypeList = new List<SelectListItem>();
                var response = _sharedFunctions.PopulateEquipmentType(1);
                if (response != null && response.Any())
                {
                    foreach (var item in response)
                    {
                        equipmentTypeList.Add(new SelectListItem
                            {
                                Text = item.EquipmentTypeDS,
                                Value = item.EquipmentTypeCD.HasValue ? item.EquipmentTypeCD.Value.ToString() : string.Empty
                            });
                    }
                    ViewBag.EquipmentTypeCD = equipmentTypeList;
                }
            }

            #endregion owner

            #region owner
            {
                var list = new List<SelectListItem>();
                var response = _sharedFunctions.PopulateOwnerDDL();
                if (response != null && response.Any())
                {
                    list.Add(new SelectListItem() { Text = "", Value = "" });
                    foreach (var item in response)
                    {
                        list.Add(new SelectListItem
                            {
                                Text = item.OwnerNM,
                                Value = item.OwnerID.HasValue ? item.OwnerID.Value.ToString() : string.Empty
                            });
                    }
                    ViewBag.OwnerID = list;
                }
            }
            #endregion owner

            #region operator
            {
                var list = new List<SelectListItem>();
                var response = _sharedFunctions.PopulateOperatorDDL();
                if (response != null && response.Any())
                {
                    list.Add(new SelectListItem() { Text = "", Value = "" });
                    foreach (var item in response)
                    {
                        list.Add(new SelectListItem
                        {
                            Text = item.OperatorNM,
                            Value = item.OperatorID.HasValue ? item.OperatorID.Value.ToString() : string.Empty
                        });
                    }
                    ViewBag.OperatorID = list;
                }
            }
            #endregion operator

            #region load status
            {
                var list = new List<SelectListItem>();
                var response = _sharedFunctions.PopulateLoadStatusType();
                if (response != null && response.Any())
                {
                    list.Add(new SelectListItem() { Text = "", Value = "" });
                    foreach (var item in response)
                    {
                        list.Add(new SelectListItem
                        {
                            Text = item.LoadStatusTypeDS,
                            Value = item.LoadStatusTypeCD.ToString()
                        });
                    }
                    ViewBag.LoadStatusTypeCD = list;
                }
            }
            #endregion loadstatus

            #region Tank Grade
            {
                var list = new List<SelectListItem>();
                var response = _sharedFunctions.PopulateTankGrade();
                if (response != null && response.Any())
                {
                    list.Add(new SelectListItem() { Text = "", Value = "" });
                    foreach (var item in response)
                    {
                        list.Add(new SelectListItem
                        {
                            Text = item.TankGradeTypeDS,
                            Value = item.TankGradeTypeCD.ToString()
                        });
                    }
                    ViewBag.TankGradeTypeCD = list;
                }
            }
            #endregion Tank Grade

            #region Service Type
            {
                var list = new List<SelectListItem>();
                var response = _sharedFunctions.PopulateMoveType();
                if (response != null && response.Any())
                {
                    list.Add(new SelectListItem() { Text = "", Value = "" });
                    foreach (var item in response)
                    {
                        list.Add(new SelectListItem
                        {
                            Text = item.MoveTypeDS,
                            Value = item.MoveTypeCD.ToString()
                        });
                    }
                    ViewBag.MoveTypeCD = list;
                }
            }
            #endregion Service Type

            #region Tank condition
            {
                var list = new List<SelectListItem>();
                var response = _sharedFunctions.PopulateBarrelCondition();
                if (response != null && response.Any())
                {
                    list.Add(new SelectListItem() { Text = "", Value = "" });
                    foreach (var item in response)
                    {
                        list.Add(new SelectListItem
                        {
                            Text = item.BarrelConditionTypeDS,
                            Value = item.BarrelConditionTypeCD.ToString()
                        });
                    }
                    ViewBag.BarrelConditionTypeCD = list;
                }
            }
            #endregion Tank condition
        }

        //PopulateTypeDDL
        [HttpGet]
        public JsonResult PopulateTypeDDL(string searchTerm)
        {
            searchTerm = searchTerm.ToUpper();
            var response = _sharedFunctions.PopulateEquipmentType(1);
            response = response.Where(s => s.EquipmentTypeDS.ToUpper().Contains(searchTerm)).ToList();
            if (response != null && response.Any())
            {
                var data = response.Where(r => r.EquipmentTypeDS != null).Select(r => new { id = r.EquipmentTypeCD, text = r.EquipmentTypeDS }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        //PopulateOwnerDDL
        [HttpGet]
        public JsonResult PopulateOwnerDDL(string searchTerm)
        {
            searchTerm = searchTerm.ToUpper();
            var response = _sharedFunctions.PopulateOwnerDDL();
            response = response.Where(s => s.OwnerNM.ToUpper().Contains(searchTerm)).ToList();
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
            searchTerm = searchTerm.ToUpper();

            var response = _sharedFunctions.PopulateOperatorDDL();
            response = response.Where(r => r.OperatorNM.ToUpper().Contains(searchTerm)).ToList();
            if (response != null && response.Any())
            {
                var data = response.Where(r => r.OperatorID != null).Select(r => new { id = r.OperatorID, text = r.OperatorNM }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        //PopulateLoadStatusDDL
        [HttpGet]
        public JsonResult PopulateLoactionDDL(string searchTerm)
        {
            PopulateSecurityExtended();
            searchTerm = searchTerm.ToUpper();
            var response = _sharedFunctions.PopulateLoadPointLocationAll(SecurityExtended.LocationId.Value);
            response = response.Where(r => r.LocationDS.ToUpper().Contains(searchTerm)).ToList();
            if (response != null && response.Any())
            {
                var data = response.Select(r => new { id = r.LocationID, text = r.LocationDS }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        //PopulateLoadStatusDDL
        [HttpGet]
        public JsonResult PopulateTankGradeDDL(string searchTerm)
        {
            searchTerm = searchTerm.ToUpper();
            var response = _sharedFunctions.PopulateTankGrade();
            response = response.Where(r => r.TankGradeTypeDS.ToUpper().Contains(searchTerm)).ToList();
            if (response != null && response.Any())
            {
                var data = response.Where(r => r.TankGradeTypeDS != null).Select(r => new { id = r.TankGradeTypeCD, text = r.TankGradeTypeDS }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        //PopulateTankConditionDDL
        [HttpGet]
        public JsonResult PopulateTankConditionDDL(string searchTerm)
        {
            searchTerm = searchTerm.ToUpper();
            var response = _sharedFunctions.PopulateBarrelCondition();
            response = response.Where(r => r.BarrelConditionTypeDS.ToUpper().Contains(searchTerm));
            if (response != null && response.Any())
            {
                var data = response.Where(r => r.BarrelConditionTypeDS != null).Select(r => new { id = r.BarrelConditionTypeCD, text = r.BarrelConditionTypeDS }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        //PopulateMoveTypeDDL
        [HttpGet]
        public JsonResult PopulateMoveTypeDDL(string searchTerm)
        {
            searchTerm = searchTerm.ToUpper();
            var response = _sharedFunctions.PopulateMoveType();
            response = response.Where(s => s.MoveTypeDS.ToUpper().Contains(searchTerm)).ToList();
            if (response != null && response.Any())
            {
                var data = response.Where(r => r.MoveTypeDS != null).Select(r => new { id = r.MoveTypeCD, text = r.MoveTypeDS }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        //PopulateMoveTypeDDL
        [HttpGet]
        public JsonResult PopulateProductDDL(string searchTerm)
        {
            searchTerm = searchTerm.ToUpper();
            var response = _sharedFunctions.PopulateProduct(false, 1, searchTerm);
            response = response.Where(s => s.ProductDS.ToUpper().Contains(searchTerm)).ToList();
            if (response != null && response.Any())
            {
                var data = response.Where(r => r.ProductDS != null).Select(r => new { id = r.ProductID, text = r.ProductDS }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        #endregion Equipment

        #region Charge Codes

        [HttpGet]
        public ActionResult ChargeCodes()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetChargeCodes(int page, int rows, string search, string sidx, string sord)
        {
            PopulateSecurityExtended();
            // database call

            var TANK_usp_sel_ChargeCodesUpd_spParams = new TANK_usp_sel_ChargeCodesUpd_spParams()
            {
                //TODO: re-factor it later from hard coded
                InstallID = 1,
                MajorLocationID = SecurityExtended.LocationId.Value
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
            data = data.Skip((page - 1) * rows).Take(rows).ToList();

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
            PopulateSecurityExtended();
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
                            UpdateUserAN = SecurityExtended.UserName,
                            MajorLocationID = SecurityExtended.LocationId,//todo change location id
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
                            UpdateUserAN = SecurityExtended.UserName,
                            MajorLocationID = SecurityExtended.LocationId.Value,//todo change location id
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
                            UpdateUserAN = SecurityExtended.UserName
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
            data = data.Skip((page - 1) * rows).Take(rows).ToList();// bug in paging

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
            PopulateSecurityExtended();
            switch (Request.Form["oper"])
            {
                case "add":
                    {
                        #region add

                        var TANK_usp_insupd_DispatchReasonType_spParams = new TANK_usp_insupd_DispatchReasonType_spParams()
                        {
                            Key = postModel.Id,
                            Description = postModel.Description,
                            UpdateUserAN = SecurityExtended.UserName,
                            LocationId = SecurityExtended.LocationId,//todo
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
                            UpdateUserAN = SecurityExtended.UserName,
                            LocationId = SecurityExtended.LocationId.Value,//todo
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
                            UpdateUserAN = SecurityExtended.UserName
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
            data = data.Skip((page - 1) * rows).Take(rows).ToList();

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
                            Radio = postModel.Radio,
                            Phone = postModel.Phone,
                            Mobile = postModel.Mobile,
                            UpdateUserAN = SecurityExtended.UserName,
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
                            UpdateUserAN = SecurityExtended.UserName,
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
                            UpdateUserAN = SecurityExtended.UserName
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
                            Id = p.Field<Int16>("Key"),
                            Class = p.Field<string>("Class*"),
                            Description = p.Field<string>("Description*"),
                            Code = p.Field<string>("Code"),
                            Length = p.Field<double>("Length"),
                        }).ToList();

            //# database call

            int totalRecords = data.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
            data = data.Skip((page - 1) * rows).Take(rows).ToList();

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
            PopulateSecurityExtended();
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
                            LocationID = SecurityExtended.LocationId.Value,
                            UpdateUserAN = SecurityExtended.UserName,
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
                            LocationID = SecurityExtended.LocationId.Value,
                            UpdateUserAN = SecurityExtended.UserName,
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
                            UpdateUserAN = SecurityExtended.UserName
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
            PopulateSecurityExtended();
            var TANK_usp_sel_FacilityParametersUpd_spParams = new TANK_usp_sel_FacilityParametersUpd_spParams()
            {
                //TODO: re-factor it later from hard coded
                InstallID = 1,
                MajorLocationID = SecurityExtended.LocationId.Value
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
            data = data.Skip((page - 1) * rows).Take(rows).ToList();

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
                            MajorLocationID = SecurityExtended.LocationId.Value,
                            UpdateDT = DateTime.Now,
                            UpdateUserAN = SecurityExtended.UserName,
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
                            MajorLocationID = SecurityExtended.LocationId.Value,
                            UpdateDT = DateTime.Now,
                            UpdateUserAN = SecurityExtended.UserName,
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
                            UpdateUserAN = SecurityExtended.UserName
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
            PopulateSecurityExtended();
            // database call

            var TANK_usp_sel_FittingsUpd_spParams = new TANK_usp_sel_FittingsUpd_spParams()
            {
                //TODO: re-factor it later from hard coded
                InstallID = 1,
                LocationID = SecurityExtended.LocationId.Value
            };
            DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_sel_FittingUPD", TANK_usp_sel_FittingsUpd_spParams);

            var data = (from p in dataTable.AsEnumerable()
                        select new
                        {
                            Id = p.Field<Int16>("Key"),
                            Description = p.Field<string>("Description*"),
                        }).ToList();

            //# database call

            int totalRecords = data.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
            data = data.Skip((page - 1) * rows).Take(rows).ToList();

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
            PopulateSecurityExtended();

            switch (Request.Form["oper"])
            {
                case "add":
                    {
                        #region add

                        var TANK_usp_insupd_Fitting_spParams = new TANK_usp_insupd_Fitting_spParams()
                        {
                            Key = postModel.Id,
                            Description = postModel.Description,
                            LocationId = SecurityExtended.LocationId.Value,
                            UpdateUserAN = SecurityExtended.UserName,
                            ActiveFL = true
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_Fitting",
                                                                          TANK_usp_insupd_Fitting_spParams);

                        #endregion add
                    }
                    break;
                case "edit":
                    {
                        #region edit

                        var TANK_usp_insupd_Fitting_spParams = new TANK_usp_insupd_Fitting_spParams()
                        {
                            Key = _sharedFunctions.ToNullableInt32(Request.Form["id"]),
                            Description = postModel.Description,
                            LocationId = SecurityExtended.LocationId.Value,
                            UpdateUserAN = SecurityExtended.UserName,
                            ActiveFL = true
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_Fitting",
                                                                          TANK_usp_insupd_Fitting_spParams);

                        #endregion edit
                    }
                    break;
                case "del":
                    {
                        #region delete

                        var TANK_usp_insupd_Fitting_spParams = new TANK_usp_insupd_Fitting_spParams()
                        {
                            Key = postModel.Id,
                            ActiveFL = false,
                            UpdateUserAN = SecurityExtended.UserName
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_Fitting",
                                                                          TANK_usp_insupd_Fitting_spParams);

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
            PopulateSecurityExtended();
            // database call

            var TANK_usp_sel_LocationsUpd_spParams = new TANK_usp_sel_LocationsUpd_spParams()
            {
                //TODO: re-factor it later from hard coded
                InstallID = 1,
                MajorLocationID = SecurityExtended.LocationId.Value
            };
            DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_sel_LocationUPD", TANK_usp_sel_LocationsUpd_spParams);

            var data = (from p in dataTable.AsEnumerable()
                        select new
                        {
                            Id = p.Field<int>("Key"),
                            LocType = p.Field<string>("LocType*"),
                            Description = p.Field<string>("Description*"),
                            Parent = p.Field<string>("Parent"),
                            Code = p.Field<string>("Code"),
                        }).ToList();

            //# database call

            int totalRecords = data.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
            data = data.Skip((page - 1) * rows).Take(rows).ToList();

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

                        var TANK_usp_insupd_Location_spParams = new TANK_usp_insupd_Location_spParams()
                        {
                            Key = postModel.Id,
                            Code = postModel.Code,
                            Description = postModel.Description,
                            ParentLocationID = postModel.ParentLocationID,
                            LocationTypeCD = null,
                            UpdateUserAN = SecurityExtended.UserName,
                            ActiveFL = true
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_Location",
                                                                          TANK_usp_insupd_Location_spParams);

                        #endregion add
                    }
                    break;
                case "edit":
                    {
                        #region edit

                        var TANK_usp_insupd_Location_spParams = new TANK_usp_insupd_Location_spParams()
                        {
                            Key = _sharedFunctions.ToNullableInt32(Request.Form["id"]),
                            Code = postModel.Code,
                            Description = postModel.Description,
                            ParentLocationID = postModel.ParentLocationID,
                            LocationTypeCD = null,
                            UpdateUserAN = SecurityExtended.UserName,
                            ActiveFL = true
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_Location",
                                                                          TANK_usp_insupd_Location_spParams);

                        #endregion edit
                    }
                    break;
                case "del":
                    {
                        #region delete

                        var TANK_usp_insupd_Location_spParams = new TANK_usp_insupd_Location_spParams()
                        {
                            Key = postModel.Id,
                            ActiveFL = false,
                            UpdateUserAN = SecurityExtended.UserName
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_Location",
                                                                          TANK_usp_insupd_Location_spParams);

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

            var TANK_usp_sel_MaintenanceConditionUpd_spParams = new TANK_usp_sel_MaintenanceConditionUpd_spParams()
            {
                //TODO: re-factor it later from hard coded
                InstallID = 1
            };
            DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_sel_MaintConditionTypeUPD", TANK_usp_sel_MaintenanceConditionUpd_spParams);

            var data = (from p in dataTable.AsEnumerable()
                        select new
                        {
                            Id = p.Field<Int16>("Key"),
                            Description = p.Field<string>("Description*")

                        }).ToList();

            //# database call

            int totalRecords = data.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
            data = data.Skip((page - 1) * rows).Take(rows).ToList();

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

                        var TANK_usp_insupd_MaintConditionType_spParams = new TANK_usp_insupd_MaintConditionType_spParams()
                        {
                            Key = postModel.Id,
                            Description = postModel.Description,
                            UpdateUserAN = SecurityExtended.UserName,
                            ActiveFL = true
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_MaintConditionType",
                                                                          TANK_usp_insupd_MaintConditionType_spParams);

                        #endregion add
                    }
                    break;
                case "edit":
                    {
                        #region edit

                        var TANK_usp_insupd_MaintConditionType_spParams = new TANK_usp_insupd_MaintConditionType_spParams()
                        {
                            Key = _sharedFunctions.ToNullableInt32(Request.Form["id"]),
                            Description = postModel.Description,
                            UpdateUserAN = SecurityExtended.UserName,
                            ActiveFL = true
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_MaintConditionType",
                                                                          TANK_usp_insupd_MaintConditionType_spParams);

                        #endregion edit
                    }
                    break;
                case "del":
                    {
                        #region delete

                        var TANK_usp_insupd_MaintConditionType_spParams = new TANK_usp_insupd_MaintConditionType_spParams()
                        {
                            Key = postModel.Id,
                            ActiveFL = false,
                            UpdateUserAN = SecurityExtended.UserName
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_MaintConditionType",
                                                                          TANK_usp_insupd_MaintConditionType_spParams);

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

            TANK_usp_sel_MovementTypeUpd_spParams TANK_usp_sel_MovementTypeUpd_spParams = null;

            DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_sel_MoveTypeUPD", TANK_usp_sel_MovementTypeUpd_spParams);

            var data = (from p in dataTable.AsEnumerable()
                        select new
                        {
                            Id = p.Field<Int32>("Key"),
                            Description = p.Field<string>("Description*"),
                        }).ToList();

            //# database call

            int totalRecords = data.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
            data = data.Skip((page - 1) * rows).Take(rows).ToList();

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

                        var TANK_usp_insupd_MoveType_spParams = new TANK_usp_insupd_MoveType_spParams()
                        {
                            Key = postModel.Id,
                            Description = postModel.Description,
                            UpdateUserAN = SecurityExtended.UserName,
                            ActiveFL = true
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_MoveType",
                                                                          TANK_usp_insupd_MoveType_spParams);

                        #endregion add
                    }
                    break;
                case "edit":
                    {
                        #region edit

                        var TANK_usp_insupd_MoveType_spParams = new TANK_usp_insupd_MoveType_spParams()
                        {
                            Key = _sharedFunctions.ToNullableInt32(Request.Form["id"]),
                            Description = postModel.Description,
                            UpdateUserAN = SecurityExtended.UserName,
                            ActiveFL = true
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_MoveType",
                                                                          TANK_usp_insupd_MoveType_spParams);

                        #endregion edit
                    }
                    break;
                case "del":
                    {
                        #region delete

                        var TANK_usp_insupd_MoveType_spParams = new TANK_usp_insupd_MoveType_spParams()
                        {
                            Key = postModel.Id,
                            ActiveFL = false,
                            UpdateUserAN = SecurityExtended.UserName
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_MoveType",
                                                                          TANK_usp_insupd_MoveType_spParams);

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

            TANK_usp_sel_OnHireReasonTypeUPD_spParams TANK_usp_sel_OnHireReasonTypeUPD_spParams = null;

            DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_sel_OnHireReasonTypeUPD", TANK_usp_sel_OnHireReasonTypeUPD_spParams);

            var data = (from p in dataTable.AsEnumerable()
                        select new
                        {
                            Id = p.Field<Int16>("Key"),
                            Description = p.Field<string>("Description*")
                        }).ToList();

            //# database call

            int totalRecords = data.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
            data = data.Skip((page - 1) * rows).Take(rows).ToList();

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

                        var TANK_usp_insupd_OnHireReasonType_spParams = new TANK_usp_insupd_OnHireReasonType_spParams()
                        {
                            Key = postModel.Id,
                            Description = postModel.Description,
                            UpdateUserAN = SecurityExtended.UserName,
                            ActiveFL = true
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_OnHireReasonType",
                                                                          TANK_usp_insupd_OnHireReasonType_spParams);

                        #endregion add
                    }
                    break;
                case "edit":
                    {
                        #region edit

                        var TANK_usp_insupd_OnHireReasonType_spParams = new TANK_usp_insupd_OnHireReasonType_spParams()
                        {
                            Key = _sharedFunctions.ToNullableInt32(Request.Form["id"]),
                            Description = postModel.Description,
                            UpdateUserAN = SecurityExtended.UserName,
                            ActiveFL = true
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_OnHireReasonType",
                                                                          TANK_usp_insupd_OnHireReasonType_spParams);

                        #endregion edit
                    }
                    break;
                case "del":
                    {
                        #region delete

                        var TANK_usp_insupd_OnHireReasonType_spParams = new TANK_usp_insupd_OnHireReasonType_spParams()
                        {
                            Key = postModel.Id,
                            ActiveFL = false,
                            UpdateUserAN = SecurityExtended.UserName
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_OnHireReasonType",
                                                                          TANK_usp_insupd_OnHireReasonType_spParams);

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

            var TANK_usp_sel_OperatorUpd_spParams = new TANK_usp_sel_OperatorUpd_spParams()
            {
                //TODO: re-factor it later from hard coded
                InstallID = 1
            };
            DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("[TANK_usp_sel_OperatorUPD]", TANK_usp_sel_OperatorUpd_spParams);

            var data = (from p in dataTable.AsEnumerable()
                        select new
                        {
                            Id = p.Field<Int32>("Key"),
                            Name = p.Field<string>("Name*")
                        }).ToList();

            //# database call

            int totalRecords = data.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
            data = data.Skip((page - 1) * rows).Take(rows).ToList();

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

                        var TANK_usp_insupd_Operator_spParams = new TANK_usp_insupd_Operator_spParams()
                        {
                            Key = postModel.Id,
                            Name = postModel.Name,
                            UpdateUserAN = SecurityExtended.UserName,
                            ActiveFL = true
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_Operator",
                                                                          TANK_usp_insupd_Operator_spParams);

                        #endregion add
                    }
                    break;
                case "edit":
                    {
                        #region edit

                        var TANK_usp_insupd_Operator_spParams = new TANK_usp_insupd_Operator_spParams()
                        {
                            Key = _sharedFunctions.ToNullableInt32(Request.Form["id"]),
                            Name = postModel.Name,
                            UpdateUserAN = SecurityExtended.UserName,
                            ActiveFL = true
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_Operator",
                                                                          TANK_usp_insupd_Operator_spParams);

                        #endregion edit
                    }
                    break;
                case "del":
                    {
                        #region delete

                        var TANK_usp_insupd_Operator_spParams = new TANK_usp_insupd_Operator_spParams()
                        {
                            Key = postModel.Id,
                            ActiveFL = false,
                            UpdateUserAN = SecurityExtended.UserName
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_Operator",
                                                                          TANK_usp_insupd_Operator_spParams);

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

            var TANK_usp_sel_OwnersUpd_spParams = new TANK_usp_sel_OwnersUpd_spParams()
            {
                //TODO: re-factor it later from hard coded
                InstallID = 1
            };
            DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_sel_OwnerUPD", TANK_usp_sel_OwnersUpd_spParams);

            var data = (from p in dataTable.AsEnumerable()
                        select new
                        {
                            Id = p.Field<Int32>("Key"),
                            Name = p.Field<string>("Name*")
                        }).ToList();

            //# database call

            int totalRecords = data.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
            data = data.Skip((page - 1) * rows).Take(rows).ToList();

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

                        var TANK_usp_insupd_Owner_spParams = new TANK_usp_insupd_Owner_spParams()
                        {
                            Key = postModel.Id,
                            Name = postModel.Name,
                            UpdateUserAN = SecurityExtended.UserName,
                            ActiveFL = true
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_Owner",
                                                                          TANK_usp_insupd_Owner_spParams);

                        #endregion add
                    }
                    break;
                case "edit":
                    {
                        #region edit

                        var TANK_usp_insupd_Owner_spParams = new TANK_usp_insupd_Owner_spParams()
                        {
                            Key = _sharedFunctions.ToNullableInt32(Request.Form["id"]),
                            Name = postModel.Name,
                            UpdateUserAN = SecurityExtended.UserName,
                            ActiveFL = true
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_Owner",
                                                                          TANK_usp_insupd_Owner_spParams);

                        #endregion edit
                    }
                    break;
                case "del":
                    {
                        #region delete

                        var TANK_usp_insupd_Owner_spParams = new TANK_usp_insupd_Owner_spParams()
                        {
                            Key = postModel.Id,
                            ActiveFL = false,
                            UpdateUserAN = SecurityExtended.UserName
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_Owner",
                                                                          TANK_usp_insupd_Owner_spParams);

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

            var TANK_usp_sel_TankConstructionUpd_spParams = new TANK_usp_sel_TankConstructionUpd_spParams()
            {
                //TODO: re-factor it later from hard coded
                InstallID = 1
            };
            DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_sel_TankConstructionTypeUPD", TANK_usp_sel_TankConstructionUpd_spParams);

            var data = (from p in dataTable.AsEnumerable()
                        select new
                        {
                            Id = p.Field<Int32>("Key"),
                            Description = p.Field<string>("Description*")
                        }).ToList();

            //# database call

            int totalRecords = data.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
            data = data.Skip((page - 1) * rows).Take(rows).ToList();

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

                        var TANK_usp_insupd_TankConstructionType_spParams = new TANK_usp_insupd_TankConstructionType_spParams()
                        {
                            Key = postModel.Id,
                            Description = postModel.Description,
                            UpdateUserAN = SecurityExtended.UserName,
                            ActiveFL = true
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_TankConstructionType",
                                                                          TANK_usp_insupd_TankConstructionType_spParams);

                        #endregion add
                    }
                    break;
                case "edit":
                    {
                        #region edit

                        var TANK_usp_insupd_TankConstructionType_spParams = new TANK_usp_insupd_TankConstructionType_spParams()
                        {
                            Key = _sharedFunctions.ToNullableInt32(Request.Form["id"]),
                            Description = postModel.Description,
                            UpdateUserAN = SecurityExtended.UserName,
                            ActiveFL = true
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_TankConstructionType",
                                                                          TANK_usp_insupd_TankConstructionType_spParams);

                        #endregion edit
                    }
                    break;
                case "del":
                    {
                        #region delete

                        var TANK_usp_insupd_TankConstructionType_spParams = new TANK_usp_insupd_TankConstructionType_spParams()
                        {
                            Key = postModel.Id,
                            ActiveFL = false,
                            UpdateUserAN = SecurityExtended.UserName
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_TankConstructionType",
                                                                          TANK_usp_insupd_TankConstructionType_spParams);

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

            TANK_usp_sel_VendorUpd_spParams TANK_usp_sel_VendorUpd_spParams = null;

            DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_sel_VendorUPD", TANK_usp_sel_VendorUpd_spParams);

            var data = (from p in dataTable.AsEnumerable()
                        select new
                        {
                            Id = p.Field<Int32>("Key"),
                            Code = p.Field<string>("Code*"),
                            Description = p.Field<string>("Description*")
                        }).ToList();

            //# database call

            int totalRecords = data.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
            data = data.Skip((page - 1) * rows).Take(rows).ToList();

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

                        var TANK_usp_insupd_Vendor_spParams = new TANK_usp_insupd_Vendor_spParams()
                        {
                            Key = postModel.Id,
                            Description = postModel.Description,
                            Code = postModel.Code,
                            UpdateUserAN = SecurityExtended.UserName,
                            ActiveFL = true
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_Vendor",
                                                                          TANK_usp_insupd_Vendor_spParams);

                        #endregion add
                    }
                    break;
                case "edit":
                    {
                        #region edit

                        var TANK_usp_insupd_Vendor_spParams = new TANK_usp_insupd_Vendor_spParams()
                        {
                            Key = _sharedFunctions.ToNullableInt32(Request.Form["id"]),
                            Description = postModel.Description,
                            Code = postModel.Code,
                            UpdateUserAN = SecurityExtended.UserName,
                            ActiveFL = true
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_Vendor",
                                                                          TANK_usp_insupd_Vendor_spParams);

                        #endregion edit
                    }
                    break;
                case "del":
                    {
                        #region delete

                        var TANK_usp_insupd_Vendor_spParams = new TANK_usp_insupd_Vendor_spParams()
                        {
                            Key = postModel.Id,
                            ActiveFL = false,
                            UpdateUserAN = SecurityExtended.UserName
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_Vendor",
                                                                          TANK_usp_insupd_Vendor_spParams);

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

            TANK_usp_sel_WasteClassUpd_spParams TANK_usp_sel_WasteClassUpd_spParams = null;

            DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_sel_WasteClassTypeUPD", TANK_usp_sel_WasteClassUpd_spParams);

            var data = (from p in dataTable.AsEnumerable()
                        select new
                        {
                            Id = p.Field<Int32>("Key"),
                            Description = p.Field<string>("Description*")
                        }).ToList();

            //# database call

            int totalRecords = data.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
            data = data.Skip((page - 1) * rows).Take(rows).ToList();

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

                        var TANK_usp_insupd_WasteClassType_spParams = new TANK_usp_insupd_WasteClassType_spParams()
                        {
                            Key = postModel.Id,
                            Description = postModel.Description,
                            UpdateUserAN = SecurityExtended.UserName,
                            ActiveFL = true
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_WasteClassType",
                                                                          TANK_usp_insupd_WasteClassType_spParams);

                        #endregion add
                    }
                    break;
                case "edit":
                    {
                        #region edit

                        var TANK_usp_insupd_WasteClassType_spParams = new TANK_usp_insupd_WasteClassType_spParams()
                        {
                            Key = _sharedFunctions.ToNullableInt32(Request.Form["id"]),
                            Description = postModel.Description,
                            UpdateUserAN = SecurityExtended.UserName,
                            ActiveFL = true
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_WasteClassType",
                                                                          TANK_usp_insupd_WasteClassType_spParams);

                        #endregion edit
                    }
                    break;
                case "del":
                    {
                        #region delete

                        var TANK_usp_insupd_WasteClassType_spParams = new TANK_usp_insupd_WasteClassType_spParams()
                        {
                            Key = postModel.Id,
                            ActiveFL = false,
                            UpdateUserAN = SecurityExtended.UserName
                        };
                        _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_WasteClassType",
                                                                          TANK_usp_insupd_WasteClassType_spParams);

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


    #region product master

    public class ProductMasterPostModel
    {

    }

    public class UpdateProductionLocationPostModel
    {
        public int ProductId { get; set; }
        public bool ActiveFL { get; set; }
    }

    #endregion product master

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

    #region Recover Deleted Moves

    public class DeleteMovesPostModel
    {
        public string TankNumber { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Product { get; set; }
        public string ChargeNumber { get; set; }
        public string Shipment { get; set; }
    }

    #endregion Recover Deleted Moves

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
        public double Length { get; set; }
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
        public int? LocationId { get; set; }
        public string Description { get; set; }
        public Boolean ActiveFL { get; set; }
        public string UpdateUserAN { get; set; }
    }


    #endregion

    #region Locations Model

    public class LocationsPostModel
    {
        public int? Id { get; set; }
        public int? ParentLocationID { get; set; }
        public int? LocationTypeCD { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public Boolean ActiveFL { get; set; }
        public string UpdateUserAN { get; set; }
    }


    #endregion

    #region MaintenanceCondition Model

    public class MaintenanceConditionPostModel
    {
        public int? Id { get; set; }
        public string Description { get; set; }
        public Boolean ActiveFL { get; set; }
        public string UpdateUserAN { get; set; }
    }


    #endregion

    #region MovementType Model

    public class MovementTypePostModel
    {
        public int? Id { get; set; }
        public string Description { get; set; }
        public Boolean ActiveFL { get; set; }
        public string UpdateUserAN { get; set; }
    }


    #endregion

    #region HireReason Model

    public class HireReasonPostModel
    {
        public int? Id { get; set; }
        public string Description { get; set; }
        public Boolean ActiveFL { get; set; }
        public string UpdateUserAN { get; set; }
    }


    #endregion

    #region Operator Model

    public class OperatorPostModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public Boolean ActiveFL { get; set; }
        public string UpdateUserAN { get; set; }
    }


    #endregion

    #region Owners Model

    public class OwnersPostModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public Boolean ActiveFL { get; set; }
        public string UpdateUserAN { get; set; }
    }


    #endregion

    #region TankConstruction Model

    public class TankConstructionPostModel
    {
        public int? Id { get; set; }
        public string Description { get; set; }
        public Boolean ActiveFL { get; set; }
        public string UpdateUserAN { get; set; }
    }


    #endregion

    #region Vendor Model

    public class VendorPostModel
    {
        public int? Id { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public Boolean ActiveFL { get; set; }
        public string UpdateUserAN { get; set; }
    }


    #endregion

    #region WasteClass Model

    public class WasteClassPostModel
    {
        public int? Id { get; set; }
        public string Description { get; set; }
        public Boolean ActiveFL { get; set; }
        public string UpdateUserAN { get; set; }
    }


    #endregion

}