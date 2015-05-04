using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOWTank.Core.Domain.TANK_usp_rpt
{
    public class TANK_usp_sel_EquipmentInfo_spParams
    {
        public int? EquipmentID { get; set; }
        public string EquipmentAN { get; set; }
        public int? MajorLocationID { get; set; }
    }
}
