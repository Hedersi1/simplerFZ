using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Simple.MVC.Security.Data;
using Simple.MVC.Security;
using WebMatrix.WebData;

namespace Simple.MVC.WEB.Seguranca.Controllers
{
    [Autorize(Roles = "Seguranca.Pessoa.Indice")]
    public class UsuarioController : Controller
    {
        private HackathonEntities db = new HackathonEntities();
        
        [Autorize(Roles = "Seguranca.Pessoa.Indice")]
        public ActionResult Indice(string filtro = "", int page = 1, int pageSize = 0)
        {
            var f = filtro != "" ? filtro : "\0";
            List<SEUsuario> mns = db.SEUsuario.Where(i => i.Login.Contains(f)).OrderBy(o => o.Login).ToList();
            List<SEUsuario> mnsP = Util.paginate(ref mns, ControllerContext.Controller, filtro, page, pageSize);
            return View(mnsP);
        }
        
        [Autorize(Roles = "Seguranca.Pessoa.Detalhes")]
        public ActionResult Detalhes(long id = 0)
        {
            SEUsuario SEUsuario = db.SEUsuario.Find(id);
            return View(SEUsuario);
        }
        
        [Autorize(Roles = "Seguranca.Pessoa.Criar")]
        public ActionResult Criar()
        {
            CriarViewBagSexo();
            return View();
        }
        
        [HttpPost]
        [Autorize(Roles = "Seguranca.Pessoa.Criar")]
        public ActionResult Criar(SEUsuarioModelo Usuario)
        {
            try
            {
                CriarViewBagSexo();

                if (!ModelState.IsValid)
                {
                    throw new Exception("");
                }
                Usuario.Situacao = 1;
                WebSecurity.CreateUserAndAccount(null, null, propertyValues: new
                {
                    SEUsuario = Usuario
                });
                TempData["s"] = "Usuário cadastrado com sucesso!";
                return RedirectToAction("Criar");
            }
            catch (Exception ex)
            {
                TempData["DbEntityValidationResult"] = db.GetValidationErrors();
                TempData["Exception"] = ex;
            }
            return View(Usuario);
        }
        
        [Autorize(Roles = "Seguranca.Pessoa.Editar")]
        public ActionResult Editar(long id = 0)
        {
            
            SEUsuario SEUsuario = db.SEUsuario.Find(id);
            CriarViewBagSituacao(SEUsuario.Situacao);

            if (SEUsuario == null)
            {
                TempData["e"] = "Usuário não encontrado, favor tentar novamente!";
                return RedirectToAction("Indice");
            }

            return View(new SEUsuarioModelo
            {
                Login = SEUsuario.Login,
                MudaSenhaProximoLogon = SEUsuario.MudaSenhaProximoLogon,
                Id = SEUsuario.Id,
                Situacao = SEUsuario.Situacao
               
            });
        }

        
        [HttpPost]
        [Autorize(Roles = "Seguranca.Pessoa.Editar")]
        public ActionResult Editar(SEUsuarioModelo SEUsuario)
        {
            try
            {
                CriarViewBagSituacao(SEUsuario.Situacao);

                if (!ModelState.IsValid) { throw new Exception(""); }

                var ObjPessoa = db.SEUsuario.Find(SEUsuario.Id);
                if (ObjPessoa != null)
                {
                    ObjPessoa.MudaSenhaProximoLogon = SEUsuario.MudaSenhaProximoLogon;
                    ObjPessoa.Situacao = SEUsuario.Situacao;

                    db.Entry(ObjPessoa).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["s"] = "Informações atualizadas com sucesso!";
                    return RedirectToAction("Indice");
                }
                else
                {
                    TempData["e"] = "Usuário não encontrado, favor tentar novamente!";
                }
            }
            catch (Exception ex)
            {

                TempData["DbEntityValidationResult"] = db.GetValidationErrors();
                TempData["Exception"] = ex;
            }
            return View(SEUsuario);
        }

