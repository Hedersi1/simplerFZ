using System.Web.Mvc;

namespace Simple.MVC.WEB
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new Simple.MVC.Security.Data.AutorizeAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}