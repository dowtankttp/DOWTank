﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOWTank.Common;
using DOWTank.Core.Domain.TANK_usp_rpt;
using DOWTank.Core.Enum;
using DOWTank.Core.Service;
using DOWTank.Models;

namespace DOWTank.Controllers
{
    public class TankSearchController : BaseController
    {
        private readonly IUtilityService _utilityService;
        private readonly ISharedFunctions _sharedFunctions;

        public TankSearchController(IUtilityService utilityService, ISharedFunctions sharedFunctions)
        {
            _utilityService = utilityService;
            _sharedFunctions = sharedFunctions;
        }
        //
        // GET: /TankSearch/
        public ActionResult Index()
        {
            PopulateSecurityExtended();
            int securityProfileId = SecurityExtended.SecurityProfileId;
            var permissionList = _sharedFunctions.GetSecuritySettings(securityProfileId, (int)SecurityCatEnum.TankSearch, null);
            ViewBag.AccessDispatch = false;
            ViewBag.AllowPrep = false;
            foreach (var permission in permissionList)
            {
                if (permission.PrivilegeDS == "Dispatch")
                {
                    ViewBag.AccessDispatch = (permission.GrantedFL == 1);
                }
                else if (permission.PrivilegeDS == "Prep")
                {
                    ViewBag.AllowPrep = (permission.GrantedFL == 1);
                }
            }

            LoadTankSearchDropdowns();

            TankSearchPostModel postModel;
            var dataTable = new DataTable();
            var TANK_usp_rpt_TankSearch_spParams = new TANK_usp_rpt_TankSearch_spParams();

            if (TempData["postModel"] != null)
            {
                postModel = (TankSearchPostModel)TempData["postModel"];

                TANK_usp_rpt_TankSearch_spParams.LocationID = SecurityExtended.LocationId.Value;
                if (postModel.TankNumber != null && postModel.TankNumber.Trim().Length > 0)
                    TANK_usp_rpt_TankSearch_spParams.EquipmentAN = postModel.TankNumber;
                if (postModel.Chassis != null && postModel.Chassis.Trim().Length > 0)
                    TANK_usp_rpt_TankSearch_spParams.ChassisEquipmentAN = postModel.Chassis;
                if (postModel.Product != null && postModel.Product.Trim().Length > 0)
                    TANK_usp_rpt_TankSearch_spParams.ProductDS = postModel.Product.Trim().Replace("%", "[%]");
                if (postModel.LoadStatus != null && postModel.LoadStatus.Trim().Length > 0)
                    TANK_usp_rpt_TankSearch_spParams.LoadStatusTypeCD = postModel.LoadStatus;
                if (postModel.LocationFromCode != null && postModel.LocationFromCode > 0)
                {
                    TANK_usp_rpt_TankSearch_spParams.FromSubLocationID = postModel.LocationFromCode;
                    TANK_usp_rpt_TankSearch_spParams.FromSubLocationDS = "";
                }
                if (postModel.LocationToCode != null && postModel.LocationToCode > 0)
                {
                    TANK_usp_rpt_TankSearch_spParams.SubLocationID = postModel.LocationToCode;
                    TANK_usp_rpt_TankSearch_spParams.SubLocationDS = "";
                }
                if (postModel.ChargeNbr != null && postModel.ChargeNbr.Trim().Length > 0)
                    TANK_usp_rpt_TankSearch_spParams.ChargeCodeAN = postModel.ChargeNbr;
                if (postModel.ShipmentNbr != null && postModel.ShipmentNbr.Trim().Length > 0)
                    TANK_usp_rpt_TankSearch_spParams.ShipmentAN = postModel.ShipmentNbr;
                if (postModel.ChargeBlockOnHire != null && postModel.ChargeBlockOnHire.Trim().Length > 0)
                    TANK_usp_rpt_TankSearch_spParams.ChargeBlockLocationAN = postModel.ChargeBlockOnHire;
                if (postModel.ServiceType != null && postModel.ServiceType.Trim().Length > 0)
                    TANK_usp_rpt_TankSearch_spParams.ServiceTypeDS = postModel.ServiceType;
                if (postModel.DedicatedProduct != null && postModel.DedicatedProduct.Trim().Length > 0)
                    TANK_usp_rpt_TankSearch_spParams.DedicatedProductDS = postModel.DedicatedProduct;
                if (postModel.DispatchReason != null && postModel.DispatchReason != null)
                    TANK_usp_rpt_TankSearch_spParams.DispatchReasonTypeCD = postModel.DispatchReason;
                if (postModel.chkLastMove != null && postModel.chkLastMove)
                    TANK_usp_rpt_TankSearch_spParams.SearchLastMoveOnlyFL = postModel.chkLastMove;

                // database call
                dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_rpt_TankSearch", TANK_usp_rpt_TankSearch_spParams);

                //# database call
            }

            @ViewBag.TotalRecords = dataTable.Rows.Count;
            return View(dataTable);
        }

