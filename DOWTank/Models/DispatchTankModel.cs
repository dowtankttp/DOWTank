using System;
using System.ComponentModel.DataAnnotations;

namespace DOWTank.Models
{
    public class DispatchTankModel
    {
        public DispatchTankModel()
        {
            LoadDispatchData = 0;
        }
        public Int32? intDispatchId { get; set; }
        [Required(ErrorMessage = "Tank Number is required")]
        public Int32 intEquipmentId { get; set; }
        public Int32? intChassisEquipmentId { get; set; }
        [Required(ErrorMessage = "Tank Status is required")]
        public Int16? sintLoadStatusTypeId { get; set; }
        public Int16? sintProductId { get; set; }
        public Int16? sintDispatchReasonTypeId { get; set; }
        public Int16? sintAdditionalDispatchReasonTypeId { get; set; }
        [Required(ErrorMessage = "Loading Location is required")]
        public Int32 intLocationFrom { get; set; }
        [Required(ErrorMessage = "Delivery Location is required")]
        public Int32 intLocationTo { get; set; }
        [Required(ErrorMessage = "Shipment Number is required")]
        [StringLength(12, ErrorMessage = "Shipment Number must not exceed 12 characters")]
        [RegularExpression(@"\d{7}", ErrorMessage = "Shipment Number must be 7 digits")]
        public String strShipmentAN { get; set; }
        public Int32? intDriverId { get; set; }
        [Required(ErrorMessage = "Dispatch Start DateTime is required")]
        public DateTime? dtmDispatchStart { get; set; }
        [Required(ErrorMessage = "Dispatch End DateTime is required")]
        public DateTime? dtmDispatchEnd { get; set; }
        public DateTime? dtmScheduledDelivery { get; set; }
        [Required(ErrorMessage = "Block Contact is required")]
        public Int32? intContactId { get; set; }
        [Required(ErrorMessage = "Charge Code is required")]
        public Int32? intChargeCode { get; set; }
        public Int32? intChargeBlockLocationId { get; set; }
        public Int16? sintCraneLiftAmt { get; set; }
        public Int32? intFittingId { get; set; }
        public Boolean bolIsPlannedFL { get; set; }
        [Required(ErrorMessage = "Pro Number is required")]
        public String strProNumberAN { get; set; }
        [StringLength(7000, ErrorMessage = "Comments must not exceed 7000 characters")]
        public String strComments { get; set; }
        public Int32 intLocationId { get; set; }
        public decimal? dblCallOutHoursAMT { get; set; }
        public Int32? intWasteClassTypeId { get; set; }
        public Boolean bolIsReloadFL { get; set; }
        public Boolean bolIsCleaningApprovedFL { get; set; }
        public String strWPNAN { get; set; }
        //load dispatch data
        public int LoadDispatchData { get; set; }
        public int EquipmentID { get; set; }
        public string EquipmentAN { get; set; }
        public string txtLMDate { get; set; }
        public int? ChargeCodeID { get; set; }
        public string ChargeCodeAN { get; set; }
        public Int16? LoadStatusTypeCD { get; set; }
        public Int32? ChargeBlockLocationID { get; set; }
        public string ChargeBlockLocationDS { get; set; }
        public int? WasteClassTypeCD { get; set; }
        public string WasteClassTypeDS { get; set; }
        public Int16? DispatchReasonTypeCD { get; set; }
        public string DispatchReasonTypeDS { get; set; }
        public Int16? AdditionalDispatchReasonTypeCD { get; set; }
        public string AdditionalDispatchReasonTypeDS { get; set; }
        public int? FittingCD { get; set; }
        public int? ContactID { get; set; }
        public string Contact { get; set; }
        
        //persist data after post
        //ddlChassis
        public int? ddlChassisId { get; set; }
        public string ddlChassisText { get; set; }
        //ddlProduct
        public int? ddlProductId { get; set; }
        public string ddlProductText { get; set; }
        //ddlDriver
        public int? ddlDriverId { get; set; }
        public string ddlDriverText { get; set; }
        //ddlLoadPoint
        public int? ddlLoadPointId { get; set; }
        public string ddlLoadPointText { get; set; }
        //ddlChargeCode
        public int? ddlChargeCodeId { get; set; }
        public string ddlChargeCodeText { get; set; }
        //ddlDeliveryLocation
        public int? ddlDeliveryLocationId { get; set; }
        public string ddlDeliveryLocationText { get; set; }
        //ddlOnHireBlock
        public int? ddlOnHireBlockId { get; set; }
        public string ddlOnHireBlockText { get; set; }
        //ddlWasteClass
        public int? ddlWasteClassId { get; set; }
        public string ddlWasteClassText { get; set; }
        //ddlInstructionsReason
        public int? ddlInstructionsReasonId { get; set; }
        public string ddlInstructionsReasonText { get; set; }
        //ddlAdditionalInstruct
        public int? ddlAdditionalInstructId { get; set; }
        public string ddlAdditionalInstructText { get; set; }
        //ddlContact
        public int? ddlContactId { get; set; }
        public string ddlContactText { get; set; }
    }
}