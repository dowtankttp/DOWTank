using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DOWTank.Models
{
    public class AdminProductModel
    {
        public AdminProductModel()
        {
            InstallID = 1;
            HazardousFL = false;
            WasteFL = false;
            ActiveFL = true;
            TempFL = false;
            UpdateUserAN = "SYSTEM";
        }

        public Int32? Key { get; set; }
        public Int32? InstallID { get; set; }
        [Required(ErrorMessage = "Product Name is required")]
        public String ProductName { get; set; }
        public Boolean HazardousFL { get; set; }
        public Boolean WasteFL { get; set; }
        public Int32? MajorLocationID { get; set; }
        public String ProductCodeAN { get; set; }
        public String MSDSNumberAN { get; set; }
        public String WPNNumberAN { get; set; }
        public String DOTShippingNameDS { get; set; }
        [StringLength(512, ErrorMessage = "Comments must not exceed 512 characters")]
        public String SynonymsDS { get; set; }
        public Int32? DOTHazardClassID { get; set; }
        public String UNNANumberAN { get; set; }
        public String DensityDS { get; set; }
        public Int32? LBSPerGalAMT { get; set; }
        public String PriorContentRestrictionDS { get; set; }
        public String CleaningRequirementsDS { get; set; }
        public decimal? TankCapacity { get; set; }
        public Int16? EquipmentTypeCD { get; set; }
        public Int32? TankConstructionTypeCD { get; set; }
        public Int16? TankNumberOfBarsAMT { get; set; }
        public Int16? TankGradeTypeCD { get; set; }
        public Boolean TankTopDischargeFL { get; set; }
        public Boolean TankBottomDischargeFL { get; set; }
        public Boolean TankSteamCoilsFL { get; set; }
        public Boolean TankElectricHeatFL { get; set; }
        public Boolean ActiveFL { get; set; }
        public Boolean TempFL { get; set; }
        public String UpdateUserAN { get; set; }
    }
}