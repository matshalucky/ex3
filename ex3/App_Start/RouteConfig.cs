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
                name: "DisplayLoad",
                url: "display/{s}/{num}",
                defaults: new { controller = "First", action = "MapOrLoad" }
            );

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "SaveRoute",
                url: "save/{ip}/{port}/{pace}/{duration}/{fileName}",
                defaults: new { controller = "First", action = "Save" }
            );

            routes.MapRoute(
               name: "DisplayRoute",
               url: "display/{ip}/{port}/{time}",
               defaults: new { controller = "First", action = "DisplayRoute" }
           );

            routes.MapRoute(
              name: "Default",
              url: "{action}/{id}",
              defaults: new { controller = "First", action = "Index" , id = UrlParameter.Optional}
          );
        }
    }
}
