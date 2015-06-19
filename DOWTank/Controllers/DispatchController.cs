using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOWTank.Common;
using DOWTank.Core.Domain.TANK_usp_insupd;
using DOWTank.Core.Domain.TANK_usp_sel;
using DOWTank.Core.Enum;
using DOWTank.Core.Service;
using DOWTank.Custom;
using DOWTank.Models;

namespace DOWTank.Controllers
{
    [ClaimsAuthorize(Roles = "Dispatch")]
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
        public ActionResult Index(string equipmentAn, int? dispatchId, string mode)
        {

            PopulateSecurityExtended();
            int securityProfileId = SecurityExtended.SecurityProfileId;
            var permissionList = _sharedFunctions.GetSecuritySettings(securityProfileId, (int)SecurityCatEnum.DispatchScreen, null);
            ViewBag.AllowDeleteDispatch = false;
            ViewBag.AllowAddProduct = false;
            ViewBag.AllowAddInvoice = false;
            foreach (var permission in permissionList)
            {
                if (permission.PrivilegeDS == "Delete Dispatch")
                {
                    ViewBag.AllowDeleteDispatch = (permission.GrantedFL == 1);
                }
                else if (permission.PrivilegeDS == "Add Product")
                {
                    ViewBag.AllowAddProduct = (permission.GrantedFL == 1);
                }
                else if (permission.PrivilegeDS == "Add Invoice")
                {
                    ViewBag.AllowAddInvoice = (permission.GrantedFL == 1);
                }
            }

            LoadDispatchTankDropdowns();
            ViewBag.EquipmentAN = equipmentAn;
            var postModel = new DispatchTankModel();
            postModel.intDispatchId = dispatchId;
            if (dispatchId.HasValue)
            {
                LoadDispatch(dispatchId.Value, postModel);
            }

            return View(postModel);
        }

