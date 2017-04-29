using Simple.MVC.Business.FZ;
using Simple.MVC.Business.Seguranca;
using Simple.MVC.Business.Util;
using Simple.MVC.Security.Data;
using Simple.MVC.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Simple.MVC.WEB.Seguranca.Controllers
{
    public class AgenteDoadorController : Controller
    {
        [Autorize(Roles = "Seguranca.AgenteDoador.Indice")]
        public ActionResult Indice()
        {
            try
            {
                if (Request.IsAjaxRequest())
                {
                    var parametros = JDataTable.GetDataTableParams(Request);

                    var dados = PessoaRepository.ListAgenteDoador(parametros.SearchText, parametros.Order, parametros.Start, parametros.PageSize);
                    var total = PessoaRepository.CountAgenteDoador(parametros.SearchText);

                    return Json(new
                    {
                        draw = parametros.Draw,
                        recordsTotal = total,
                        recordsFiltered = total,
                        data = dados.Select(x => new {Id = x.Id, Nome = x.Nome, Email = x.Email, PessoaStatus = x.PessoaStatus, SEUsuario = x.Usuario, Cidade = x.Cidade })
                    },
                    JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                TempData["Exception"] = ex;
            }
            return View();
        }

        [Autorize(Roles = "Seguranca.AgenteDoador.Detalhes")]
        public ActionResult Detalhes(int id)
        {
            try
            {
                var obj = PessoaRepository.FirstOrDefault(id, x => x.PessoaStatus , x => x.Usuario , x => x.Cidade );
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["Exception"] = ex;
                return RedirectToAction("Indice");
            }
        }

        [Autorize(Roles = "Seguranca.AgenteDoador.Criar")]
        public ActionResult Criar()
        {
            CarregarViewBags();
            return View(new Pessoa());
        }

        [HttpPost]
        [Autorize(Roles = "Seguranca.AgenteDoador.Criar")]
        public ActionResult Criar(Pessoa obj)
        {
            try
            {
                CarregarViewBags();
                if (ModelState.IsValid)
                {
                    PessoaRepository.Save(obj);
                    TempData["s"] = "Item Inserido com sucesso!";

                    return RedirectToAction("Criar");
                }
            }
            catch (Exception ex)
            {
                TempData["e"] = "Erro ao inserir o objeto!";
                TempData["Exception"] = ex;
            }

            return View(obj);
        }

        [Autorize(Roles = "Seguranca.AgenteDoador.Editar")]
        public ActionResult Editar(int id)
        {
            try
            {
                CarregarViewBags();
                var obj = PessoaRepository.FirstOrDefault(id);
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["Exception"] = ex;
                return RedirectToAction("Indice");
            }
        }

        [HttpPost]
        [Autorize(Roles = "Seguranca.AgenteDoador.Editar")]
        public ActionResult Editar(Pessoa obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    PessoaRepository.Save(obj);
                    TempData["s"] = "Alteração Realizada com sucesso!";
                    return RedirectToAction("Indice");
                }
            }
            catch (Exception ex)
            {
                TempData["e"] = "Erro ao alterar o objeto!";
                TempData["Exception"] = ex;
            }
            CarregarViewBags();
            return View(obj);
        }

        [Autorize(Roles = "Seguranca.AgenteDoador.Excluir")]
        public ActionResult Excluir(int id = 0)
        {
            try
            {
                var obj = PessoaRepository.FirstOrDefault(id, x => x.PessoaStatus , x => x.Usuario , x => x.Cidade );
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["Exception"] = ex;
                return RedirectToAction("Indice");
            }
        }

        [HttpPost, ActionName("Excluir")]
        [Autorize(Roles = "Seguranca.AgenteDoador.Excluir")]
        public ActionResult ConfirmarExclusao(int id)
        {
            try
            {
                PessoaRepository.Delete(id);
                TempData["s"] = "Exclusão Realizada com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["e"] = "Erro ao excluir o objeto!";
                TempData["Exception"] = ex;
            }
            return RedirectToAction("Indice");
        }
        public void CarregarViewBags()
        {
            ViewBag.PessoaStatus = PessoaStatusRepository.List("Descricao ASC", 0, 150).Select(x => new { x.Id, x.Descricao });
            ViewBag.Cidade = CidadeRepository.List("Nome ASC", 0, 150).Select(x => new { x.Id, x.Nome });
        }
    }
}


