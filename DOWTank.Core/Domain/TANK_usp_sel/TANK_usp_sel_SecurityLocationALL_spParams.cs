using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOWTank.Core.Domain.TANK_usp_sel
{
    public class TANK_usp_sel_SecurityLocationALL_spParams
    {
        public string UserAn { get; set; }
        public int? SecurityID { get; set; }
    }

    public class TANK_usp_sel_SecurityLocationALL_spResults
    {
        public int? LocationID { get; set; }
        public string LocationDS { get; set; }
        public int? SecurityProfileID { get; set; }
        public string SecurityProfileDS { get; set; }
        public int? DefaultLocationID { get; set; }
        public string DefaultDS { get; set; }
    }
}
