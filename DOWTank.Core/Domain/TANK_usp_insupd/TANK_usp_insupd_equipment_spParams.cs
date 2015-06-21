using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOWTank.Core.Domain.TANK_usp_insupd
{
    public class TANK_usp_insupd_equipment_spParams
    {
        public int? EquipmentID { get; set; }
        public string EquipmentAN { get; set; }
        public char CheckDigitAN { get; set; }
        public Int16? EquipmentTypeCD { get; set; }
        public int? LocationID { get; set; }
        public bool LocationDedicatedFL { get; set; }
        public int? OwnerID { get; set; }
        public int? OperatorID { get; set; }
        public Int16? LoadStatusTypeCD { get; set; }
        public Int16? MaintConditionTypeCD { get; set; }
        public DateTime? DOTInspectedDT { get; set; }
        public decimal? TankCapacity { get; set; }
        public DateTime? Tank2YRTestDT { get; set; }
        public DateTime? Tank5YRTestDT { get; set; }
        public bool? LastTestHydroFL { get; set; }
        public Int16? DedicatedProductID { get; set; }
        public int? DedicatedLocationID { get; set; }
        public int? BarrelConditionTypeCD { get; set; }
        public bool PoolFL { get; set; }
        public int? TankGradeTypeCD { get; set; }
        public int? ProductID { get; set; }
        public int? MoveTypeCD { get; set; }
        public bool ActiveFL { get; set; }
        public string UpdateUserAN { get; set; }
    }
}
