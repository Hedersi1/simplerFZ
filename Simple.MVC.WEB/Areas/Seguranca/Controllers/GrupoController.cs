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
    [Autorize(Roles = "Seguranca.Grupo.Indice")]
    public class GrupoController : Controller
    {
        private HackathonEntities db = new HackathonEntities();
        /*****************************************************************************************************************************************/
        [Autorize(Roles = "Seguranca.Grupo.Indice")]
        public ActionResult Indice(string filtro = "", int page = 1, int pageSize = 0)
        {
            var f = filtro != "" ? filtro : "\0";
            List<SEGrupo> mns = db.SEGrupo.OrderByDescending(o => o.Nome)
                .Where(n => n.Nome.Contains(filtro) || n.Origem.Contains(filtro)).ToList();
            List<SEGrupo> mnsP = Util.paginate(ref mns, ControllerContext.Controller, filtro, page, pageSize);
            return View(mnsP);
        }
        /*****************************************************************************************************************************************/
        [Autorize(Roles = "Seguranca.Grupo.Detalhes")]
        public ActionResult Detalhes(long id = 0)
        {
            SEGrupo SEGrupo = db.SEGrupo.Find(id);
            return View(SEGrupo);
        }
        /*****************************************************************************************************************************************/
        [Autorize(Roles = "Seguranca.Grupo.Criar")]
        public ActionResult Criar()
        {
            return View();
        }
        /*****************************************************************************************************************************************/
        [HttpPost]
        [Autorize(Roles = "Seguranca.Grupo.Criar")]
        public ActionResult Criar(SEGrupo SEGrupo)
        {
            try
            {
                if (!ModelState.IsValid) { throw new Exception(""); }
                SEGrupo.Origem = "SEGURANCA";

                db.SEGrupo.Add(SEGrupo);
                db.SaveChanges();
                Util.log("SEGrupo: [" + SEGrupo.Nome + "] criado pelo usuário [" + User.Identity.Name + "] em [" + DateTime.Now + "]", User.Identity.Name);
                TempData["s"] = "Grupo criado com sucesso!";
                return RedirectToAction("Indice");
            }
            catch (Exception ex)
            {
                TempData["DbEntityValidationResult"] = db.GetValidationErrors();
                TempData["Exception"] = ex;
            }
            return View(SEGrupo);
        }
        /*****************************************************************************************************************************************/
        [Autorize(Roles = "Seguranca.Grupo.Editar")]
        public ActionResult Editar(int id)
        {
            SEGrupo SEGrupo = db.SEGrupo.Find(id);
            return View(SEGrupo);
        }
        /*****************************************************************************************************************************************/
        [HttpPost]
        [Autorize(Roles = "Seguranca.Grupo.Editar")]
        public ActionResult Editar(SEGrupo SEGrupo)
        {
            try
            {
                if (!ModelState.IsValid) { throw new Exception(""); }
                db.Entry(SEGrupo).State = EntityState.Modified;
                db.SaveChanges();

                TempData["s"] = "Informações salvas com sucesso!";
                return RedirectToAction("Indice");
            }
            catch (Exception ex)
            {
                TempData["DbEntityValidationResult"] = db.GetValidationErrors();
                TempData["Exception"] = ex;
                return View(SEGrupo);
            }
        }
        /*****************************************************************************************************************************************/
        [Autorize(Roles = "Seguranca.Grupo.Excluir")]
        public ActionResult Excluir(int id)
        {
            SEGrupo SEGrupo = db.SEGrupo.Find(id);
            return View(SEGrupo);
        }
        /*****************************************************************************************************************************************/
        [HttpPost, ActionName("Excluir")]
        [Autorize(Roles = "Seguranca.Grupo.Excluir")]
        public ActionResult ConfirmarExclusao(int id)
        {
            SEGrupo SEGrupo = db.SEGrupo.Find(id);
            db.SEGrupo.Remove(SEGrupo);
            db.SaveChanges();

            TempData["s"] = "Grupo excluído com sucesso!";
            return RedirectToAction("Indice");
        }
        /*****************************************************************************************************************************************/
        [Autorize(Roles = "Seguranca.Grupo.Excluir")]
        public ActionResult GrupoPessoa(int grupos, string filtro = "")
        {
            ViewBag.grupo = db.SEGrupo.Find(grupos);
            if (ViewBag.grupo != null)
            {
                ViewBag.grupos = new SelectList(db.SEGrupo.Select(g => new SelectListItem() { Text = g.Id.ToString(), Value = g.Nome }).OrderBy(o => o.Text), "Value", "Text", grupos);
                return View(db.SEGrupoUsuario.Where(p => p.IdGrupo == grupos && p.SEUsuario.Login.Contains(filtro)).OrderBy(o => o.SEUsuario.Login));
            }
            TempData["e"] = "Nenhum grupo encontrado!";
            return RedirectToAction("Indice");
        }
        /*****************************************************************************************************************************************/
        [Autorize(Roles = "Seguranca.Grupo.Excluir")]
        public ActionResult FiltroGrupoPessoa(string search, string sort, string order, int IdGrupo, int? limit = 10, int? offset = 0)
        {
            int quantidade = limit ?? 10;
            int pagina= offset ?? 0;
            
            IEnumerable<SEGrupoUsuario> gruposSEPessoa = null;

            if (IdGrupo != 0)
            {
                gruposSEPessoa = db.SEGrupoUsuario.Where(p => p.IdGrupo == IdGrupo).OrderBy(o => o.SEUsuario.Login);

                if (!string.IsNullOrEmpty(search))
                {
                    gruposSEPessoa = gruposSEPessoa.Where(x => x.SEUsuario.Login.ToLower().Contains(search.ToLower()));
                }

                int totalItens = gruposSEPessoa.Count();
                gruposSEPessoa = gruposSEPessoa.Skip(pagina).Take(quantidade).ToList();
                var pessoas = gruposSEPessoa.Select(x => new { Nome = x.SEUsuario.Login }).ToList();
                return Json(new { total = totalItens, rows = pessoas }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        /*****************************************************************************************************************************************/
        [HttpGet]
        [Autorize(Roles = "Seguranca.Grupo.Excluir")]
        public ActionResult PessoaGrupo(int id= 0)
        {
            if (id != 0)
                ViewBag.SEUsuario = db.SEUsuario.Find(id);

            ViewBag.grupos = db.SEGrupo.OrderBy(o => o.Nome);
            return View();
        }

        /*****************************************************************************************************************************************/

        [HttpPost]
        [Autorize(Roles = "Seguranca.Grupo.Excluir")]
        public JsonResult AdicionarGrupoPessoa(int IdUsuario, int IdGrupo)
        {
            try
            {
                if (db.SEGrupoUsuario.Where(a => a.IdUsuario == IdUsuario && a.IdGrupo == IdGrupo).Count() == 0)
                {
                    SEGrupoUsuario gp = new SEGrupoUsuario()
                    {
                        IdUsuario = IdUsuario,
                        IdGrupo = IdGrupo
                    };
                    db.SEGrupoUsuario.Add(gp);
                    db.SaveChanges();
                    return new JsonResult
                    {
                        Data = new { text = "O Usuário foi incluído ao grupo.", type = "success"  }
                    };
                }
                else
                {
                    return new JsonResult
                    {
                        Data = new { text = "O usuário já está no grupo informado.", type = "error" }
                    };
                }
            }
            catch (Exception ex)
            {
                return new JsonResult
                {
                    Data = new { text = ex.Message, type = "error" }
                };
            }
        }
        /*****************************************************************************************************************************************/
        [Autorize(Roles = "Seguranca.Grupo.Excluir")]
        public ActionResult GetUsuario(string term)
        {
            IEnumerable<SEUsuario> pessoas = db.SEUsuario.Where(p => p.Login.Contains(term));

            if (pessoas.Count() == 0)
            {
                var nada = new object[] { new { Id = "", Nome = "Não encontrado!", Login = "Nenhuma ocorrencia foi encontrada, tente outros termos de pesquisa...", Email = "" } };
                return this.Json(nada, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json(
                    pessoas.Select(p => new { Id = p.Id, Nome = p.Login, Login = p.Login, Email = "email@padrao.com.br" })
                    , JsonRequestBehavior.AllowGet);
            }
        }
        /*****************************************************************************************************************************************/
        [HttpPost]
        [Autorize(Roles = "Seguranca.Grupo.Excluir")]
        public ActionResult ExcluirGrupoPessoa(long IdUsuario, int IdGrupo)
        {
            try
            {
                SEGrupoUsuario gp = db.SEGrupoUsuario.Where(a => a.IdUsuario == IdUsuario && a.IdGrupo == IdGrupo).First();
                db.SEGrupoUsuario.Remove(gp);

                db.SaveChanges();
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Conflict);
            }
        }
        /*****************************************************************************************************************************************/
    }
}