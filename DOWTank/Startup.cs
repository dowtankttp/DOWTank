using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using DOWTank;
using DOWTank.Common;
using DOWTank.Core;
using DOWTank.Core.Service;
using DOWTank.Utility;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace DOWTank
{

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
                {
                    AuthenticationType = "ApplicationCookie",
                    LoginPath = new PathString("/Account/Login")
                });
        }

    }
}