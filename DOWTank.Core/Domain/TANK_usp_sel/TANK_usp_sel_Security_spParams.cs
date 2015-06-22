using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOWTank.Core.Domain.TANK_usp_sel
{
    public class TANK_usp_sel_Security_spParams
    {
        public int? SecurityID { get; set; }
        public string UserAN { get; set; }
        public int? LocationID { get; set; }
    }

    public class TANK_usp_sel_Security_spResults
    {
        public int SecurityID { get; set; }
        public string FirstNM { get; set; }
        public string LastNM { get; set; }
        public string UserAN { get; set; }
        public string FullName { get; set; }
        public DateTime? LastLoginDt { get; set; }
        public int? LocationID { get; set; }
        public bool ActiveFL { get; set; }
        public string ActiveDS { get; set; }
        public DateTime? CreateDT { get; set; }
        public int? SecurityProfileID { get; set; }
        public string SecurityProfileDS { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }
    }
}
