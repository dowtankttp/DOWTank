using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOWTank.Core.Domain.TANK_usp_sel
{
    public class TANK_usp_sel_SecurityProfilePrivileges_spResults
    {
        public int? PrivilegeCategoryID { get; set; }
        public string PrivilegeCategoryDS { get; set; }
        public int? PrivilegeID { get; set; }
        public string PrivilegeDS { get; set; }
        public int GrantedFL { get; set; }
    }
}
