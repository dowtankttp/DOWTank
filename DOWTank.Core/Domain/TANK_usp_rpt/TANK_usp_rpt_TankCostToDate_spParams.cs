using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOWTank.Core.Domain.TANK_usp_rpt
{
    public class TANK_usp_rpt_TankCostToDate_spParams
    {
        public DateTime StartDT { get; set; }
        public DateTime EndDT { get; set; }
        public int LocationID { get; set; }
        public int? OwnerID { get; set; }
        public int? OperatorID { get; set; }
        public Boolean PoolFL { get; set; }
    }
}
