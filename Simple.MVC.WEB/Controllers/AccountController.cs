using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Simple.MVC.Security.Data;
using System.Data;
using Simple.MVC.Security;
using WebMatrix.WebData;
using Simple.MVC.WEB.Models;
using System.Net;
using Simple.MVC.Business.Seguranca;

namespace Simple.MVC.WEB.Controllers
{
    public class AccountController : Controller
    {
        private HackathonEntities db = new HackathonEntities();
        
        [AllowAnonymous]
        public ActionResult Indice()
        {
            return RedirectToAction("Indice", "Inicio");
        }
        
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(String ReturnUrl)
        {
            try
            {
                ViewBag.ReturnUrl = ReturnUrl;
                return View();
            }
            catch (Exception ex)
            {
                TempData["DbEntityValidationResult"] = db.GetValidationErrors();
                TempData["Exception"] = ex;
                return View();
            }
        }
        
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model, String ReturnUrl)
        {
            try
            {
                if (!ModelState.IsValid) { throw new Exception(""); }
                if (String.IsNullOrEmpty(ReturnUrl) && Session["ReturnUrl"] != null)
                {
                    ReturnUrl = Session["ReturnUrl"].ToString();
                }

                SEUsuario ps = db.SEUsuario.FirstOrDefault(p => p.Login.Equals(model.login));

                if (WebSecurity.Login((ps == null ? model.login : ps.Login), model.password, persistCookie: model.RememberMe))
                {
                    if (!String.IsNullOrEmpty(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Indice", "Inicio");
                    }

                }
            }
            catch (Exception ex)
            {
                TempData["DbEntityValidationResult"] = db.GetValidationErrors();
                TempData["Exception"] = ex;
            }
            return View(model);
        }
        
        public ActionResult EditMe()
        {
            SEUsuario SEUsuario = db.SEUsuario.FirstOrDefault(p => p.Login.Equals(User.Identity.Name));
            CriarViewBagSexo();
            return View(SEUsuario);
        }
        
        [HttpPost]
        public ActionResult EditMe(SEUsuario SEUsuario)
        {
            try
            {
                CriarViewBagSexo();
                // if (SEUsuario.cpf != null) { SEUsuario.cpf = System.Text.RegularExpressions.Regex.Replace(SEUsuario.cpf, @"[^0-9]", m => ""); }
                // if (SEUsuario.Nome != null) { SEUsuario.Nome = SEUsuario.Nome.Trim(); }
                if (!ModelState.IsValid) { throw new Exception(""); }
                
                SEUsuario pessoaValidado = db.SEUsuario.FirstOrDefault(p => p.Login == User.Identity.Name);
                
                db.SaveChanges();
                Util.log("[" + SEUsuario.Login + "] modificou seus dados cadastrais.", User.Identity.Name);
                TempData["s"] = "Informações Atualizadas com sucesso!";
                return View(pessoaValidado);
            }
            catch (Exception ex)
            {
                TempData["DbEntityValidationResult"] = db.GetValidationErrors();
                TempData["Exception"] = ex;
                return View(SEUsuario);
            }
        }
        
        [HttpGet]
        public ActionResult addTel()
        {
            return RedirectToAction("Indice");
        }
        
        [AllowAnonymous]
        public ActionResult Sair(string ReturnUrl)
        {
            Session.Abandon();
            WebSecurity.Logout();
            return RedirectToAction("Login", "Account");
        }
        
        [HttpGet]
        public ActionResult MudarSenha()
        {
            return View();
        }
        
        [HttpPost]
        [Authorize]
        public ActionResult MudarSenha(LocalPasswordModel model, String ReturnUrl)
        {
            try
            {
                if (!ModelState.IsValid) { throw new Exception(""); }
                if (WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    TempData["s"] = "A senha foi modificada com sucesso!";
                    return RedirectToAction("Indice", "Home");
                }
                else
                {
                    TempData["e"] = "A senha anterior informada está incorreta!";
                }
            }
            catch (Exception ex)
            {
                TempData["DbEntityValidationResult"] = db.GetValidationErrors();
                TempData["Exception"] = ex;
            }
            return View(model);
        }
        
        [HttpGet]
        [AllowAnonymous]
        public ActionResult RecuperarSenhaTK(string tk)
        {
            SEAutenticacao auth = db.SEAutenticacao.Find(Simple.MVC.Security.Data.SimpleMembershipProvider.ValidateToken(tk));
            if (auth != null)
            {
                SEUsuario SEUsuario = db.SEUsuario.Find(auth.IdPessoa);
                String novaSenha = Util.geraSenha();
                if (WebSecurity.ResetPassword(tk, novaSenha))
                {
                    string corpo = "<h2>Olá " + SEUsuario.Login
                        + "</h2><br />Você redefiniu sua senha com sucesso!"
                        + "<br /><br />Sua nova senha: " + novaSenha
                        + "<br />Seu Id: " + SEUsuario.Login
                        + "<br /><br />Clique <a href='"+ db.SESistema.FirstOrDefault(s => s.Nome.Equals("Simple.MVC.Web.Home")).Caminho + 
                        "'> aqui </a> para acessar a Central de Aplicativos Simple: ";

                    Util.enviaEmail("email@padrao.com.br", corpo, "[Simple] - Nova senha e instruções para acesso");
                    TempData["s"] = "Uma nova senha foi gerada e as instruções para acesso foram enviadas para seu E-mail Alternativo.";
                }
                else
                {
                    TempData["e"] = ("Ocorreu um erro no processo. Procure a equipe de suporte!");
                }
            }
            else
            {
                TempData["e"] = ("Token inválido. Procure a equipe de suporte!");
            }
            return RedirectToAction("Login");
        }
        
        [HttpPost]
        [AllowAnonymous]
        public ActionResult RecuperarSenha(string ident = "@")
        {
            try
            {
                SEUsuario SEUsuario = null;
                if (SEUsuario == null) { SEUsuario = db.SEUsuario.FirstOrDefault(p => p.Login.Equals(ident)); }

                if (SEUsuario != null)
                {
                    throw new Exception("Desculpe-nos " + SEUsuario.Login +
                            ", apesar de termos encontrado o seu cadastro, infelizmente não existe um E-mail Alternativo cadastrado no seu perfil." +
                            " Não será possível recuperar sua senha utilizando esta ferramenta. Você precisará recorrer à equipe de suporte.");
                }
                else
                {
                    TempData["e"] = ("Não localizamos o seu cadastro! Caso necessário, entre em contato com o suporte.");
                }
            }
            catch (Exception ex)
            {
                TempData["DbEntityValidationResult"] = db.GetValidationErrors();
                TempData["Exception"] = ex;
            }
            return RedirectToAction("Login");
        }
        
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        private void CriarViewBagSexo(string valor = "")
        {
            var list = new List<dynamic>();
            list.Add(new { Id = "", Text = "" });
            list.Add(new { Id = "Masculino", Text = "Masculino" });
            list.Add(new { Id = "Feminino", Text = "Feminino" });
            ViewBag.Sexo = new SelectList(list, "Id", "Text", valor);
        }

        public ActionResult Autorizacao()
        {
            var papel = Request.Params["papel"];

            var autorizacaoConfig = AutorizacaoConfiguracaoRepository.FirstOrDefault(papel);

            if(autorizacaoConfig == null)
                return View("ConfiguracaoNaoEncontrada");

            return View(new AutorizacaoModel { papel = papel, formId = Request.Params["formId"] });
        }

        public ActionResult AutorizarAcao(AutorizacaoModel model)
        {
            //Verifica se o login e senha esta preenchido
            if (string.IsNullOrEmpty(model.login) || string.IsNullOrEmpty(model.password))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable);
            }

