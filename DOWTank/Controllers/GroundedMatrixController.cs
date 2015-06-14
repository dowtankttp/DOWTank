using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOWTank.Core.Domain.TANK_usp_rpt;
using DOWTank.Core.Service;
using DOWTank.Core.Domain.TANK_usp_sel;
using DOWTank.Custom;
using DOWTank.Utility;

namespace DOWTank.Controllers
{
    [ClaimsAuthorize(Roles = "Yard")]
    public class GroundedMatrixController : BaseController
    {
        private readonly IUtilityService _utilityService;

        public GroundedMatrixController(IUtilityService utilityService)
        {
            _utilityService = utilityService;
        }

        // GET: /Grounded Matrix/
        public ActionResult Index()
        {
            PopulateSecurityExtended();
            
            GroundedMatrixModel groundedMatrixModel = new GroundedMatrixModel();

            bool includeBlank = false;
            int? majorLocationID = SecurityExtended.LocationId;
            int? locationTypeCS = 7;
            int? selectedLocation = null;
            groundedMatrixModel.DataTableLocationDDl = GetLocationDDL(includeBlank, locationTypeCS, majorLocationID);

            if (groundedMatrixModel.DataTableLocationDDl.Rows.Count > 0)
                selectedLocation = Convert.ToInt32(groundedMatrixModel.DataTableLocationDDl.Rows[0]["LocationID"]);

            groundedMatrixModel.DataTableGroundedMatrix = GetTankGroundedMatrix(majorLocationID, selectedLocation); 
            
            return View(groundedMatrixModel);
        }

        public ActionResult Search(GroundedMatrixModel postModel)
        {
            PopulateSecurityExtended();
            GroundedMatrixModel groundedMatrixModel = new GroundedMatrixModel();
            int? locationTypeCS = Convert.ToInt32(postModel.HfSelectedSection);
            bool includeBlank = false;
            int? majorLocationID = SecurityExtended.LocationId;

            groundedMatrixModel.DataTableLocationDDl = GetLocationDDL(false, locationTypeCS, majorLocationID);
            groundedMatrixModel.DataTableGroundedMatrix = GetTankGroundedMatrix(majorLocationID, locationTypeCS);
            return View("Index", groundedMatrixModel);            
        }

        #region Private Methods

        private DataTable GetTankGroundedMatrix(int? majorLocationID, int? sectionLocationID)
        {
            // database call                        
            var TANK_usp_sel_GroundedMatrix_spParams = new TANK_usp_sel_GroundedMatrix_spParams();
            TANK_usp_sel_GroundedMatrix_spParams.MajorLocationID = Convert.ToInt32(majorLocationID);
            TANK_usp_sel_GroundedMatrix_spParams.SectionLocationID = sectionLocationID; 

            DataTable data = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_sel_GroundedMatrix", TANK_usp_sel_GroundedMatrix_spParams);

            @ViewBag.TotalRecordsGroundedMatrix = data.Rows.Count;

            return data;
        }

        private DataTable GetLocationDDL(bool includeBlank, int? locationTypeCD, int? majorLocationID)
        {
            // database call                        
            var TANK_usp_sel_LocationDDL_spParams = new TANK_usp_sel_LocationDDL_spParams();
            TANK_usp_sel_LocationDDL_spParams.IncludeBlank = includeBlank;
            TANK_usp_sel_LocationDDL_spParams.LocationTypeCD = locationTypeCD;
            TANK_usp_sel_LocationDDL_spParams.MajorLocationID = majorLocationID;

            DataTable data = _utilityService.ExecStoredProcedureForDataTable("TANK_usp_sel_LocationDDL", TANK_usp_sel_LocationDDL_spParams);

            return data;
        }
        
        #endregion

    }

    public class GroundedMatrixModel
    {
        public DataTable DataTableGroundedMatrix { get; set; }
        public DataTable DataTableLocationDDl { get; set; }        
        public string HfSelectedSection { get; set; }
    }
    
}