        private void LoadDispatch(int dispatchId, DispatchTankModel postModel)
        {
            postModel.LoadDispatchData = 1;
            // database call

            var TANK_usp_sel_Dispatch_spParams = new TANK_usp_sel_Dispatch_spParams()
            {
                DispatchID = dispatchId
            };
            DataTable dataTable = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_sel_Dispatch", TANK_usp_sel_Dispatch_spParams);
            postModel.intDispatchId = dispatchId;
            postModel.EquipmentAN = dataTable.Rows[0]["EquipmentAN"].ToString();
            postModel.EquipmentID = dataTable.Rows[0]["EquipmentId"] != null ? (int)dataTable.Rows[0]["EquipmentId"] : 0;
            postModel.ChargeCodeAN = dataTable.Rows[0]["ChargeCodeAN"].ToString();
            postModel.ChargeCodeID = dataTable.Rows[0]["ChargeCodeID"] != null ? (int)dataTable.Rows[0]["EquipmentId"] : 0;
            postModel.LoadStatusTypeCD = dataTable.Rows[0]["LoadStatusTypeCD"] != null ? (Int16?)dataTable.Rows[0]["LoadStatusTypeCD"] : 0;
            postModel.ChargeBlockLocationID = !string.IsNullOrEmpty(dataTable.Rows[0]["ChargeBlockLocationID"].ToString()) ? (Int32?)dataTable.Rows[0]["ChargeBlockLocationID"] : 0;
            postModel.ChargeBlockLocationDS = dataTable.Rows[0]["ChargeBlockLocationDS"].ToString();
            postModel.bolIsReloadFL = (Boolean)dataTable.Rows[0]["ReloadFL"];
            postModel.WasteClassTypeCD = !string.IsNullOrEmpty(dataTable.Rows[0]["WasteClassTypeCD"].ToString()) ? (Int32?)dataTable.Rows[0]["WasteClassTypeCD"] : 0;
            postModel.WasteClassTypeDS = dataTable.Rows[0]["WasteClassTypeDS"].ToString();
            postModel.DispatchReasonTypeCD = !string.IsNullOrEmpty(dataTable.Rows[0]["DispatchReasonTypeCD"].ToString()) ? (Int16?)dataTable.Rows[0]["DispatchReasonTypeCD"] : 0;
            postModel.DispatchReasonTypeDS = dataTable.Rows[0]["DispatchReasonTypeDS"].ToString();
            postModel.AdditionalDispatchReasonTypeCD = !string.IsNullOrEmpty(dataTable.Rows[0]["AdditionalDispatchReasonTypeCD"].ToString()) ? (Int16?)dataTable.Rows[0]["AdditionalDispatchReasonTypeCD"] : 0;
            postModel.AdditionalDispatchReasonTypeDS = dataTable.Rows[0]["AdditionalDispatchReasonTypeDS"].ToString();
            postModel.strProNumberAN = dataTable.Rows[0]["ProNumberAN"].ToString();
            postModel.dblCallOutHoursAMT = !string.IsNullOrEmpty(dataTable.Rows[0]["CallOutHoursAMT"].ToString()) ? (decimal?)dataTable.Rows[0]["CallOutHoursAMT"] : null;
            postModel.sintCraneLiftAmt = !string.IsNullOrEmpty(dataTable.Rows[0]["CraneLiftAmt"].ToString()) ? (Int16?)dataTable.Rows[0]["CraneLiftAmt"] : null;

            //dispatch start date
            postModel.dtmDispatchStart = !string.IsNullOrEmpty(dataTable.Rows[0]["DispatchDt"].ToString())
                                             ? (DateTime?)Convert.ToDateTime(dataTable.Rows[0]["DispatchDt"])
                                             : null;
            var start = !string.IsNullOrEmpty(dataTable.Rows[0]["Start"].ToString())
                                             ? (TimeSpan?)TimeSpan.Parse(dataTable.Rows[0]["Start"].ToString())
                                             : null;
            if (postModel.dtmDispatchStart.HasValue && start.HasValue)
            {
                postModel.dtmDispatchStart = postModel.dtmDispatchStart.Value.Add(start.Value);
            }

            //disptach end date
            postModel.dtmDispatchEnd = !string.IsNullOrEmpty(dataTable.Rows[0]["DeliveryDt"].ToString())
                                             ? (DateTime?)Convert.ToDateTime(dataTable.Rows[0]["DeliveryDt"])
                                             : null;
            var end = !string.IsNullOrEmpty(dataTable.Rows[0]["End"].ToString())
                                             ? (TimeSpan?)TimeSpan.Parse(dataTable.Rows[0]["End"].ToString())
                                             : null;
            if (postModel.dtmDispatchEnd.HasValue && end.HasValue)
            {
                postModel.dtmDispatchEnd = postModel.dtmDispatchEnd.Value.Add(end.Value);
            }

            //dtmScheduledDelivery
            postModel.dtmScheduledDelivery = !string.IsNullOrEmpty(dataTable.Rows[0]["ScheduledDeliveryDT"].ToString())
                                             ? (DateTime?)Convert.ToDateTime(dataTable.Rows[0]["ScheduledDeliveryDT"])
                                             : null;
            var deliveryTime = !string.IsNullOrEmpty(dataTable.Rows[0]["ScheduledDeliveryTime"].ToString())
                                             ? (TimeSpan?)TimeSpan.Parse(dataTable.Rows[0]["ScheduledDeliveryTime"].ToString())
                                             : null;
            if (postModel.dtmScheduledDelivery.HasValue && deliveryTime.HasValue)
            {
                postModel.dtmScheduledDelivery = postModel.dtmScheduledDelivery.Value.Add(deliveryTime.Value);
            }

            postModel.strShipmentAN = dataTable.Rows[0]["ShipmentAN"].ToString();
            postModel.FittingCD = !string.IsNullOrEmpty(dataTable.Rows[0]["FittingCD"].ToString()) ? (int?)dataTable.Rows[0]["FittingCD"] : null;
            postModel.ContactID = postModel.FittingCD = !string.IsNullOrEmpty(dataTable.Rows[0]["ContactID"].ToString()) ? (int?)dataTable.Rows[0]["ContactID"] : null;
            postModel.Contact = dataTable.Rows[0]["Contact"].ToString();
            postModel.strComments = dataTable.Rows[0]["CommentsAN"].ToString();


            //# database call   
        }

