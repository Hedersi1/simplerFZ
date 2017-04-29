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

namespace Simple.MVC.WEB.Controllers
{
    public class PatrocinadorController : Controller
    {
        [Autorize(Roles = "Seguranca.Patrocinador.Indice")]
        public ActionResult Indice()
        {
            try
            {
                if (Request.IsAjaxRequest())
                {
                    var parametros = JDataTable.GetDataTableParams(Request);

                    var dados = PatrocinadorRepository.List(x => x.Nome, parametros.SearchText, parametros.Order, parametros.Start, parametros.PageSize);
                    var total = PatrocinadorRepository.Count(x => x.Nome, parametros.SearchText);

                    return Json(new
                    {
                        draw = parametros.Draw,
                        recordsTotal = total,
                        recordsFiltered = total,
                        data = dados.Select(x => new {Id = x.Id, Nome = x.Nome, Logomarca = x.Logomarca })
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

        [Autorize(Roles = "Seguranca.Patrocinador.Detalhes")]
        public ActionResult Detalhes(int id)
        {
            try
            {
                var obj = PatrocinadorRepository.FirstOrDefault(id);
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["Exception"] = ex;
                return RedirectToAction("Indice");
            }
        }

        [Autorize(Roles = "Seguranca.Patrocinador.Criar")]
        public ActionResult Criar()
        {
            CarregarViewBags();
            return View(new Patrocinador());
        }

        [HttpPost]
        [Autorize(Roles = "Seguranca.Patrocinador.Criar")]
        public ActionResult Criar(Patrocinador obj)
        {
            try
            {
                CarregarViewBags();
                if (ModelState.IsValid)
                {
                    var arquivo = Request.Files[0];
                    arquivo.SaveAs(Server.MapPath("~/Upload/Files/" + arquivo.FileName));

                    obj.Logomarca = arquivo.FileName;

                    PatrocinadorRepository.Save(obj);
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

        [Autorize(Roles = "Seguranca.Patrocinador.Editar")]
        public ActionResult Editar(int id)
        {
            try
            {
                CarregarViewBags();
                var obj = PatrocinadorRepository.FirstOrDefault(id);
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["Exception"] = ex;
                return RedirectToAction("Indice");
            }
        }

        [HttpPost]
        [Autorize(Roles = "Seguranca.Patrocinador.Editar")]
        public ActionResult Editar(Patrocinador obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    PatrocinadorRepository.Save(obj);
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

        [Autorize(Roles = "Seguranca.Patrocinador.Excluir")]
        public ActionResult Excluir(int id = 0)
        {
            try
            {
                var obj = PatrocinadorRepository.FirstOrDefault(id);
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["Exception"] = ex;
                return RedirectToAction("Indice");
            }
        }

        [HttpPost, ActionName("Excluir")]
        [Autorize(Roles = "Seguranca.Patrocinador.Excluir")]
        public ActionResult ConfirmarExclusao(int id)
        {
            try
            {
                PatrocinadorRepository.Delete(id);
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
        }
    }
}


