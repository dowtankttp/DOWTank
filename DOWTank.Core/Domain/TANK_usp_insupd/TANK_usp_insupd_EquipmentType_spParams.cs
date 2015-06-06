using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOWTank.Core.Domain.TANK_usp_insupd
{
    public class TANK_usp_insupd_EquipmentType_spParams
    {
        public TANK_usp_insupd_EquipmentType_spParams()
        {
            ActiveFL = true;
        }
        public int? Key { get; set; }
        public int? Class { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public double Length { get; set; }
        public int LocationID { get; set; }
        public Boolean ActiveFL { get; set; }
        public string UpdateUserAN { get; set; }
    }
}