        [HttpPost]
        public ActionResult Index(DispatchTankModel postModel, String ChassisID, String ProductID, String DriverID, String LoadStatusTypeCD)
        {
            PopulateSecurityExtended();
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
            objDispatchTankParams.LocationID = SecurityExtended.LocationId.Value;
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
            objDispatchTankParams.UpdateUserAN = SecurityExtended.UserName;

            _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_Dispatch", objDispatchTankParams);
            Success("Dispatch Tank Saved Successfully.");
            //return appropriate message
            return View(postModel);
        }

        #region Delete Dispatch

        [HttpPost]
        public JsonResult DeleteDispatchData(DeleteDispatchPostModel postModel)
        {
            PopulateSecurityExtended();
            if (postModel == null || postModel.DispatchID == null || postModel.EquipmentID == 0)
            {
                return Json(0);
            }
            TANK_usp_insupd_DispatchTank_spParams objDispatchTankParams = new TANK_usp_insupd_DispatchTank_spParams()
                {
                    LocationID = SecurityExtended.LocationId.Value,
                    DispatchID = postModel.DispatchID,
                    EquipmentID = postModel.EquipmentID,
                    ActiveFL = false,
                    UpdateUserAN = "System"
                };

            _utilityService.ExecStoredProcedureWithoutResults("TANK_usp_insupd_Dispatch", objDispatchTankParams);

            Success("Dispatch Tank Deleted Successfully.");
            return Json(1);
        }

        public class DeleteDispatchPostModel
        {
            public int? DispatchID { get; set; }
            public int EquipmentID { get; set; }
        }

        #endregion Delete Dispatch

        [HttpGet]
        public JsonResult LoadLastMove(int? equipmentId)
        {
            PopulateSecurityExtended();
            if (equipmentId == null || equipmentId.Value == 0)
            {
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
            var data = _sharedFunctions.LoadDispatchLastMove(equipmentId.Value, SecurityExtended.LocationId.Value);
            if (data != null && data.Any())
            {
                var result = data.FirstOrDefault();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        private void LoadDispatchTankDropdowns()
        {
            PopulateSecurityExtended();

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
            var response = _sharedFunctions.PopulateFitting(SecurityExtended.LocationId.Value);
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

        #region select2

        //PopulateEquipment
        [HttpGet]
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
        public JsonResult PopulateProduct(string searchTerm)
        {
            PopulateSecurityExtended();
            //todo: re-factor it later as required
            var response = _sharedFunctions.PopulateProduct(false, SecurityExtended.LocationId.Value, searchTerm.Trim());
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
        public JsonResult PopulateDriver(string searchTerm)
        {
            searchTerm = searchTerm.ToUpper();
            var response = _sharedFunctions.PopulateDrivers(false).ToList();
            response = response.Where(d => !d.Driver.StartsWith(" ")).ToList();
            var DriverList = new List<Select2ViewModel>();
            if (response != null && response.Any())
            {
                //foreach (var item in response.ToList())
                foreach (var item in response)
                {
                    var Driver = new Select2ViewModel();
                    Driver.id = item.DriverID;
                    Driver.text = item.Driver;
                    var fullName = item.Driver.Split(',');
                    if (fullName[0].ToUpper().Contains(searchTerm) || fullName[1].ToUpper().Contains(searchTerm))
                    { DriverList.Add(Driver); }
                }
            }
            return Json(DriverList, JsonRequestBehavior.AllowGet);
        }

        //PopulateLoadPoint
        [HttpGet]
        public JsonResult PopulateLoadPoint(string searchTerm, int locationType = 1)
        {
            //todo: re-factor it later as required
            var locations = LoadLocations(locationType);
            return Json(locations, JsonRequestBehavior.AllowGet);
        }

        //PopulateChargeCode
        [HttpGet]
        public JsonResult PopulateChargeCode(string searchTerm)
        {
            PopulateSecurityExtended();
            //todo: re-factor it later as required
            var response = _sharedFunctions.PopulateChargeCode(0, searchTerm.Trim(), SecurityExtended.LocationId.Value);
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
        public JsonResult PopulateContacts(string searchTerm)
        {
            PopulateSecurityExtended();
            var response = _sharedFunctions.PopulateContacts(SecurityExtended.LocationId.Value);

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
            PopulateSecurityExtended();
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

        #endregion select2
    }
}