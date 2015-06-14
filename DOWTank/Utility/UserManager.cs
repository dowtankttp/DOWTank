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

        public bool Athenticate(string userName, string password)
        {
            return _passwordService.ValidateUserLogin(userName, password);
        }

        public ClaimsIdentity CreateIdentity(LoginPostModel model)
        {
            //todo: remove it later
            model.UserName = "coryg";
            var identity = new ClaimsIdentity("ApplicationCookie");
            identity.AddClaim(new Claim("UserName", model.UserName));
            
            //TODO: add identity as required

            #region  location

            var locationResults = _sharedFunctions.GetLocation(model.UserName);
            if (locationResults != null && locationResults.Any())
            {
                identity.AddClaim(new Claim("LocationId", locationResults[0].LocationID.HasValue ? locationResults[0].LocationID.ToString() : null));
                identity.AddClaim(new Claim("LocationName", locationResults[0].LocationDS));
                identity.AddClaim(new Claim("SecurityProfileId", locationResults[0].SecurityProfileID.ToString()));
            }

            #endregion location

            #region SetSecurityGroups

            var securityGroups = GetSecurityGroups();

            string roleList = string.Empty;
            if (securityGroups != null && securityGroups.Any())
            {
                roleList = string.Join<string>(",", securityGroups.ToList());
                identity.AddClaim(new Claim("Roles", roleList));
            }

            #endregion SetSecurityGroups

            return identity;
        }

        private IEnumerable<string> GetSecurityGroups()
        {

            var TANK_usp_Login_spParams = new TANK_usp_Login_spParams()
            {
                //todo: re-factor this logic later
                SecurityID = "603"
            };
            return _utilityService.ExecStoredProcedureWithResults<string>("TANK_usp_Login",
                                                                                            TANK_usp_Login_spParams);
        }

    }

}