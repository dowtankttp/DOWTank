using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOWTank.Common;
using DOWTank.Core.Service;
using DOWTank.Models;

namespace DOWTank.Controllers
{
    public class DispatchController : Controller
    {
        private readonly IUtilityService _utilityService;
        private readonly ISharedFunctions _sharedFunctions;
        public DispatchController(IUtilityService utilityService, ISharedFunctions sharedFunctions)
        {
            _utilityService = utilityService;
            _sharedFunctions = sharedFunctions;
        }

        // GET: Dispatch
        public ActionResult Index()
        {
            LoadDispatchTankDropdowns();
            return View();
        }

        [HttpPost]
        public ActionResult Index(DispatchTankModel postModel)
        {
            if (!ModelState.IsValid)
            {
                //return appropriate validation messages
                return View(postModel);
            }
            return View(postModel);
        }

        [HttpGet]
        public JsonResult LoadLastMove(String strTankNumber)
        {
            var data = _sharedFunctions.LoadDispatchLastMove(strTankNumber, 1);
            if (data != null && data.Any())
            {
                var result = data.FirstOrDefault();
                return Json(new { ChargeBlockLocationDS = result.ChargeBlockLocationDS, ChargeCodeAN = result.ChargeCodeAN, ChassisEquipmentID = result.ChassisEquipmentID, ChassisEquipmentAN = result.ChassisEquipmentAN, DispatchStartDt = result.DispatchStartDt, Driver = result.Driver, DriverID = result.DriverID, ProductID = result.ProductID, ProductDS = result.ProductDS, ToLocationDS = result.ToLocationDS, FittingDS = result.FittingDS, ShipmentAN = result.ShipmentAN, ContactNM = result.ContactNM, ContactID = result.ContactID }, JsonRequestBehavior.AllowGet);
            }
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        private void LoadDispatchTankDropdowns()
        {
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
                ViewBag.FittingCD = fittingList;
            }

            #endregion fitting ddl

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
    }
}