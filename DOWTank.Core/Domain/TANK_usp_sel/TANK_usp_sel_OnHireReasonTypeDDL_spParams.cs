using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOWTank.Core.Domain.TANK_usp_sel
{
    public class TANK_usp_sel_OnHireReasonTypeDDL_spParams
    {
        public bool IncludeBlank { get; set; }
    }
    public class TANK_usp_sel_OnHireReasonTypeDDL_spResults
    {
        public Int16? OnHireReasonTypeCD { get; set; }
        public string OnHireReasonTypeDS { get; set; }
        public int SortNr { get; set; }
    }
}
