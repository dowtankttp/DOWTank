using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOWTank.Core.Domain.TANK_usp_insupd
{
    public class TANK_usp_insupd_SecurityLocation_spParams
    {
        public int? SecurityID { get; set; }
        public int? LocationID { get; set; }
        public int? SecurityProfileId { get; set; }
        public bool DefaultFL { get; set; }
    }
}
