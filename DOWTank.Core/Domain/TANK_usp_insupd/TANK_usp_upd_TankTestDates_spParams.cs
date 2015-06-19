using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOWTank.Core.Domain.TANK_usp_insupd
{
    public class TANK_usp_upd_TankTestDates_spParams
    {
        public int EquipmentID { get; set; }
        public DateTime Tank5YRTestDT { get; set; }
        public bool LastTestHydroFL { get; set; }
        public string UpdateUserAN { get; set; }
    }
}
