using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOWTank.Core.Domain.TANK_usp_insupd
{
    public class TANK_usp_insupd_FacilityParameter_spParams
    {
        public TANK_usp_insupd_FacilityParameter_spParams()
        {
            ActiveFL = true;
        }
        public int InstallID { get; set; }
        public int? KEY { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public int? MajorLocationID { get; set; }
        public Boolean ActiveFL { get; set; }
        public DateTime? UpdateDT  { get; set; }
        public string UpdateUserAN { get; set; }
    }
}
