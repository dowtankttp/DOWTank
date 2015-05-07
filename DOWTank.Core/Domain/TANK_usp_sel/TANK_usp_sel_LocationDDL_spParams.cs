using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOWTank.Core.Domain.TANK_usp_sel
{
    public class TANK_usp_sel_LocationDDL_spParams
    {
        public bool IncludeBlank { get; set; }
        public int? MajorLocationID { get; set; }
        public int? LocationTypeCD { get; set; }

    }
}
