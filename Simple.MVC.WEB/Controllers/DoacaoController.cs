using Simple.MVC.Business.FZ;
using Simple.MVC.Business.Seguranca;
using Simple.MVC.Business.Util;
using Simple.MVC.Security;
using Simple.MVC.Security.Data;
using Simple.MVC.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Simple.MVC.WEB.Controllers
{
    public class DoacaoController : Controller
    {
        [Autorize(Roles = "Seguranca.Doacao.Indice")]
        public ActionResult Indice()
        {
            try
            {
                if (Request.IsAjaxRequest())
                {
                    var parametros = JDataTable.GetDataTableParams(Request);

                    var dados = DoacaoRepository.List(x => x.Id, parametros.SearchText, parametros.Order, parametros.Start, parametros.PageSize, x => x.Pessoa , x => x.PessoaFisica );
                    var total = DoacaoRepository.Count(x => x.Id, parametros.SearchText);

                    return Json(new
                    {
                        draw = parametros.Draw,
                        recordsTotal = total,
                        recordsFiltered = total,
                        data = dados.Select(x => new {Id = x.Id, Data = x.Data.ToString("dd/MM/yyyy HH:mm"), InicioDisponibilidadeEntrega = x.InicioDisponibilidadeEntrega.ToString("dd/MM/yyyy HH:mm:ss"), TerminoDisponibilidadeEntrega = x.TerminoDisponibilidadeEntrega.ToString("dd/MM/yyyy HH:mm:ss") })
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

        [Autorize(Roles = "Seguranca.Doacao.Detalhes")]
        public ActionResult Detalhes(int id)
        {
            try
            {
                var obj = DoacaoRepository.FirstOrDefault(id, x => x.Pessoa , x => x.PessoaFisica );
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["Exception"] = ex;
                return RedirectToAction("Indice");
            }
        }

        [Autorize(Roles = "Seguranca.Doacao.Criar")]
        public ActionResult Criar()
        {
            CarregarViewBags();
            return View(new Doacao { InicioDisponibilidadeEntrega = DateTime.Now, TerminoDisponibilidadeEntrega = DateTime.Now.AddDays(2) });
        }

        [HttpPost]
        [Autorize(Roles = "Seguranca.Doacao.Criar")]
        public ActionResult Criar(Doacao obj)
        {
            try
            {
                CarregarViewBags();
                if (ModelState.IsValid)
                {
                    var pessoa = PessoaRepository.List(x => x.IdUsuario, Util.GetUsuarioLogado().Id, "Id ASC").FirstOrDefault();
                    obj.IdAgenteDoador = pessoa.Id;

                    DoacaoRepository.Save(obj);
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

        [Autorize(Roles = "Seguranca.Doacao.Editar")]
        public ActionResult Editar(int id)
        {
            try
            {
                CarregarViewBags();
                var obj = DoacaoRepository.FirstOrDefault(id);
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["Exception"] = ex;
                return RedirectToAction("Indice");
            }
        }

        [HttpPost]
        [Autorize(Roles = "Seguranca.Doacao.Editar")]
        public ActionResult Editar(Doacao obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DoacaoRepository.Save(obj);
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

        [Autorize(Roles = "Seguranca.Doacao.Excluir")]
        public ActionResult Excluir(int id = 0)
        {
            try
            {
                var obj = DoacaoRepository.FirstOrDefault(id, x => x.Pessoa , x => x.PessoaFisica );
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["Exception"] = ex;
                return RedirectToAction("Indice");
            }
        }

        [HttpPost, ActionName("Excluir")]
        [Autorize(Roles = "Seguranca.Doacao.Excluir")]
        public ActionResult ConfirmarExclusao(int id)
        {
            try
            {
                DoacaoRepository.Delete(id);
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
            ViewBag.IdAgenteDoador = PessoaRepository.List("Nome ASC", 0, 150).Select(x => new { x.Id, x.Nome });
            ViewBag.IdAgenteFZ = PessoaFisicaRepository.List("CPF ASC", 0, 150).Select(x => new { x.Id, x.CPF });
        }

        public ActionResult Mapa()
        {
            return View();
        }

        public ActionResult GetDoacoes()
        {
            if(Request.IsAjaxRequest())
            {
                return Json(DoacaoRepository.List().Select(x => new { lat = x.Latitude, lng = x.Longitude }), JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Mapa");
        }
    }
}


