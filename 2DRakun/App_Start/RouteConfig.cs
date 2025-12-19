using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace _2DRakun
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "LoginRoute",
            //    url: "Login",
            //    defaults: new { controller = "Home", action = "Login" }
            //);

            routes.MapRoute(
                  name: "ApiRoute",
                  url: "api/Api/{action}",
                  defaults: new { controller = "Api", action = "Index", id = UrlParameter.Optional }
                );


            routes.MapRoute(
            name: "RootWithName",
            url: "{action}/{name}/{id}",
            defaults: new { controller = "Home", action = "Index", name = UrlParameter.Optional, id = UrlParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
