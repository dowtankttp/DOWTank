﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOWTank.Core.Domain.TANK_usp_rpt
{
    public class TANK_usp_rpt_DedicatedTanks_spParams
    {
        public int LocationID { get; set; }
        public string CurrentLocationDS { get; set; }
        public string DedicatedLocationDS { get; set; }
        public string DedicatedProductDS { get; set; }
    }
}
