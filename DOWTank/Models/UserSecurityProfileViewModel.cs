using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOWTank.Models
{
    public class UserSecurityProfileViewModel
    {
        public UserSecurityProfileViewModel()
        {
            DashboardList = new List<UserSecurityListViewModel>();
            DashboardMenu = new List<UserSecurityListViewModel>();
            DispatchScreen = new List<UserSecurityListViewModel>();
            PrepScreen = new List<UserSecurityListViewModel>();
            RequireCleaning = new List<UserSecurityListViewModel>();
            RequireService = new List<UserSecurityListViewModel>();
            TankSearchScreen = new List<UserSecurityListViewModel>();
            TankHistoryScreen = new List<UserSecurityListViewModel>();
            GroundedMatrix = new List<UserSecurityListViewModel>();
            ReportsMenuOptions = new List<UserSecurityListViewModel>();
            AdminMenuOptions = new List<UserSecurityListViewModel>();
            ProductMaster = new List<UserSecurityListViewModel>();
        }
        public int Id { get; set; }
        public string ProfileName { get; set; }
        public bool IsActive { get; set; }
        public List<UserSecurityListViewModel> DashboardMenu { get; set; }
        public List<UserSecurityListViewModel> DashboardList { get; set; }
        public List<UserSecurityListViewModel> DispatchScreen { get; set; }
        public List<UserSecurityListViewModel> PrepScreen { get; set; }
        public List<UserSecurityListViewModel> RequireCleaning { get; set; }
        public List<UserSecurityListViewModel> RequireService { get; set; }
        public List<UserSecurityListViewModel> TankSearchScreen { get; set; }
        public List<UserSecurityListViewModel> TankHistoryScreen { get; set; }
        public List<UserSecurityListViewModel> GroundedMatrix { get; set; }
        public List<UserSecurityListViewModel> ReportsMenuOptions { get; set; }
        public List<UserSecurityListViewModel> AdminMenuOptions { get; set; }
        public List<UserSecurityListViewModel> ProductMaster { get; set; }
    }

    public class UserSecurityListViewModel
    {
        public int PrivilegeID { get; set; }
        public string PrivilegeDS { get; set; }
        public int GrantedFL { get; set; }
    }
}