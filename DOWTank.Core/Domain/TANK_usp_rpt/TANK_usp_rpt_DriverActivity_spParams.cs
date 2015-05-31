using System;

namespace DOWTank.Core.Domain.TANK_usp_rpt
{
    public class TANK_usp_rpt_DriverActivity_spParams
    {
        public DateTime StartDT { get; set; }
        public DateTime EndDT { get; set; }
        public int? DriverID { get; set; }
        public int LocationID { get; set; }
    }
}
