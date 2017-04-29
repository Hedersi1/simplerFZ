using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Simple.MVC.Security.Data;

namespace Simple.MVC.WEB.Seguranca.Controllers
{
    
    [Autorize(Roles = "Seguranca.MenuGrupo.Indice")]
    public class MenuGrupoController : Controller
    {
        private HackathonEntities db = new HackathonEntities();
        /*****************************************************************************************************************************************/
        [Autorize(Roles = "Seguranca.MenuGrupo.Indice")]
        public ActionResult Indice()
        {
            ViewBag.grupos = new SelectList(db.SEGrupo.OrderBy(o => o.Nome), "Id", "Nome");
            ViewBag.sistemas = new SelectList(db.SESistema.Where(s => s.SEMenu.Count > 0).OrderBy(o => o.NomeExibicao), "Id", "NomeExibicao");
            return View();
        }
        /*****************************************************************************************************************************************/
        [HttpPost]
        [Autorize(Roles = "Seguranca.MenuGrupo.Excluir")]
        public JsonResult Excluir(long id)
        {
            SEMenuGrupo mg = db.SEMenuGrupo.Find(id);
            //long pai = mg.SEMenu.IdMenuPai ?? 0;
            SEMenuGrupo Pai = db.SEMenuGrupo.Where(w => w.IdGrupo == mg.IdGrupo && w.IdMenu == mg.SEMenu.IdMenuPai).FirstOrDefault();
            db.SEMenuGrupo.Remove(mg);
            long idDoPai = 0;

            //Se não restou irmãos, então deleta o pai

            IEnumerable<SEMenuGrupo> irmaos = db.SEMenuGrupo.Where(a => a.IdGrupo == mg.IdGrupo && a.IdMenu != mg.IdMenu && a.SEMenu.IdMenuPai == Pai.SEMenu.Id);
            if (irmaos.Count() == 0)
            {
                db.SEMenuGrupo.Remove(Pai);
                idDoPai = Pai.IdMenu;
            }
            
            db.SaveChanges();
            return this.Json(new { text = "Menu Excluído do Grupo.", value = "success", deletaPai = idDoPai }, JsonRequestBehavior.AllowGet);
        }
        /*****************************************************************************************************************************************/
        [Autorize(Roles = "Seguranca.MenuGrupo.Criar")]
        public ActionResult MenuSistema(long Id)
        {
            IEnumerable<SEMenu> menus = db.SEMenu.Where(m => m.IdSistema == Id);
            return View(menus);
        }
        /*****************************************************************************************************************************************/
        [Autorize(Roles = "Seguranca.MenuGrupo.Criar")]
        public ActionResult MenuGrupoSistema(long IdSistema, int IdGrupo)
        {
            IEnumerable<SEMenuGrupo> menus = db.SEMenuGrupo
                .Where(mg => mg.IdGrupo == IdGrupo)
                .Where(m => m.SEMenu.IdSistema == IdSistema);
            return View(menus);
        }
        /***************************************************************************************************************/
        [HttpGet]
        [Autorize(Roles = "Seguranca.MenuGrupo.Criar")]
        public JsonResult AdicionarMenuGrupo(long IdMenu, int IdGrupo)
        {
            try
            {
                if (IdMenu != 0 && IdGrupo != 0)
                {
                    SEMenuGrupo jaTem = db.SEMenuGrupo.Where(mg => mg.IdMenu == IdMenu && mg.IdGrupo == IdGrupo).FirstOrDefault();
                    if (jaTem == null)
                    {
                        SEMenu menu = db.SEMenu.Find(IdMenu);
                        if (menu.IdMenuPai != null)
                        {
                            SEMenuGrupo temOpai = db.SEMenuGrupo.Where(mg => mg.IdGrupo == IdGrupo && mg.SEMenu.Id == menu.IdMenuPai).FirstOrDefault();
                            if (temOpai == null)
                            {
                                db.SEMenuGrupo.Add(new SEMenuGrupo()
                                {
                                    IdGrupo = IdGrupo,
                                    IdMenu = (long)menu.IdMenuPai
                                });
                            }
                        }
                        else
                        {
                            return this.Json(new { text = "Os menus principais não precisam ser adicionados, basta adicionar os menus internos.", value = "error"}, JsonRequestBehavior.AllowGet);
                        }
                        db.SEMenuGrupo.Add(new SEMenuGrupo()
                        {
                            IdGrupo = IdGrupo,
                            IdMenu = IdMenu
                        });
                        db.SaveChanges();
                        return this.Json(new { text = "Menu adicionado ao Grupo.", value = "success", idDoPai = menu.IdMenuPai }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return this.Json(new { text = "O Grupo já possui este Menu.", value = "error" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return this.Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return this.Json(new { text = ex.Message, value = "error" }, JsonRequestBehavior.AllowGet);
            }
        }
        /*****************************************************************************************************************************************/
    }
}