﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOWTank.Core.Domain.TANK_usp_rpt
{
    public class TANK_usp_sel_EquipmentOnHireHistory_spParams
    {
        public string EquipmentAn { get; set; }
        public int? OnHireHistoryID { get; set; }
        public int? LocationID { get; set; }
    }
}
