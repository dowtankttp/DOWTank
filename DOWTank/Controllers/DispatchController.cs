using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOWTank.Common;
using DOWTank.Core.Domain.TANK_usp_insupd;
using DOWTank.Core.Service;
using DOWTank.Models;

namespace DOWTank.Controllers
{
    public class DispatchController : BaseController
    {
        private readonly IUtilityService _utilityService;
        private readonly ISharedFunctions _sharedFunctions;
        public DispatchController(IUtilityService utilityService, ISharedFunctions sharedFunctions)
        {
            _utilityService = utilityService;
            _sharedFunctions = sharedFunctions;
        }

        // GET: Dispatch
        public ActionResult Index(string equipmentAn)
        {
            LoadDispatchTankDropdowns();
            ViewBag.EquipmentAN = equipmentAn;
            var postModel = new DispatchTankModel();
            return View(postModel);
        }

        [HttpPost]
        public ActionResult Index(DispatchTankModel postModel, String ChassisID, String ProductID, String DriverID, String LoadStatusTypeCD)
        {
            LoadDispatchTankDropdowns();
            if (!ModelState.IsValid)
            {
                //return appropriate validation messages
                return View(postModel);
            }
            //todo: re-factor it as required
            TANK_usp_insupd_DispatchTank_spParams objDispatchTankParams = new TANK_usp_insupd_DispatchTank_spParams();
            if (postModel.intDispatchId != null && postModel.intDispatchId > 0)
                objDispatchTankParams.DispatchID = postModel.intDispatchId;
            objDispatchTankParams.LocationID = 1;
            objDispatchTankParams.EquipmentID = postModel.intEquipmentId;
            if (ChassisID.Trim().Length > 0)
                objDispatchTankParams.ChassisEquipmentID = Convert.ToInt32(ChassisID);
            if (postModel.sintLoadStatusTypeId != null && postModel.sintLoadStatusTypeId > 0)
                objDispatchTankParams.LoadStatusTypeCD = postModel.sintLoadStatusTypeId;
            if (ProductID.Trim().Length > 0)
                objDispatchTankParams.ProductID = Convert.ToInt16(ProductID);
            if (postModel.sintDispatchReasonTypeId != null && postModel.sintDispatchReasonTypeId > 0)
                objDispatchTankParams.DispatchReasonTypeCd = postModel.sintDispatchReasonTypeId;
            if (postModel.sintAdditionalDispatchReasonTypeId != null && postModel.sintAdditionalDispatchReasonTypeId > 0)
                objDispatchTankParams.AdditionalDispatchReasonTypeCD = postModel.sintAdditionalDispatchReasonTypeId;
            objDispatchTankParams.FromLocationID = postModel.intLocationFrom;
            objDispatchTankParams.ToLocationID = postModel.intLocationTo;
            objDispatchTankParams.ShipmentAN = postModel.strShipmentAN;
            if (DriverID.Trim().Length > 0)
                objDispatchTankParams.DriverID = Convert.ToInt32(DriverID);
            if (postModel.dtmDispatchStart != null && postModel.dtmDispatchStart.Value != DateTime.MinValue)
                objDispatchTankParams.DispatchStartDT = postModel.dtmDispatchStart;
            if (postModel.dtmDispatchEnd != null && postModel.dtmDispatchEnd.Value != DateTime.MinValue)
                objDispatchTankParams.DispatchEndDT = postModel.dtmDispatchEnd;
            if (postModel.dtmScheduledDelivery != null && postModel.dtmScheduledDelivery.Value != DateTime.MinValue)
                objDispatchTankParams.ScheduledDeliveryDT = postModel.dtmScheduledDelivery;
            if (postModel.intContactId != null && postModel.intContactId > 0)
                objDispatchTankParams.ContactID = postModel.intContactId;
            if (postModel.intChargeCode != null && postModel.intChargeCode > 0)
                objDispatchTankParams.ChargeCodeID = postModel.intChargeCode;
            if (postModel.intChargeBlockLocationId != null && postModel.intChargeBlockLocationId > 0)
                objDispatchTankParams.ChargeBlockLocationID = postModel.intChargeBlockLocationId;
            if (postModel.sintCraneLiftAmt != null && postModel.sintCraneLiftAmt > 0)
                objDispatchTankParams.CraneLiftAmt = postModel.sintCraneLiftAmt;
            if (postModel.intFittingId != null && postModel.intFittingId > 0)
                objDispatchTankParams.FittingCD = postModel.intFittingId;
            objDispatchTankParams.PlannedFL = postModel.bolIsPlannedFL;
            objDispatchTankParams.ProNumberAN = postModel.strProNumberAN;
            objDispatchTankParams.CommentsAn = postModel.strComments;
            if (postModel.dblCallOutHoursAMT != null && postModel.dblCallOutHoursAMT > 0)
                objDispatchTankParams.CallOutHoursAMT = postModel.dblCallOutHoursAMT.Value;
            if (postModel.intWasteClassTypeId != null && postModel.intWasteClassTypeId > 0)
                objDispatchTankParams.WasteClassTypeCD = postModel.intWasteClassTypeId;
            objDispatchTankParams.ReloadFL = postModel.bolIsReloadFL;
            objDispatchTankParams.CleaningApprovedFL = postModel.bolIsCleaningApprovedFL;
            objDispatchTankParams.WPNAN = postModel.strWPNAN;
            objDispatchTankParams.UpdateUserAN = "System";

            _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_Dispatch", objDispatchTankParams);
            Success("Dispatch Tank Saved Successfully.");
            //return appropriate message
            return View(postModel);
        }

