using System;
namespace DOWTank.Core.Domain.TANK_usp_rpt
{
    public class TANK_usp_rpt_DailyDispatch_spParams
    {
        public int LocationID { get; set; }
        public DateTime StartDT { get; set; }
        public DateTime EndDT { get; set; }
    }
}
