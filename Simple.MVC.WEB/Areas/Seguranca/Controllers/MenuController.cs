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
    [Autorize(Roles = "Seguranca.Menu.Indice")]
    public class MenuController : Controller
    {
        private HackathonEntities db = new HackathonEntities();
        /*****************************************************************************************************************************************/
        [Autorize(Roles = "Seguranca.Menu.Indice")]
        public ActionResult Indice(string filtro = "", int page = 1, int pageSize = 0)
        {
            var f = filtro != "" ? filtro : "\0";
            List<SEMenu> mns = db.SEMenu.Where(i => (i.Nome.Contains(f) || i.Controlador.Contains(f) ||
                i.Acao.Contains(f))).OrderBy(o => o.Nome).ToList();
            List<SEMenu> mnsP = Util.paginate(ref mns, ControllerContext.Controller, filtro, page, pageSize);
            return View(mnsP);
        }
        /*****************************************************************************************************************************************/
        [Autorize(Roles = "Seguranca.Menu.Detalhes")]
        public ActionResult Detalhes(int id = 0)
        {
            SEMenu SEMenu = db.SEMenu.Find(id);
            return View(SEMenu);
        }
        /*****************************************************************************************************************************************/
        [Autorize(Roles = "Seguranca.Menu.Criar")]
        public ActionResult Criar()
        {
            ViewBag.IdSistema = new SelectList(db.SESistema.OrderBy(o => o.NomeExibicao), "Id", "NomeExibicao");
            return View();
        }
        /*****************************************************************************************************************************************/
        [HttpPost]
        [Autorize(Roles = "Seguranca.Menu.Criar")]
        public ActionResult Criar(SEMenu SEMenu)
        {
            try
            {
                if (!ModelState.IsValid) { throw new Exception(""); }
                db.SEMenu.Add(SEMenu);
                db.SaveChanges();
                TempData["s"] = "Menu criado com sucesso!";
                return RedirectToAction("Indice");
            }
            catch (Exception ex)
            {
	            TempData["DbEntityValidationResult"] = db.GetValidationErrors();
	            TempData["Exception"] = ex;
                ViewBag.IdSistema = new SelectList(db.SESistema.OrderBy(o => o.NomeExibicao), "Id", "NomeExibicao");
                return View(SEMenu);
            }            
        }
        /*****************************************************************************************************************************************/
        [Autorize(Roles = "Seguranca.Menu.Editar")]
        public ActionResult Editar(int id = 0)
        {
            SEMenu SEMenu = db.SEMenu.Find(id);
            ViewBag.sistemaId = new SelectList(db.SESistema.OrderBy(o => o.NomeExibicao), "Id", "NomeExibicao", SEMenu.IdSistema);
            return View(SEMenu);
        }
        /*****************************************************************************************************************************************/
        [HttpPost]
        [Autorize(Roles = "Seguranca.Menu.Editar")]
        public ActionResult Editar(SEMenu SEMenu)
        {
            try
            {
                if (!ModelState.IsValid) { throw new Exception(""); }
                db.Entry(SEMenu).State = EntityState.Modified;
                db.SaveChanges();
                TempData["s"] = "Informações atualizadas com sucesso!";
                return RedirectToAction("Indice");
            }
            catch (Exception ex)
            {
                TempData["DbEntityValidationResult"] = db.GetValidationErrors();
                TempData["Exception"] = ex;
                ViewBag.sistemaId = new SelectList(db.SESistema.OrderBy(o => o.NomeExibicao), "Id", "NomeExibicao", SEMenu.IdSistema);
                return View(SEMenu);
            }            
        }
        /*****************************************************************************************************************************************/
        [Autorize(Roles = "Seguranca.Menu.Excluir")]
        public ActionResult Excluir(int id = 0)
        {
            SEMenu SEMenu = db.SEMenu.Find(id);
            return View(SEMenu);
        }
        /*****************************************************************************************************************************************/
        [HttpPost, ActionName("Excluir")]
        [Autorize(Roles = "Seguranca.Menu.Excluir")]
        public ActionResult ConfirmarExclusao(int id)
        {
            SEMenu Menu = db.SEMenu.Find(id);
            db.SEMenu.Remove(Menu);
            db.SaveChanges();
            return RedirectToAction("Indice");
        }
    }
}