using System;
using System.ComponentModel.DataAnnotations;

namespace DOWTank.Models
{
    public class AdminEquipmentModel
    {
        public AdminEquipmentModel()
        {
            LocationDedicatedFL = false;
            PoolFL = true;
            ActiveFL = true;
            UpdateUserAN = "SYSTEM";
        }
        public Int16? EquipmentClassTypeCD { get; set; }
        public Int32? EquipmentID { get; set; }
        [Required(ErrorMessage = "Equipment # is required")]
        public String EquipmentAN { get; set; }
        public Char CheckDigitAN { get; set; }
        [Required(ErrorMessage = "Type is required")]
        public Int16? EquipmentTypeCD { get; set; }
        public Int32? LocationID { get; set; }
        public Boolean LocationDedicatedFL { get; set; }
        public Int32? OwnerID { get; set; }
        public Int32? OperatorID { get; set; }
        public Int16? LoadStatusTypeCD { get; set; }
        public Int16? MaintConditionTypeCD { get; set; }
        public DateTime? DOTInspectedDT { get; set; }
        public decimal? TankCapacity { get; set; }
        public DateTime? Tank2YRTestDT { get; set; }
        public DateTime? Tank5YRTestDT { get; set; }
        public Boolean LastTestHydroFL { get; set; }
        public Int16? DedicatedProductID { get; set; }
        public Int32? DedicatedLocationID { get; set; }
        public Int32? BarrelConditionTypeCD { get; set; }
        public Boolean PoolFL { get; set; }
        public Int32? TankGradeTypeCD { get; set; }
        public Int32? ProductID { get; set; }
        [Required(ErrorMessage = "Service Type is required")]
        public Int32? MoveTypeCD { get; set; }
        public Boolean ActiveFL { get; set; }
        public String UpdateUserAN { get; set; }
    }
}