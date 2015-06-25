using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOWTank.Models
{
    public class SecurityLocationPostModel
    {
        public int? SecurityID { get; set; }
        public int? LocationID { get; set; }
        public int? SecurityProfileId { get; set; }
        public bool DefaultFL { get; set; }
    }
}