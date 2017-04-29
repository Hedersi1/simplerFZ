using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Simple.MVC.WEB.Controllers
{
    [AllowAnonymous]
    public class SiteController : Controller
    {
        // GET: Site
        public ActionResult Indice()
        {
            return View();
        }
    }
}