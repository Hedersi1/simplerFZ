using System;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;
using Simple.MVC.Security.Data;
using Simple.MVC.Security;

namespace Simple.MVC.WEB.Seguranca.Controllers
{
    public partial class SecurityAccountController : Controller
    {
        private HackathonEntities db = new HackathonEntities();
        /*****************************************************************************************************************************************/
        [AllowAnonymous] // id == oaunt
        public ActionResult AcessoExterno(string id = "")
        {
            Session.Clear();
            try
            {
                SEAutenticacao autenticacao = db.SEAutenticacao.Where(a => a.Auth.Equals(id)).FirstOrDefault();
                if (autenticacao != null)
                {
                    // para usar a autenticação dessa maneira é necessário configurar o ExternoSimpleMembershipProvider no web config. 
                    if (WebSecurity.Login(autenticacao.SEUsuario.Login, id))
                    {
                        return RedirectToAction("Indice", "Inicio");
                    }
                }
                else
                {
                    TempData["e"] = "Código de autenticação não encontrado no sistema destino!";
                }
            }
            catch (Exception ex)
            {
                TempData["exception"] = ex;
            }
            SESistema apps = db.SESistema.FirstOrDefault(s => s.Nome == "Simple.MVC.Web.Home");
            Response.Redirect(apps.Caminho + "Account/Login");
            return View();
        }
        /*****************************************************************************************************************************************/
        [Authorize] // id == oaunt
        public ActionResult Redirecionar(string id = "", string pagina = null)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var idDestino = Util.Base64Decode(id);
                SEUsuario pessoa = db.SEUsuario.Where(p => p.Login == User.Identity.Name).FirstOrDefault();
                var sistemaDestino = db.SESistema.Where(s => s.Nome == idDestino).FirstOrDefault();
                var sistemaOrigem = db.SESistema.Where(s => s.Nome == System.Web.Security.Membership.ApplicationName).FirstOrDefault();

                if (sistemaDestino.Caminho.Contains("?auth="))
                {
                    return Redirect(sistemaDestino.Caminho.Replace("?auth=", Util.CreateAuth(User.Identity.Name)));
                }

                if (!string.IsNullOrWhiteSpace(sistemaDestino.PaginaAutenticacao))
                {
                    String oauth = Util.hash(DateTime.Now.ToString() + pessoa.Login);
                    SEAutenticacao autenticacao = new SEAutenticacao { Pagina = pagina, IdSistemaDestino = sistemaDestino.Id, IdSistemaOrigem = sistemaOrigem.Id, IdPessoa = pessoa.Id, Utilizado = 0, Auth = oauth, DataSolicitacao = DateTime.Now, Validade = 60 };
                    db.SEAutenticacao.Add(autenticacao);
                    db.SaveChanges();

                    return Redirect(sistemaDestino.Caminho + sistemaDestino.PaginaAutenticacao + oauth);
                }
                return Redirect(sistemaDestino.Caminho);
            }
            return View();
        }
        /*****************************************************************************************************************************************/
        public ActionResult Sair()
        {
            Session.Abandon();
            WebSecurity.Logout();
            if (System.Web.Security.Membership.ApplicationName != "Simple.MVC.Web.Home")
            {
                SESistema s = @db.SESistema.Where(stm => stm.Nome.Equals("Simple.MVC.Web.Home")).FirstOrDefault();
                return Redirect(s.Caminho + "Account/Sair");
            }
            return RedirectToAction("Login", "Account");
        }
        /*****************************************************************************************************************************************/
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
