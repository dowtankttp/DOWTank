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
        public static string GetRoles(this ClaimsIdentity identity)
        {
            return identity.FindFirst(c => c.Type == "Roles").Value;
        }
        public static int GetSecurityProfileId(this ClaimsIdentity identity)
        {
            return Convert.ToInt32(identity.FindFirst(c => c.Type == "SecurityProfileId").Value);
        }
        public static string GetLocationName(this ClaimsIdentity identity)
        {
            return identity.FindFirst(c => c.Type == "LocationName").Value;
        }
        public static string GetLocationId(this ClaimsIdentity identity)
        {
            return identity.FindFirst(c => c.Type == "LocationId").Value;
        }

        public static int? ToNullableInt32(this string s)
        {
            int i;
            if (Int32.TryParse(s, out i)) return i;
            return null;
        }
    }
}