        public ActionResult Search(TankSearchPostModel postModel, String strDedicatedProduct)
        {
            postModel.DedicatedProduct = strDedicatedProduct.Trim();
            TempData["postModel"] = postModel;
            return RedirectToAction("Index", "TankSearch");
        }

        private void LoadTankSearchDropdowns()
        {
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

            #region LoadPoint

            var loadItemsPoint = new List<SelectListItem>();

            //todo: re-factor it later
            //aList.Add(GetLocationName() & " Facility")
            loadItemsPoint.Add(new SelectListItem { Text = "Freeport" + " Facility", Value = "1" });
            loadItemsPoint.Add(new SelectListItem { Text = "Over The Road", Value = "2" });
            loadItemsPoint.Add(new SelectListItem { Text = "Block", Value = "3", Selected = true });
            loadItemsPoint.Add(new SelectListItem { Text = "Grounded", Value = "4" });

            ViewBag.LoadPoint = loadItemsPoint;

            #endregion LoadPoint
        }

        //PopulateLoadPoint
        [HttpGet]
        public JsonResult PopulateLoadPoint(string searchTerm, int locationType = 1)
        {
            searchTerm = searchTerm.ToUpper();
            var locations = LoadLocations(locationType);
            locations = locations.Where(l => l.text.ToUpper().Contains(searchTerm)).ToList();

            return Json(locations, JsonRequestBehavior.AllowGet);
        }

        private List<Select2ViewModel> LoadLocations(int locationType)
        {
            PopulateSecurityExtended();
            //todo: re-factor it later as required
            var loadPoints = new List<Select2ViewModel>();

            //selected location
            if (locationType == 1)
            {
                var response = _sharedFunctions.PopulateLoadPointLocationAll(SecurityExtended.LocationId.Value);
                if (response != null && response.Any())
                {
                    foreach (var item in response)
                    {
                        var loadPoint = new Select2ViewModel();
                        loadPoint.id = item.LocationID;
                        loadPoint.text = item.LocationDS;
                        loadPoints.Add(loadPoint);

                    }
                }
            }
            //Over The Road
            else if (locationType == 2)
            {
                var response = _sharedFunctions.PopulateLoadPointLocationTreeFlatOverTheRoad(SecurityExtended.LocationId.Value);
                if (response != null && response.Any())
                {
                    foreach (var item in response)
                    {
                        var loadPoint = new Select2ViewModel();
                        loadPoint.id = item.LocationID;
                        loadPoint.text = item.LocationDS;
                        loadPoints.Add(loadPoint);

                    }
                }
            }
            //Block
            else if (locationType == 3)
            {
                var response = _sharedFunctions.PopulateLoadPointLocationTreeFlatBlock(SecurityExtended.LocationId.Value);
                if (response != null && response.Any())
                {
                    foreach (var item in response)
                    {
                        var loadPoint = new Select2ViewModel();
                        loadPoint.id = item.LocationID;
                        loadPoint.text = item.LocationDS;
                        loadPoints.Add(loadPoint);

                    }
                }
            }
            //Grounded
            else if (locationType == 4)
            {
                var response = _sharedFunctions.PopulateLoadPointLocationGrounded(SecurityExtended.LocationId.Value);
                if (response != null && response.Any())
                {
                    foreach (var item in response)
                    {
                        var loadPoint = new Select2ViewModel();
                        loadPoint.id = item.LocationID;
                        loadPoint.text = item.LocationDS;
                        loadPoints.Add(loadPoint);
                    }
                }
            }
            return loadPoints;
        }

