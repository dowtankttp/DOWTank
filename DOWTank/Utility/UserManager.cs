using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace DOWTank.Utility
{
    public class UserManager
    {
        //private readonly IMembershipProvider _membershipProvider;
        //private readonly GluuDatabase _database;

        //public UserManager(IMembershipProvider membershipProvider, GluuDatabase database)
        //{
        //    _membershipProvider = membershipProvider;
        //    _database = database;
        //}

        public bool Athenticate(string userName, string password, bool rememberMe, string site = null)
        {
            //if (_membershipProvider.ValidateUser(userName, password, site))
            //{
            //    return true;
            //}
            return false;
        }

        //public ClaimsIdentity CreateIdentity(LoginPostModel model, string site = null)
        //{
        //    var identity = new ClaimsIdentity("ApplicationCookie");
        //    identity.AddClaim(new Claim("UserName", model.UserName));
        //    var user = _database.Users.GetByUsername(model.UserName, site);
        //    identity.AddClaim(new Claim("AccountId", user.UniqueId.ToString()));

        //    return identity;
        //}
    }

}