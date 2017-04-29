using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Simple.MVC.Security.Data;

namespace Simple.MVC.WEB.Seguranca.Controllers
{
    public class AjaxController : Controller
    {
        private HackathonEntities db = new HackathonEntities();
        /*****************************************************************************************************************************************/
        [HttpGet]
        public ActionResult GetRolesForSystem(int Id)
        {
            IEnumerable<SEPapel> papeis = db.SEPapel.Where(p => p.IdSistema.Equals(Id));
            if (papeis.Count() == 0)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(
                    papeis.Select(p => new { Id = p.Id, Nome = p.Nome }), JsonRequestBehavior.AllowGet);
            }
        }
        /*****************************************************************************************************************************************/
        [HttpGet]
        public ActionResult GetGroupsForPeople(int Id)
        {
            IEnumerable<SEGrupo> grupos = db.SEGrupoUsuario.Where(g => g.SEGrupo != null && g.SEUsuario.Id == Id).Select(gp => gp.SEGrupo).ToList();
            if (grupos.Count() == 0)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(
                    grupos.Select(p => new { Id = p.Id, Nome = p.Nome }), JsonRequestBehavior.AllowGet);
            }
        }
        /***************************************************************************************************************/
        public ActionResult GetUsuario(string term)
        {
            var nomes = db.SEUsuario.Where(p => p.Login.Contains(term))
                .Select(A => new
                {
                    Id = A.Id,
                    Nome = A.Login.ToUpper(),
                    Login = A.Login,
                    Email = "email@padrao.com.br",
                    CPF = "000.000.000-00"
                });

            if (nomes.Count() == 0)
            {
                var nada = new object[] { new { Id = "", RoleName = "", Nome = "Não encontrado!", Login = "Nenhuma pessoa foi encontrada, tente outros termos de pesquisa..." } };
                return Json(nada, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(nomes, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetPessoa(string term)
        {
            return GetUsuario(term);
        }
        /***************************************************************************************************************/
        public JsonResult GetMenusForSystem(int Id)
        {
            if (Id != 0)
            {
                var MenusSite = db.SEMenu.Where(m => m.IdSistema == Id && m.IdMenuPai == null).OrderBy(o => o.Nome);
                List<SelectListItem> l = new List<SelectListItem>();
                foreach (var item in MenusSite)
                {
                    l.Add(new SelectListItem()
                    {
                        Value = item.Id.ToString(),
                        Text = item.Nome
                    });
                    if (item.SEMenu1.Count > 0)
                    {
                        foreach (var subitem in item.SEMenu1.OrderBy(o => o.Nome))
                        {
                            l.Add(new SelectListItem() { Value = subitem.Id.ToString(), Text = "......" + subitem.Nome });
                        }
                    }
                }
                SelectList items = new SelectList(l, "Value", "Text");
                if (items.Count() == 0)
                {
                    var nada = new object[] { new { Value = "0", Text = "O sistema selecionado não possui menus!" } };
                    return new JsonResult() { Data = nada, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    return new JsonResult() { Data = items, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            else
            {
                return null;
            }
        }
        /*****************************************************************************************************************************************/
        public JsonResult CarregaSEPessoasSEGrupo(int IdGrupo)
        {
            var resultado = db.SEGrupoUsuario.Where(s => s.IdGrupo == IdGrupo).Select(p => new SelectListItem() { Value = p.SEUsuario.Id.ToString(), Text = p.SEUsuario.Login }).OrderBy(p => p.Text).ToList();
            return new JsonResult() { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /*****************************************************************************************************************************************/
    }
}