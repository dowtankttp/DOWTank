using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOWTank.Utility
{
    public class SecurityExtended
    {
        public string UserName { get; set; }
        public int SecurityProfileId { get; set; }
        public int? LocationId { get; set; }
        public string LocationName { get; set; }
    }
}