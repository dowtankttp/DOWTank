using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOWTank.Core.Domain.TANK_usp_sel
{
    public class TANK_usp_sel_SecurityProfilePrivileges_spParams
    {
        public int? SecurityProfileID { get; set; }
        public int? PrivilegeCategoryID { get; set; }
        public int? CurrentUserProfileID { get; set; }
    }
}
