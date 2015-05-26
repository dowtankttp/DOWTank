using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOWTank.Core.Domain.TANK_usp_insupd
{
    public class TANK_usp_insupd_Contact_spParams
    {
        public TANK_usp_insupd_Contact_spParams()
        {
            ActiveFL = true;
        }
        public int? Key { get; set; }
        public int? MajorLocationID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Phone { get; set; }
        public string Ext { get; set; }
        public Boolean ActiveFL { get; set; }
        public string UpdateUserAN { get; set; }
    }
}
