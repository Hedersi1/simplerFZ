using System.Web.Mvc;
using System.Web.Routing;

namespace Simple.MVC.WEB
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Default","{controller}/{action}/{id}",new { controller = "Site", action = "Indice", id = UrlParameter.Optional },new string[] { "Simple.MVC.WEB.Controllers" }
            );
        }
    }
}