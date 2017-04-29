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
    public class BeneficioController : Controller
    {
        [Autorize(Roles = "Seguranca.Beneficio.Indice")]
        public ActionResult Indice()
        {
            try
            {
                if (Request.IsAjaxRequest())
                {
                    var parametros = JDataTable.GetDataTableParams(Request);

                    var dados = BeneficioRepository.List(x => x.Id, parametros.SearchText, parametros.Order, parametros.Start, parametros.PageSize, x => x.PessoaJuridica );
                    var total = BeneficioRepository.Count(x => x.Id, parametros.SearchText);

                    return Json(new
                    {
                        draw = parametros.Draw,
                        recordsTotal = total,
                        recordsFiltered = total,
                        data = dados.Select(x => new {Id = x.Id, IdAgenteParceiro = x.IdAgenteParceiro, Descricao = x.Descricao, Valor = x.Valor, PessoaJuridica = x.PessoaJuridica })
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

        [Autorize(Roles = "Seguranca.Beneficio.Detalhes")]
        public ActionResult Detalhes(int id)
        {
            try
            {
                var obj = BeneficioRepository.FirstOrDefault(id, x => x.PessoaJuridica );
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["Exception"] = ex;
                return RedirectToAction("Indice");
            }
        }

        [Autorize(Roles = "Seguranca.Beneficio.Criar")]
        public ActionResult Criar()
        {
            CarregarViewBags();
            return View(new Beneficio());
        }

        [HttpPost]
        [Autorize(Roles = "Seguranca.Beneficio.Criar")]
        public ActionResult Criar(Beneficio obj)
        {
            try
            {
                CarregarViewBags();
                if (ModelState.IsValid)
                {
                    BeneficioRepository.Save(obj);
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

        [Autorize(Roles = "Seguranca.Beneficio.Editar")]
        public ActionResult Editar(int id)
        {
            try
            {
                CarregarViewBags();
                var obj = BeneficioRepository.FirstOrDefault(id);
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["Exception"] = ex;
                return RedirectToAction("Indice");
            }
        }

        [HttpPost]
        [Autorize(Roles = "Seguranca.Beneficio.Editar")]
        public ActionResult Editar(Beneficio obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BeneficioRepository.Save(obj);
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

        [Autorize(Roles = "Seguranca.Beneficio.Excluir")]
        public ActionResult Excluir(int id = 0)
        {
            try
            {
                var obj = BeneficioRepository.FirstOrDefault(id, x => x.PessoaJuridica );
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["Exception"] = ex;
                return RedirectToAction("Indice");
            }
        }

        [HttpPost, ActionName("Excluir")]
        [Autorize(Roles = "Seguranca.Beneficio.Excluir")]
        public ActionResult ConfirmarExclusao(int id)
        {
            try
            {
                BeneficioRepository.Delete(id);
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
            ViewBag.IdAgenteParceiro = PessoaJuridicaRepository.List("CNPJ ASC", 0, 150).Select(x => new { x.Id, x.CNPJ });
        }
    }
}


