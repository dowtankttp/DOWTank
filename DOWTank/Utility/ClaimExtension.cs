using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace DOWTank.Utility
{
    public static class ClaimExtension
    {
        public static string GetUserName(this ClaimsIdentity identity)
        {
            return identity.FindFirst(c => c.Type == "UserName").Value;
        }
        public static string GetAccountId(this ClaimsIdentity identity)
        {
            return identity.FindFirst(c => c.Type == "AccountId").Value;
        }
    }
}