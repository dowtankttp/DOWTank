using System;

namespace DOWTank.Core.Domain.TANK_usp_rpt
{
    public class TANK_usp_rpt_TankShipmentSchedule_spParams
    {
        public int InstallID { get; set; }
        public int LocationID { get; set; }
        public int? DispatchedFL { get; set; }
        public string EquipmentAN { get; set; }
        public DateTime StartDT { get; set; }
        public DateTime EndDT { get; set; }
    }
}
