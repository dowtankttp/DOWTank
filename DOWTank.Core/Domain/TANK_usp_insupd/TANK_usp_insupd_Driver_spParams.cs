using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOWTank.Core.Domain.TANK_usp_insupd
{
    public class TANK_usp_insupd_Driver_spParams
    {
        public TANK_usp_insupd_Driver_spParams()
        {
            ActiveFL = true;
        }

        public int? Key { get; set; }        
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Trucker { get; set; }
        public string Unit { get; set; }
        public string Radio { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }        
        public Boolean ManualFL { get; set; }
        public Boolean ActiveFL { get; set; }
        public string UpdateUserAN { get; set; }
    }
}
