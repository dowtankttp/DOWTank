using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOWTank.Core.Domain.TANK_usp_insupd
{
    public class TANK_usp_insupd_SecurityProfile_spParams
    {
        public int? SecurityProfileID { get; set; }
        public string SecurityProfileDS { get; set; }
        public int? LocationID { get; set; }
        public bool ActiveFL { get; set; }
        public string UpdateUserAN { get; set; }
    }
}
