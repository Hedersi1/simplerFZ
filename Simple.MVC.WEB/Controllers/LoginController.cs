﻿using System.Web.Mvc;

namespace Simple.MVC.WEB.Controllers
{
    public class LoginController : Controller
    {
        /*****************************************************************************************************************************************/
        [Authorize]
        public ActionResult Indice()
        {
            return View();
        }
        /*****************************************************************************************************************************************/
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        /*****************************************************************************************************************************************/
    }
}
