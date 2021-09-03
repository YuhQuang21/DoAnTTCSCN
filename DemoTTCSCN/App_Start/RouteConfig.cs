using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DemoTTCSCN
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //Admin Controller
            routes.MapRoute(
                name: "Update",
                url: "Update",
                defaults: new { controller = "Admin", action = "UpdateStudent", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Detail",
                url: "Detail/{id}",
                defaults: new { controller = "Admin", action = "GetStudentById", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "SystemManagement",
                url: "SystemManagement",
                defaults: new { controller = "Admin", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "GetListStudent",
                url: "GetListStudent",
                defaults: new { controller = "Admin", action = "GetListStudent", id = UrlParameter.Optional }
            );
            //Account Controller
            routes.MapRoute(
                name: "Logout",
                url: "Logout",
                defaults: new { controller = "Account", action = "Logout", id = UrlParameter.Optional }
            );
            //Home Controller
            routes.MapRoute(
                name: "StudentDetail",
                url: "StudentDetail",
                defaults: new { controller = "Home", action = "StudentDetail", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Transcript",
                url: "Transcript",
                defaults: new { controller = "Home", action = "Transcript", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Home",
                url: "Home",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
