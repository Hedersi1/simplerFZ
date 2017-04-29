using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Simple.MVC.Security.Data;
using Simple.MVC.Security;

namespace Simple.MVC.WEB.Seguranca.Controllers
{
    [Autorize(Roles = "Seguranca.Sistema.Indice")]
    public class SistemaController : Controller
    {
        private HackathonEntities db = new HackathonEntities();
        /*****************************************************************************************************************************************/
        [Autorize(Roles = "Seguranca.Sistema.Indice")]
        public ActionResult Indice(string filtro = "", int page = 1, int pageSize = 0)
        {
            var f = filtro != "" ? filtro : "\0";
            List<SESistema> mns = db.SESistema.Where(i => (i.NomeExibicao.Contains(f) ||
                i.Descricao.Contains(f))).OrderBy(o => o.NomeExibicao).ToList();
            List<SESistema> mnsP = Util.paginate(ref mns, ControllerContext.Controller, filtro, page, pageSize);
            return View(mnsP);
        }
        /*****************************************************************************************************************************************/
        [Autorize(Roles = "Seguranca.Sistema.Detalhes")]
        public ActionResult Detalhes(long id = 0)
        {
            SESistema SESistema = db.SESistema.Find(id);
            if (SESistema == null)
            {
                return HttpNotFound();
            }
            return View(SESistema);
        }
        /*****************************************************************************************************************************************/
        [Autorize(Roles = "Seguranca.Sistema.Criar")]
        public ActionResult Criar()
        {
            return View();
        }
        /*****************************************************************************************************************************************/
        [HttpPost]
        [Autorize(Roles = "Seguranca.Sistema.Editar")]
        public ActionResult Editar(SESistema SESistema)
        {
            if (ModelState.IsValid)
            {
                db.Entry(SESistema).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Indice");
            }

            return View(SESistema);
        }

        /*****************************************************************************************************************************************/
        [Autorize(Roles = "Seguranca.Sistema.Editar")]
        public ActionResult Editar(long id = 0)
        {
            SESistema sistema = db.SESistema.Find(id);

            if (sistema == null)
            {
                return HttpNotFound();
            }
            return View(sistema);
        }

        /*****************************************************************************************************************************************/
        [HttpPost]
        [Autorize(Roles = "Seguranca.Sistema.Criar")]
        public ActionResult Criar(SESistema SESistema)
        {
            if (ModelState.IsValid)
            {
                db.SESistema.Add(SESistema);
                db.SaveChanges();
                return RedirectToAction("Indice");
            }

            return View(SESistema);
        }

        /*****************************************************************************************************************************************/
        [Autorize(Roles = "Seguranca.Sistema.Excluir")]
        public ActionResult Excluir(long id = 0)
        {
            SESistema SESistema = db.SESistema.Find(id);
            if (SESistema == null)
            {
                return HttpNotFound();
            }
            return View(SESistema);
        }
        /*****************************************************************************************************************************************/
        [HttpPost, ActionName("Excluir")]
        [Autorize(Roles = "Seguranca.Sistema.Excluir")]
        public ActionResult ConfirmarExclusao(long id)
        {
            SESistema SESistema = db.SESistema.Find(id);
            db.SESistema.Remove(SESistema);
            db.SaveChanges();
            return RedirectToAction("Indice");
        }
        /*****************************************************************************************************************************************/
    }
}