﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOWTank.Core.Domain.TANK_usp_sel
{
    public class TANK_usp_sel_DispatchLastMove_spResults
    {
        public int? DispatchID { get; set; }
        public int? EquipmentID { get; set; }
        public string EquipmentAN { get; set; }
        public int? ChargeBlockLocationID { get; set; }
        public String ChargeBlockLocationDS { get; set; }
        public String ChargeCodeAN { get; set; }
        public Int32? ChassisEquipmentID { get; set; }
        public String ChassisEquipmentAN { get; set; }
        public String DispatchStartDt { get; set; }
        public int? FromLocationID { get; set; }
        public string FromLocationDS { get; set; }
        public String DispatchEndDt { get; set; }
        public String Driver { get; set; }
        public Int32? DriverID { get; set; }
        public Int16? ProductID { get; set; }
        public String ProductDS { get; set; }
        public int? ToLocationID { get; set; }
        public String ToLocationDS { get; set; }
        public String FittingDS { get; set; }
        public Int32? FittingCD { get; set; }        
        public String ShipmentAN { get; set; }
        public String ContactNM { get; set; }
        public Int32? ContactID { get; set; }        
    }
}
