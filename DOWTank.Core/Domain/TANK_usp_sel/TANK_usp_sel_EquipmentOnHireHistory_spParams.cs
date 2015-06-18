using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOWTank.Core.Domain.TANK_usp_rpt
{
    public class TANK_usp_sel_EquipmentOnHireHistory_spParams
    {
        public string EquipmentAn { get; set; }
        public int? OnHireHistoryID { get; set; }
        public int? LocationID { get; set; }
    }

    public class TANK_usp_sel_EquipmentOnHireHistory_spResults
    {
        public int? OnHireHistoryID { get; set; }
        public int? EquipmentID { get; set; }
        public string EquipmentAN { get; set; }
        public string CheckDigitAN { get; set; }
        public int? ChargeCodeID { get; set; }
        public string ChargeCodeAn { get; set; }
        public string ChargeCode { get; set; }
        public string ShipmentAn { get; set; }
        public string StatusDt { get; set; }
        public string StatusTime { get; set; }
        public bool OnHireFl { get; set; }
        public int? OnHireReasonTypeCD { get; set; }

    }
}
