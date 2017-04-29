using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Simple.MVC.WEB.Seguranca
{
    public  class RegistrarAreaSeguranca : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Seguranca";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Seguranca_default",
                "Seguranca/{controller}/{action}/{id}",
                new { controller = "Inicio", action = "Indice", id = UrlParameter.Optional },
                new string[] { "Simple.MVC.WEB.Seguranca.Controllers" }
            );
        }
    }
}