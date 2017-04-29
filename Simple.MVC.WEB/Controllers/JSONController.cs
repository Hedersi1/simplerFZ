using Simple.MVC.Security.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Simple.MVC.Security
{
    public class JSONController : Controller
    {
        public ActionResult Indice()
        {
            return View();
        }
    }
}