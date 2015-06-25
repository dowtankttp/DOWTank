using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOWTank.Core.Domain.TANK_usp_sel
{
    public class TANK_usp_sel_SecurityProfileDDL_spParams
    {
        public bool IncludeBlank { get; set; }
        public int? LocationID { get; set; }
    }

    public class TANK_usp_sel_SecurityProfileDDL_spResults
    {
        public int? SecurityProfileID { get; set; }
        public string SecurityProfileDS { get; set; }
        public int SortNr { get; set; }
    }
}
