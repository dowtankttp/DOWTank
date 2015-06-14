using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOWTank.Core.Domain.TANK_usp_sel
{
    public class TANK_usp_sel_SecurityLocation_spParams
    {
        public string UserAN { get; set; }
    }

    public class TANK_usp_sel_SecurityLocation_spResults
    {
        public int? LocationID { get; set; }
        public string LocationDS { get; set; }
        public int SecurityProfileID { get; set; }
    }
}
