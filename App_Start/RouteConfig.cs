using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SchoolERP_System
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
               "Default",
              "{controller}/{action}/{id}",
                new { controller = "Account", action = "AdminAuthentication", id = UrlParameter.Optional },
               new[] { "AppName.Controllers" }
           );
            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Login", action = "LoginAdmin", id = UrlParameter.Optional }

            //).DataTokens = new RouteValueDictionary(new { area = "LearningAdmin" });

            // routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "LoginERP", action = "LoginERPAdmin", id = UrlParameter.Optional }

            //).DataTokens = new RouteValueDictionary(new { area = "ERPAdmin" });
        }
    }
}
