using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOWTank.Core.Domain.TANK_usp_insupd
{
    public class TANK_usp_insupd_DispatchTank_spParams
    {
        public TANK_usp_insupd_DispatchTank_spParams()
        {
            CraneLiftAmt = 0;
            PlannedFL = false;
            CallOutHoursAMT = 0;
            ActiveFL = true;
        }
        public Int32? DispatchID { get; set; }
        public Int32 EquipmentID { get; set; }
        public Int32? ChassisEquipmentID { get; set; }
        public Int16? LoadStatusTypeCD { get; set; }
        public Int16? ProductID { get; set; }
        public Int16? DispatchReasonTypeCd { get; set; }
        public Int16? AdditionalDispatchReasonTypeCD { get; set; }
        public Int32? FromLocationID { get; set; }
        public Int32? ToLocationID { get; set; }
        public String ShipmentAN { get; set; }
        public Int32? DriverID { get; set; }
        public DateTime? DispatchStartDT { get; set; }
        public DateTime? DispatchEndDT { get; set; }
        public DateTime? ScheduledDeliveryDT { get; set; }
        public Int32? ContactID { get; set; }
        public Int32? ChargeCodeID { get; set; }
        public Int32? ChargeBlockLocationID { get; set; }
        public Int16? CraneLiftAmt { get; set; }
        public Int32? FittingCD { get; set; }
        public Boolean PlannedFL { get; set; }
        public String ProNumberAN { get; set; }
        public String CommentsAn { get; set; }
        public Int32 LocationID { get; set; }
        public Double CallOutHoursAMT { get; set; }
        public Int32? MoveTypeCD { get; set; }
        public Int32? WasteClassTypeCD { get; set; }
        public Boolean? ReloadFL { get; set; }
        public Boolean? CleaningApprovedFL { get; set; }
        public String WPNAN { get; set; }
        public Boolean ActiveFL { get; set; }
        public String UpdateUserAN { get; set; }
    }
}