        [HttpGet]
        public JsonResult LoadLastMove(int? equipmentId)
        {
            if (equipmentId == null || equipmentId.Value == 0)
            {
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
            var data = _sharedFunctions.LoadDispatchLastMove(equipmentId.Value, 1);
            if (data != null && data.Any())
            {
                var result = data.FirstOrDefault();
                return Json(new { ChargeBlockLocationDS = result.ChargeBlockLocationDS, ChargeCodeAN = result.ChargeCodeAN, ChassisEquipmentID = result.ChassisEquipmentID, ChassisEquipmentAN = result.ChassisEquipmentAN, DispatchStartDt = result.DispatchStartDt, Driver = result.Driver, DriverID = result.DriverID, ProductID = result.ProductID, ProductDS = result.ProductDS, ToLocationDS = result.ToLocationDS, FittingDS = result.FittingDS, ShipmentAN = result.ShipmentAN, ContactNM = result.ContactNM, ContactID = result.ContactID }, JsonRequestBehavior.AllowGet);
            }
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        private void LoadDispatchTankDropdowns()
        {
            #region LoadPoint

            var loadItemsPoint = new List<SelectListItem>();

            //todo: re-factor it later
            //aList.Add(GetLocationName() & " Facility")
            loadItemsPoint.Add(new SelectListItem { Text = "Freeport" + " Facility", Value = "1" });
            loadItemsPoint.Add(new SelectListItem { Text = "Over The Road", Value = "2" });
            loadItemsPoint.Add(new SelectListItem { Text = "Block", Value = "3", Selected = true });
            loadItemsPoint.Add(new SelectListItem { Text = "Grounded", Value = "4" });

            ViewBag.LoadPoint = loadItemsPoint;
            ViewBag.DeliveryPoint = loadItemsPoint;
            ViewBag.OnHireBlockPoint = loadItemsPoint;

            #endregion LoadPoint

            #region PopulateTankStatus

            var statusTypeList = new List<SelectListItem>();
            var responseStustType = _sharedFunctions.PopulateLoadStatusType();
            if (responseStustType != null && responseStustType.Any())
            {
                statusTypeList.Add(new SelectListItem { Text = "", Value = "" });
                foreach (var item in responseStustType)
                {
                    statusTypeList.Add(new SelectListItem { Text = item.LoadStatusTypeDS, Value = item.LoadStatusTypeCD.ToString() });
                }
                ViewBag.sintLoadStatusTypeId = statusTypeList;
            }

            #endregion PopulateLoadStatusType

            #region fitting ddl

            var fittingList = new List<SelectListItem>();
            var response = _sharedFunctions.PopulateFitting();
            if (response != null && response.Any())
            {
                fittingList.Add(new SelectListItem { Text = "", Value = "" });
                foreach (var item in response)
                {
                    fittingList.Add(new SelectListItem { Text = item.FittingDS, Value = item.FittingCD.HasValue ? item.FittingCD.Value.ToString() : string.Empty });
                }
                ViewBag.intFittingId = fittingList;
            }

            #endregion fitting ddl
        }

        //PopulateEquipment
        [HttpGet]
        [OutputCache(Duration = int.MaxValue, VaryByParam = "searchTerm")]
        public JsonResult PopulateDispatchReason(string searchTerm)
        {
            searchTerm = searchTerm.Trim();
            //todo: re-factor it later as required
            var response = _sharedFunctions.PopulateEquipment(1, searchTerm);

            var productList = new List<Select2ViewModel>();
            if (response != null && response.Any())
            {
                foreach (var item in response)
                {
                    var product = new Select2ViewModel();
                    product.id = item.EquipmentID;
                    product.text = item.EquipmentAn + " " + item.EquipmentTypeDS;
                    productList.Add(product);
                }
            }
            return Json(productList, JsonRequestBehavior.AllowGet);
        }

        //PopulateChassis
        [HttpGet]
        [OutputCache(Duration = int.MaxValue, VaryByParam = "searchTerm")]
        public JsonResult PopulateChassis(string searchTerm)
        {
            //todo: re-factor it later as required
            var response = _sharedFunctions.PopulateEquipment(2, searchTerm.Trim());

            var productList = new List<Select2ViewModel>();
            if (response != null && response.Any())
            {
                foreach (var item in response)
                {
                    var product = new Select2ViewModel();
                    product.id = item.EquipmentID;
                    product.text = item.EquipmentAn + " " + item.EquipmentTypeDS;
                    productList.Add(product);
                }
            }
            return Json(productList, JsonRequestBehavior.AllowGet);
        }

        //PopulateProduct
        [HttpGet]
        [OutputCache(Duration = int.MaxValue, VaryByParam = "searchTerm")]
        public JsonResult PopulateProduct(string searchTerm)
        {
            //todo: re-factor it later as required
            var response = _sharedFunctions.PopulateProduct(false, 1, searchTerm.Trim());
            var productList = new List<Select2ViewModel>();
            if (response != null && response.Any())
            {
                foreach (var item in response)
                {
                    var product = new Select2ViewModel();
                    product.id = item.ProductID;
                    product.text = item.ProductDS;
                    productList.Add(product);
                }
            }
            return Json(productList, JsonRequestBehavior.AllowGet);
        }

        //PopulateDriver
        [HttpGet]
        [OutputCache(Duration = int.MaxValue, VaryByParam = "searchTerm")]
        public JsonResult PopulateDriver(string searchTerm)
        {
            //todo: re-factor it later as required
            var response = _sharedFunctions.PopulateDrivers(false);
            var DriverList = new List<Select2ViewModel>();
            if (response != null && response.Any())
            {
                foreach (var item in response)
                {
                    var Driver = new Select2ViewModel();
                    Driver.id = item.DriverID;
                    Driver.text = item.DriverNM;
                    DriverList.Add(Driver);
                }
            }
            return Json(DriverList, JsonRequestBehavior.AllowGet);
        }

        //PopulateLoadPoint
        [HttpGet]
        [OutputCache(Duration = int.MaxValue, VaryByParam = "locationType")]
        public JsonResult PopulateLoadPoint(string searchTerm, int locationType = 1)
        {
            //todo: re-factor it later as required
            var locations = LoadLocations(locationType);
            return Json(locations, JsonRequestBehavior.AllowGet);
        }

        //PopulateChargeCode
        [HttpGet]
        [OutputCache(Duration = int.MaxValue, VaryByParam = "searchTerm")]
        public JsonResult PopulateChargeCode(string searchTerm)
        {
            //todo: re-factor it later as required
            var response = _sharedFunctions.PopulateChargeCode(0, searchTerm.Trim());
            var chargeCodes = new List<Select2ViewModel>();
            if (response != null && response.Any())
            {
                foreach (var item in response)
                {
                    var chargeCode = new Select2ViewModel();
                    if (item.ChargeCodeID.HasValue)
                    {
                        chargeCode.id = item.ChargeCodeID.Value;
                        chargeCode.text = item.ChargeCodeAN;
                        chargeCodes.Add(chargeCode);
                    }
                }
            }
            return Json(chargeCodes, JsonRequestBehavior.AllowGet);
        }

        //PopulateWasteClass
        [HttpGet]
        [OutputCache(Duration = int.MaxValue, VaryByParam = "searchTerm")]
        public JsonResult PopulateWasteClass(string searchTerm)
        {
            //todo: re-factor it later as required
            var response = _sharedFunctions.PopulateWasteClassTypes(false);
            var WasteClassList = new List<Select2ViewModel>();
            if (response != null && response.Any())
            {
                foreach (var item in response)
                {
                    var WasteClass = new Select2ViewModel();
                    WasteClass.id = item.WasteClassTypeCd;
                    WasteClass.text = item.WasteClassTypeDS;
                    WasteClassList.Add(WasteClass);
                }
            }
            return Json(WasteClassList, JsonRequestBehavior.AllowGet);
        }

        //PopulateDispatchReasons
        [HttpGet]
        [OutputCache(Duration = int.MaxValue, VaryByParam = "searchTerm")]
        public JsonResult PopulateDispatchReasons(string searchTerm)
        {
            //todo: re-factor it later as required
            var response = _sharedFunctions.PopulateDispatchReasons(false);

            var DispatchReasons = new List<Select2ShortViewModel>();
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

        //PopulateContacts
        [HttpGet]
        [OutputCache(Duration = int.MaxValue, VaryByParam = "searchTerm")]
        public JsonResult PopulateContacts(string searchTerm)
        {
            //todo: re-factor it later as required
            var response = _sharedFunctions.PopulateContacts();

            var productList = new List<Select2ViewModel>();
            if (response != null && response.Any())
            {
                foreach (var item in response)
                {
                    var product = new Select2ViewModel();
                    product.id = item.ContactID;
                    product.text = item.Contact;
                    productList.Add(product);
                }
            }
            return Json(productList, JsonRequestBehavior.AllowGet);
        }

        private List<Select2ViewModel> LoadLocations(int locationType)
        {
            //todo: re-factor it later as required
            var loadPoints = new List<Select2ViewModel>();

            //selected location
            if (locationType == 1)
            {
                var response = _sharedFunctions.PopulateLoadPointLocationAll(1);
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
                var response = _sharedFunctions.PopulateLoadPointLocationTreeFlatOverTheRoad(1);
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
                var response = _sharedFunctions.PopulateLoadPointLocationTreeFlatBlock(1);
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
                var response = _sharedFunctions.PopulateLoadPointLocationGrounded(1);
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
    }
}