using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CounterWebSite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

#if DEBUG
            //prevent browser link errors (see: http://stackoverflow.com/a/21515049)
            routes.IgnoreRoute("{*browserlink}", new { browserlink = @".*/arterySignalR/ping" });
#endif

            
            /* A custom route is added until we handle new login page */            
            routes.MapRoute(
                name: "IfPerformanceHall", 
                url: "ifperformance",
                defaults: new { controller = "Account", action = "Login" }
                
            );
            /**/
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            
        }
    }
}