        //PopulateDispatchReasons
        [HttpGet]
        public JsonResult PopulateDispatchReasons(string searchTerm)
        {
            searchTerm = searchTerm.ToUpper();
            var response = _sharedFunctions.PopulateDispatchReasons(false);
            var DispatchReasons = new List<Select2ShortViewModel>();
            response = response.Where(r => r.DispatchReasonTypeDS.ToUpper().Contains(searchTerm)).ToList();
            if (response != null && response.Any())
            {
                foreach (var item in response)
                {
                    var DispatchReason = new Select2ShortViewModel();
                    DispatchReason.id = item.DispatchReasonTypeCD;
                    DispatchReason.text = item.DispatchReasonTypeDS;
                    DispatchReasons.Add(DispatchReason);
                }
            }
            return Json(DispatchReasons, JsonRequestBehavior.AllowGet);
        }

        //PopulateServiceType
        [HttpGet]
        public JsonResult PopulateServiceType(string searchTerm)
        {
            searchTerm = searchTerm.ToUpper();
            var response = _sharedFunctions.PopulateServiceType(false);
            response = response.Where(r => r.ServiceTypeDS.ToUpper().Contains(searchTerm)).ToList();
            var ServiceTypes = new List<SelectServiceTypeModel>();
            if (response != null && response.Any())
            {
                foreach (var item in response)
                {
                    var ServiceType = new SelectServiceTypeModel();
                    ServiceType.id = item.ServiceTypeCD;
                    ServiceType.text = item.ServiceTypeDS;
                    ServiceTypes.Add(ServiceType);
                }
            }
            return Json(ServiceTypes, JsonRequestBehavior.AllowGet);
        }

        //PopulateProducts
        [HttpGet]
        public JsonResult PopulateProducts(string searchTerm)
        {
            searchTerm = searchTerm.ToUpper();
            var response = _sharedFunctions.PopulateProduct(false, 1, searchTerm.Trim().Replace("%", "[%]"));
            response = response.Where(r => r.ProductDS.ToUpper().Contains(searchTerm)).ToList();
            var Products = new List<Select2ShortViewModel>();
            if (response != null && response.Any())
            {
                foreach (var item in response)
                {
                    var Product = new Select2ShortViewModel();
                    Product.id = item.ProductID;
                    Product.text = item.ProductDS;
                    Products.Add(Product);
                }
            }
            return Json(Products, JsonRequestBehavior.AllowGet);
        }
    }

    public class TankSearchPostModel
    {
        public string TankNumber { get; set; }
        public string Chassis { get; set; }
        public string Product { get; set; }
        public string LoadStatus { get; set; }
        public Int32? LocationFromCode { get; set; }
        public string LocationFrom { get; set; }
        public Int32? LocationToCode { get; set; }
        public string LocationTo { get; set; }
        public string ChargeNbr { get; set; }
        public string ShipmentNbr { get; set; }
        public string ChargeBlockOnHire { get; set; }
        public string ServiceType { get; set; }
        public string DedicatedProduct { get; set; }
        public Int16? DispatchReason { get; set; }
        public Boolean chkLastMove { get; set; }
    }
}