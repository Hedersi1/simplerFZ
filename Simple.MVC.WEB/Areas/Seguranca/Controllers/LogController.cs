using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Simple.MVC.Security.Data;
using Simple.MVC.Security;

namespace Simple.MVC.WEB.Seguranca.Controllers
{
    //[FiltraSessao]
    public class LogController : Controller
    {
        private HackathonEntities db = new HackathonEntities();
        /*****************************************************************************************************************************************/
        public ActionResult Indice(string filtro = "", int page = 1, int pageSize = 0)
        {
            var f = filtro != "" ? filtro : "\0";
            List<SELog> mns = db.SELog.OrderByDescending(o => o.LogData)
                .Where(n => n.Texto.Contains(filtro)
                    || (n.Usuario != null && n.Usuario.Contains(filtro))
                    || (n.Sistema != null && n.Sistema.Contains(filtro))
                    || (n.LogData != null && n.LogData.ToString().Contains(filtro))).ToList();
            List<SELog> mnsP = Util.paginate(ref mns, ControllerContext.Controller, filtro, page, pageSize);
            return View(mnsP);
        }
        /*****************************************************************************************************************************************/
   }
}