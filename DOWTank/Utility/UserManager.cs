using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using DOWTank.Common;
using DOWTank.Core.Domain.TANK_usp_sel;
using DOWTank.Core.Service;
using DOWTank.Models;

namespace DOWTank.Utility
{
    public class UserManager
    {
        private readonly IUtilityService _utilityService;
        private readonly ISharedFunctions _sharedFunctions;
        private readonly IPasswordService _passwordService;

        public UserManager(IPasswordService passwordService, IUtilityService utilityService, ISharedFunctions sharedFunctions)
        {
            _passwordService = passwordService;
            _utilityService = utilityService;
            _sharedFunctions = sharedFunctions;
        }

        public bool Athenticate(string userName, string password, ref TANK_usp_sel_Security_spResults userDetails)
        {
            return _passwordService.ValidateUserLogin(userName, password, ref userDetails);
        }

        public ClaimsIdentity CreateIdentity(DOWTank.Core.Domain.TANK_usp_sel.TANK_usp_sel_Security_spResults userDetails)
        {
            //todo: remove it later
            var identity = new ClaimsIdentity("ApplicationCookie");
            identity.AddClaim(new Claim("UserName", userDetails.UserAN));
            identity.AddClaim(new Claim("DisplayName", userDetails.FullName));

            #region  location

            var locationResults = _sharedFunctions.GetLocation(userDetails.UserAN);
            if (locationResults != null && locationResults.Any())
            {
                identity.AddClaim(new Claim("LocationId", locationResults[0].LocationID.HasValue ? locationResults[0].LocationID.ToString() : null));
                identity.AddClaim(new Claim("LocationName", locationResults[0].LocationDS));
                identity.AddClaim(new Claim("SecurityProfileId", locationResults[0].SecurityProfileID.ToString()));
            }

            #endregion location

            #region SetSecurityGroups

            var securityGroups = GetSecurityGroups(userDetails);

            string roleList = string.Empty;
            if (securityGroups != null && securityGroups.Any())
            {
                roleList = string.Join<string>(",", securityGroups.ToList());
                identity.AddClaim(new Claim("Roles", roleList));
            }

            #endregion SetSecurityGroups

            return identity;
        }

        private IEnumerable<string> GetSecurityGroups(TANK_usp_sel_Security_spResults userDetails)
        {

            var TANK_usp_Login_spParams = new TANK_usp_Login_spParams()
            {
                //todo: re-factor this logic later
                SecurityID = userDetails.SecurityID.ToString()
            };
            return _utilityService.ExecStoredProcedureWithResults<string>("TANK_usp_Login",
                                                                                            TANK_usp_Login_spParams);
        }

    }

}