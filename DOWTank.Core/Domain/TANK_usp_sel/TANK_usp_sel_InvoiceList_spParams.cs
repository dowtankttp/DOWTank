using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOWTank.Core.Domain.TANK_usp_rpt
{
    public class TANK_usp_sel_InvoiceList_spParams
    {
        public int? LocationID { get; set; }
        public string EquipmentAN { get; set; }
        public int? VendorID { get; set; }
        public string InvoiceAN { get; set; }
        DateTime? InvoiceDT { get; set; }
    }
}
