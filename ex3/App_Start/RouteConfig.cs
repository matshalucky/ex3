using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ex3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Display",
                url: "Display/{ip}/{port}",
                defaults: new { controller = "First", action = "Map" }
            );

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "SaveRoute",
                url: "Save/{ip}/{port}/{pace}/{duration}/{fileName}",
                defaults: new { controller = "First", action = "save" }
            );

            routes.MapRoute(
               name: "DisplayRoute",
               url: "Display/{ip}/{port}/{time}",
               defaults: new { controller = "First", action = "displayRoute" }
           );

            routes.MapRoute(
              name: "Default",
              url: "{action}/{id}",
              defaults: new { controller = "First", action = "Index" , id = UrlParameter.Optional}
          );
        }
    }
}
