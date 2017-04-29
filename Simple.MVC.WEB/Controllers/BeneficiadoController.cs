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
    public class BeneficiadoController : Controller
    {
        [Autorize(Roles = "Seguranca.Beneficiado.Indice")]
        public ActionResult Indice()
        {
            try
            {
                if (Request.IsAjaxRequest())
                {
                    var parametros = JDataTable.GetDataTableParams(Request);

                    var dados = BeneficiadoRepository.List(x => x.Id, parametros.SearchText, parametros.Order, parametros.Start, parametros.PageSize, x => x.SituacaoBeneficiado );
                    var total = BeneficiadoRepository.Count(x => x.Id, parametros.SearchText);

                    return Json(new
                    {
                        draw = parametros.Draw,
                        recordsTotal = total,
                        recordsFiltered = total,
                        data = dados.Select(x => new {Id = x.Id, NomeResponsavel = x.NomeResponsavel, CPF = x.CPF, IdCidade = x.IdCidade, IdSituacaoBeneficiado = x.IdSituacaoBeneficiado, SituacaoBeneficiado = x.SituacaoBeneficiado })
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

        [Autorize(Roles = "Seguranca.Beneficiado.Detalhes")]
        public ActionResult Detalhes(int id)
        {
            try
            {
                var obj = BeneficiadoRepository.FirstOrDefault(id, x => x.SituacaoBeneficiado );
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["Exception"] = ex;
                return RedirectToAction("Indice");
            }
        }

        [Autorize(Roles = "Seguranca.Beneficiado.Criar")]
        public ActionResult Criar()
        {
            CarregarViewBags();
            return View(new Beneficiado());
        }

        [HttpPost]
        [Autorize(Roles = "Seguranca.Beneficiado.Criar")]
        public ActionResult Criar(Beneficiado obj)
        {
            try
            {
                CarregarViewBags();
                if (ModelState.IsValid)
                {
                    BeneficiadoRepository.Save(obj);
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

        [Autorize(Roles = "Seguranca.Beneficiado.Editar")]
        public ActionResult Editar(int id)
        {
            try
            {
                CarregarViewBags();
                var obj = BeneficiadoRepository.FirstOrDefault(id);
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["Exception"] = ex;
                return RedirectToAction("Indice");
            }
        }

        [HttpPost]
        [Autorize(Roles = "Seguranca.Beneficiado.Editar")]
        public ActionResult Editar(Beneficiado obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BeneficiadoRepository.Save(obj);
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

        [Autorize(Roles = "Seguranca.Beneficiado.Excluir")]
        public ActionResult Excluir(int id = 0)
        {
            try
            {
                var obj = BeneficiadoRepository.FirstOrDefault(id, x => x.SituacaoBeneficiado );
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["Exception"] = ex;
                return RedirectToAction("Indice");
            }
        }

        [HttpPost, ActionName("Excluir")]
        [Autorize(Roles = "Seguranca.Beneficiado.Excluir")]
        public ActionResult ConfirmarExclusao(int id)
        {
            try
            {
                BeneficiadoRepository.Delete(id);
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
            ViewBag.IdSituacaoBeneficiado = SituacaoBeneficiadoRepository.List("Descricao ASC", 0, 150).Select(x => new { x.Id, x.Descricao });
        }
    }
}