            //Verifica se o login e senha estao corretos
            if(!Security.Data.SimpleMembershipProvider.ValidarUsuario(model.login, model.password))
            {
                return new HttpStatusCodeResult(HttpStatusCode.PreconditionFailed);
            }

            //Verifica se a pessoa tem o papel
            if(!Security.Data.SimpleMembershipProvider.UsuarioPossuiPapel(model.login, model.papel))
            {
                return new HttpStatusCodeResult(HttpStatusCode.MethodNotAllowed);
            }

            int idUsuarioAutorizador = 0, idUsuarioLogado = 0;
            var login = User.Identity.Name;

            using (HackathonEntities db = new HackathonEntities())
            {

                idUsuarioAutorizador = db.SEUsuario.FirstOrDefault(x => x.Login == model.login).Id;
                idUsuarioLogado = db.SEUsuario.FirstOrDefault(x => x.Login == login).Id;
            }

            var autorizacaoConfig = AutorizacaoConfiguracaoRepository.FirstOrDefault(model.papel);

            var autorizacao = new Autorizacao { Data = DateTime.Now, IdAutorizacaoConfiguracao = autorizacaoConfig.Id, IdUsuarioAutorizador = idUsuarioAutorizador, IdUsuarioLogado = idUsuarioLogado, Ip = Request.UserHostAddress };
            AutorizacaoRepository.Save(autorizacao);

            return Json(new { idAutorizacao = autorizacao.Id }, JsonRequestBehavior.AllowGet);
        }
    }
}