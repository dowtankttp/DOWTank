using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using DOWTank.Core.Domain.TANK_usp_sel;
using DOWTank.Core.Service;
using DOWTank.Models;

namespace DOWTank.Utility
{
    public class UserManager
    {
        private readonly IUtilityService _utilityService;
        private readonly IPasswordService _passwordService;

        public UserManager(IPasswordService passwordService, IUtilityService utilityService)
        {
            _passwordService = passwordService;
            _utilityService = utilityService;
        }

        public bool Athenticate(string userName, string password)
        {
            return _passwordService.ValidateUserLogin(userName, password);
        }

        public ClaimsIdentity CreateIdentity(LoginPostModel model)
        {
            var identity = new ClaimsIdentity("ApplicationCookie");
            identity.AddClaim(new Claim("UserName", model.UserName));

            //TODO: add identity as required

            #region SetSecurityGroups

            var TANK_usp_Login_spParams = new TANK_usp_Login_spParams()
                {
                    //todo: re-factor this logic later
                    SecurityID = "603"
                };
            var securityGroups = _utilityService.ExecStoredProcedureWithResults<string>("TANK_usp_Login",
                                                                                            TANK_usp_Login_spParams);
            string roleList = string.Empty;
            if (securityGroups != null && securityGroups.Any())
            {
                roleList = string.Join<string>(",", securityGroups.ToList());
                identity.AddClaim(new Claim("Roles", roleList));
            }

            #endregion SetSecurityGroups

            return identity;
        }


    }

}