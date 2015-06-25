using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOWTank.Models
{
    public class EditUserViewModel
    {
        public int SecurityID { get; set; }
        public string FullName { get; set; }
        public bool ActiveFL { get; set; }
    }
}