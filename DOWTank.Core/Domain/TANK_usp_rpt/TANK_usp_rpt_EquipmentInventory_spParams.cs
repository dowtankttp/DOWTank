using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOWTank.Core.Domain.TANK_usp_rpt
{
    public class TANK_usp_rpt_EquipmentInventory_spParams
    {
        public int LocationID { get; set; }
        public int? OwnerID { get; set; }
        public int? OperatorID { get; set; }
        public int? EquipmentClassTypeCD { get; set; }
        public int? EquipmentTypeCD { get; set; }
        public int? TankGradeTypeCD { get; set; }
        public int? BarrelConditionTypeCD { get; set; }
        public int? LoadStatusTypeCD { get; set; }
        public string CurrentLocationDS { get; set; }
        public int? MoveTypeCD { get; set; }
    }
}
