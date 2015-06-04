using System;

namespace DOWTank.Core.Domain.TANK_usp_rpt
{
    public class TANK_usp_rpt_TankActivity_spParams
    {
        public string EquipmentAN { get; set; }
        public DateTime StartDT { get; set; }
        public DateTime EndDT { get; set; }
        public int? OwnerID { get; set; }
        public int? OperatorID { get; set; }
        public int? PoolFL { get; set; }
        public int? LocationID { get; set; }
    }
}
