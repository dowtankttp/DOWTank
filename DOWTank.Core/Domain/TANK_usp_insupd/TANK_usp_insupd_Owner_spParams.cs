using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOWTank.Core.Domain.TANK_usp_insupd
{
    public class TANK_usp_insupd_Owner_spParams
    {
        public TANK_usp_insupd_Owner_spParams()
        {
            ActiveFL = true;
        }
        public int? Key { get; set; }
        public string Name { get; set; }
        public Boolean ActiveFL { get; set; }
        public string UpdateUserAN { get; set; }
    }

}
