using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using DOWTank.Core.Service;
using DOWTank.Models;

namespace DOWTank.Utility
{
    public class UserManager
    {
        private readonly IPasswordService _passwordService;

        public UserManager(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        public bool Athenticate(string userName, string password)
        {
           return _passwordService.ValidateUserLogin(userName, password);
        }

        public ClaimsIdentity CreateIdentity(LoginPostModel model)
        {
            var identity = new ClaimsIdentity("ApplicationCookie");
            identity.AddClaim(new Claim("UserName", model.UserName));
            //var user = _database.Users.GetByUsername(model.UserName, site);
            //identity.AddClaim(new Claim("AccountId", user.UniqueId.ToString()));
            //TODO: add identity as required



            return identity;
        }
    }

}