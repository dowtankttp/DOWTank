using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DOWTank.Models
{
    public class EquipmentOnHireHistoryPostModel
    {
        //for add/update
        public int? OnHireHistoryID { get; set; }
        [Required(ErrorMessage = "Tank is required.")]
        [StringLength(10)]
        public string EquipmentAN { get; set; }
        [Required(ErrorMessage = "Charge Code is required")]
        [StringLength(20)]
        public string ChargeCodeAn { get; set; }
        [StringLength(20)]
        public string ShipmentAN { get; set; }
        public bool OnHireFL { get; set; }
        [Required(ErrorMessage = "Reason is required.")]
        public int? OnHireReasonTypeCD { get; set; }
        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.DateTime)]
        public DateTime? StatusDt { get; set; }
        public int? ChargeCodeID { get; set; }
        public int? OnHireReasonTypeCDEdit { get; set; }
    }
}