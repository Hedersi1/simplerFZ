using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Simple.MVC.Security.Data;
using Simple.MVC.Security;

namespace Simple.MVC.WEB.Seguranca.Controllers
{
    [Autorize(Roles = "Seguranca.Papel.Indice")]
    public class PapelController : Controller
    {
        private HackathonEntities db = new HackathonEntities();
        /*****************************************************************************************************************************************/
        [Autorize(Roles = "Seguranca.Papel.Indice")]
        public ActionResult Indice(string filtro = "", int page = 1, int pageSize = 0)
        {
            var f = filtro != "" ? filtro : "\0";
            List<SEPapel> mns = db.SEPapel.Where(i => (i.Nome.Contains(f) || i.SESistema.NomeExibicao.Contains(f) ||
                i.Descricao.Contains(f))).OrderBy(o => o.Nome).ToList();
            List<SEPapel> mnsP = Util.paginate(ref mns, ControllerContext.Controller, filtro, page, pageSize);
            return View(mnsP);
        }
        /*****************************************************************************************************************************************/
        [Autorize(Roles = "Seguranca.Papel.Detalhes")]
        public ActionResult Detalhes(long id = 0)
        {
            SEPapel SEPapel = db.SEPapel.Find(id);
            return View(SEPapel);
        }
        /*****************************************************************************************************************************************/
        [Autorize(Roles = "Seguranca.Papel.Criar")]
        public ActionResult Criar()
        {
            ViewBag.IdSistema = new SelectList(db.SESistema.OrderBy(o=>o.NomeExibicao), "Id", "NomeExibicao");
            return View();
        }
        /*****************************************************************************************************************************************/
        [HttpPost]
        [Autorize(Roles = "Seguranca.Papel.Criar")]
        public ActionResult Criar(SEPapel SEPapel)
        {
            try
            {
                if (!ModelState.IsValid) { throw new Exception(""); }
                db.SEPapel.Add(SEPapel);
                db.SaveChanges();
                TempData["s"] = "Registro criado com sucesso!";
                return RedirectToAction("Indice");
            }
            catch (Exception ex)
            {
                TempData["DbEntityValidationResult"] = db.GetValidationErrors();
                TempData["Exception"] = ex;
            }
            ViewBag.IdSistema = new SelectList(db.SESistema, "Id", "NomeExibicao", SEPapel.IdSistema);
            return View(SEPapel);
        }
        /*****************************************************************************************************************************************/
        [Autorize(Roles = "Seguranca.Papel.Editar")]
        public ActionResult Editar(long id = 0)
        {
            SEPapel SEPapel = db.SEPapel.Find(id);
            ViewBag.IdSistema = new SelectList(db.SESistema, "Id", "NomeExibicao", SEPapel.IdSistema);
            return View(SEPapel);
        }
        /*****************************************************************************************************************************************/
        [HttpPost]
        [Autorize(Roles = "Seguranca.Papel.Editar")]
        public ActionResult Editar(SEPapel SEPapel)
        {
            try
            {
                if (!ModelState.IsValid) { throw new Exception(""); }
                db.Entry(SEPapel).State = EntityState.Modified;
                db.SaveChanges();
                TempData["s"] = "Registro modificado com sucesso!";
                return RedirectToAction("Indice");
            }
            catch (Exception ex)
            {
                TempData["DbEntityValidationResult"] = db.GetValidationErrors();
                TempData["Exception"] = ex;
                ViewBag.IdSistema = new SelectList(db.SESistema, "Id", "NomeExibicao", SEPapel.IdSistema);
                return View(SEPapel);
            }
        }
        /*****************************************************************************************************************************************/
        [Autorize(Roles = "Seguranca.Papel.Excluir")]
        public ActionResult Excluir(long id = 0)
        {
            SEPapel SEPapel = db.SEPapel.Find(id);
            return View(SEPapel);
        }
        /*****************************************************************************************************************************************/
        [HttpPost, ActionName("Excluir")]
        [Autorize(Roles = "Seguranca.Papel.Excluir")]
        public ActionResult ConfirmarExclusao(long id)
        {
            SEPapel SEPapel = db.SEPapel.Find(id);
            db.SEPapel.Remove(SEPapel);
            db.SaveChanges();
            TempData["s"] = "Registro excluído com sucesso!";
            return RedirectToAction("Indice");
        }
        /*****************************************************************************************************************************************/
        public ActionResult PapelGrupo()
        {
            ViewBag.sys = new SelectList(db.SESistema.Where(a => a.SEPapel.Count > 0).OrderBy(o => o.NomeExibicao), "Id", "NomeExibicao");
            ViewBag.groups = new SelectList(db.SEGrupo.OrderBy(w=>w.Nome), "Id", "Nome");
            return View();
        }
        /*****************************************************************************************************************************************/
        [HttpPost]
		[Autorize(Roles = "Seguranca.Papel.Excluir")]
        public JsonResult PapelGrupo(string[] idsPapeis, int IdGrupo, long idSistema)        {
            db.SEPapelGrupo.Where(pg => pg.IdGrupo == IdGrupo && pg.SEPapel.IdSistema == idSistema).ToList().ForEach(i => db.SEPapelGrupo.Remove(i));
            if (idsPapeis != null)
            {
                foreach (var item in idsPapeis)
                {
                    long papel = long.Parse(item);
                    db.SEPapelGrupo.Add(new SEPapelGrupo() { IdGrupo = IdGrupo, IdPapel = papel });
                }
            }
            db.SaveChanges();
            return this.Json("ok", JsonRequestBehavior.AllowGet);
        }
        /*****************************************************************************************************************************************/
        [HttpGet]
		[Autorize(Roles = "Seguranca.Papel.Excluir")]
        public JsonResult PapeisDeUmGrupo(int IdGrupo, long idSistema)        {
            var papeis = db.SEPapelGrupo
                .Where(p => p.IdGrupo == IdGrupo && p.SEPapel.IdSistema == idSistema)
                .Select(pg => new { Id = pg.SEPapel.Id});
            return this.Json(papeis, JsonRequestBehavior.AllowGet);
        }
        /*****************************************************************************************************************************************/
    }
}