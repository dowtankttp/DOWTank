using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOWTank.Utility
{
    public static class Utility
    {
        public static string Left(this string str, int length)
        {
            return str.Substring(0, Math.Min(length, str.Length));
        }
    }
}