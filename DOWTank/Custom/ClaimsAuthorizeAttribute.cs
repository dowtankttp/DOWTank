using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using DOWTank.Utility;
using Microsoft.Owin.Security;

namespace DOWTank.Custom
{
    public class ClaimsAuthorizeAttribute : AuthorizeAttribute
    {
        private static readonly string[] _emptyArray = new string[0];
        //roles populated through attribute
        public string Roles { get; set; }
        private RouteValueDictionary RedirectRoute { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var userManager = DependencyResolver.Current.GetService<UserManager>();
            var user = httpContext.User as ClaimsPrincipal;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                return false;
            }

            var identity = httpContext.User.Identity as ClaimsIdentity;
            string userRoles = identity.GetRoles();

            var routeValues = httpContext.Request.RequestContext.RouteData.Values;
            if (routeValues != null && routeValues.ContainsKey("action") && routeValues.ContainsKey("controller"))
            {
                if (!CheckRoles(userRoles))
                {
                    
                     RedirectRoute = new RouteValueDictionary {
                            { "controller", "Home" },
                            { "action", "AccessDenied" },
                        };
                    return false;
                }
            }

            return true;
        }

        private bool CheckRoles(string userRoles)
        {
            string[] roles = RolesSplit;
            if (roles.Length == 0)
            {
                return false;
            }
            string[] userRolesArray = SplitString(userRoles);
            if (userRolesArray.Length == 0)
            {
                return false;
            }
            return (from role in roles from item in userRolesArray where role == item select role).Any();
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(RedirectRoute);
            }
        }

        protected string[] RolesSplit
        {
            get { return SplitString(Roles); }
        }

        internal static string[] SplitString(string original)
        {
            if (String.IsNullOrEmpty(original))
            {
                return _emptyArray;
            }

            var split = from piece in original.Split(',')
                        let trimmed = piece.Trim()
                        where !String.IsNullOrEmpty(trimmed)
                        select trimmed;
            return split.ToArray();
        }

    }
}