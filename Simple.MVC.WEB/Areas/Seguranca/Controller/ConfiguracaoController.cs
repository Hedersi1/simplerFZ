using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using Simple.MVC.Security.Data;
using Simple.MVC.Security;

namespace Simple.MVC.WEB.Seguranca.Controllers
{
    [Autorize(Roles = "Seguranca.Configuracao.Indice")]
    public class ConfiguracaoController : Controller
    {
        private HackathonEntities db = new HackathonEntities();
        /*****************************************************************************************************************************************/
        [Autorize(Roles = "Seguranca.Configuracao.Indice")]
        public ActionResult Indice(string filtro = "", int page = 1, int pageSize = 0)
        {
            var f = filtro != "" ? filtro : "\0";
            List<SEConfiguracao> mns = db.SEConfiguracao.OrderByDescending(o => o.NomeExibicao)
                .Where(n => n.Nome.Contains(filtro) || n.NomeExibicao.Contains(filtro) || n.Valor.Contains(filtro)).ToList();
            List<SEConfiguracao> mnsP = Util.paginate(ref mns, ControllerContext.Controller, filtro, page, pageSize);
            return View(mnsP);
        }
        /*****************************************************************************************************************************************/
        [Autorize(Roles = "Seguranca.Configuracao.Editar")]
        public ActionResult Editar(long id = 0)
        {
            SEConfiguracao SEConfiguracao = db.SEConfiguracao.Find(id);
            SEConfiguracao.Valor = "";
            return View(SEConfiguracao);
        }
        /*****************************************************************************************************************************************/
        [HttpPost]
        [Autorize(Roles = "Seguranca.Configuracao.Editar")]
        public ActionResult Editar(SEConfiguracao SEConfiguracao)
        {
            try
            {
                if (!ModelState.IsValid) { throw new Exception(); }
                db.Entry(SEConfiguracao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Indice");
            }
            catch (Exception ex)
            {
                TempData["DbEntityValidationResult"] = db.GetValidationErrors();
                TempData["Exception"] = ex;
                return View(SEConfiguracao);
            }
        }
        /****************************************************************************************************************************************/
        [Autorize(Roles = "Seguranca.Configuracao.Criar")]
        public ActionResult Criar()
        {
            return View();
        }

        /*****************************************************************************************************************************************/
        [HttpPost]
        [Autorize(Roles = "Seguranca.Configuracao.Criar")]
        public ActionResult Criar(SEConfiguracao SEConfiguracao)
        {
            try
            {
                SEConfiguracao.Valor = Util.Encrypt(new ValueSymmetric(SEConfiguracao.Valor)).Text;
                if (!ModelState.IsValid) { throw new Exception(); }
                db.Entry(SEConfiguracao).State = EntityState.Added;
                db.SaveChanges();
                return RedirectToAction("Indice");
            }
            catch (Exception ex)
            {
                TempData["DbEntityValidationResult"] = db.GetValidationErrors();
                TempData["Exception"] = ex;
                return View(SEConfiguracao);
            }
        }
        [Autorize(Roles = "Seguranca.Configuracao.Excluir")]
        public ActionResult Excluir(long id = 0)
        {
            SEConfiguracao SEConfiguracao = db.SEConfiguracao.Find(id);
            return View(SEConfiguracao);
        }
        [HttpPost, ActionName("Excluir")]
        [Autorize(Roles = "Seguranca.Configuracao.Excluir")]
        public ActionResult ConfirmarExclusao(long id)
        {
            SEConfiguracao SEConfiguracao = db.SEConfiguracao.Find(id);
            db.SEConfiguracao.Remove(SEConfiguracao);
            db.SaveChanges();
            TempData["s"] = "Configuracao excluída com sucesso!";
            return RedirectToAction("Indice");
        }
    }
}
