using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOWTank.Core.Domain.TANK_usp_insupd
{
    public class TANK_usp_insupd_EquipmentOnHireHistory_spParams
    {
        public TANK_usp_insupd_EquipmentOnHireHistory_spParams()
        {
            InstallID = 1;
            StatusDt = DateTime.Now;
        }
        public int InstallID { get; set; }
        public int? OnHireHistoryID { get; set; }
        public int LocationID { get; set; }
        public string EquipmentAN { get; set; }
        public string ChargeCodeAn { get; set; }
        public string ShipmentAN { get; set; }
        public bool OnHireFL { get; set; }
        public int? OnHireReasonTypeCD { get; set; }
        public DateTime StatusDt { get; set; }
        public string UpdateUserAn { get; set; }
    }
}