        [HttpPost]
        [Autorize(Roles = "Seguranca.Pessoa.Editar")]
        public ActionResult MudarSenha(SEUsuarioAlterarSenhaModelo SEUsuario)
        {

            var retorno = new { text = "", type = "" };
            try
            {
                var ObjPessoa = db.SEUsuario.Find(SEUsuario.Id);

                if (!ModelState.IsValid) {
                    var erros = "";
                    var errorKeys = (from item in ModelState where item.Value.Errors.Any() select item.Key).ToList();
                    foreach (var key in errorKeys)
                    {
                        erros += ModelState[key].Errors[0].ErrorMessage +"<br />";
                    }
                    retorno = new { text = "Erro de validação. Erro: " + erros, type = "error" };
                    return new JsonResult { Data = retorno };
                }
                
                if (ObjPessoa != null)
                {
                    ObjPessoa.MudaSenhaProximoLogon = SEUsuario.MudaSenhaProximoLogon;
                    if(SEUsuario.GerarSenha)
                    {
                        var senha = Util.geraSenha();
                        ObjPessoa.Senha = Util.hash(Util.geraSenha());
                        //mandar senha por e-mail
                    }else
                    {
                        ObjPessoa.Senha = Util.hash(SEUsuario.Senha);
                    }

                    db.Entry(ObjPessoa).State = EntityState.Modified;
                    db.SaveChanges();
                    retorno = new { text = "Informações atualizadas com sucesso!", type = "success" };
                }
                else
                {
                    retorno = new { text = "Usuário não encontrado, favor tentar novamente!", type = "error" };
                }
            }
            catch (Exception)
            {
                retorno = new { text = "Aconteceu um erro ao tentar alterar a senha.", type = "error" };
            }
            return new JsonResult { Data = retorno };
        }

        [Autorize(Roles = "Seguranca.Pessoa.Editar")]
        public ActionResult MudarSenha(long id = 0)
        {

            SEUsuario SEUsuario = db.SEUsuario.Find(id);
            if (SEUsuario == null)
            {
                TempData["e"] = "Usuário não encontrado, favor tentar novamente!";
                return RedirectToAction("Indice");
            }

            return View(new SEUsuarioAlterarSenhaModelo
            {
                Login = SEUsuario.Login,
                MudaSenhaProximoLogon = SEUsuario.MudaSenhaProximoLogon,
                Id = SEUsuario.Id,
            });
        }
        
        [Autorize(Roles = "Seguranca.Pessoa.Excluir")]
        public ActionResult Excluir(long id = 0)
        {
            SEUsuario SEUsuario = db.SEUsuario.Find(id);
            return View(SEUsuario);
        }
        
        [HttpPost, ActionName("Excluir")]
        [Autorize(Roles = "Seguranca.Pessoa.Excluir")]
        public ActionResult ConfirmarExclusao(long id)
        {
            SEUsuario SEUsuario = db.SEUsuario.Find(id);
            db.SEUsuario.Remove(SEUsuario);
            db.SaveChanges();
            TempData["s"] = "Usuário excluído com sucesso!";
            return RedirectToAction("Indice");
        }

        private void CriarViewBagSexo(String valor = "")
        {
            var list = new List<dynamic>();
            list.Add(new { Id = "", Text = "" });
            list.Add(new { Id = "M", Text = "Masculino" });
            list.Add(new { Id = "F", Text = "Feminino" });
            ViewBag.Sexo = new SelectList(list, "Id", "Text", valor);
        }

        private void CriarViewBagSituacao(int? valor = 0)
        {
            var list = new List<dynamic>();
            list.Add(new { Id = 1, Text = "Ativo" });
            list.Add(new { Id = 2, Text = "Inativo" });
            ViewBag.Status = new SelectList(list, "Id", "Text", valor);
        }
    }
}