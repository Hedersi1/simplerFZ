using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Simple.MVC.Security.Data;
using System.Data.Entity.Infrastructure;
using System;
using Simple.MVC.Business.Seguranca;
using System.IO;
using Simple.MVC.WEB.Models;

namespace Simple.MVC.WEB.Seguranca.Controllers
{
    public class GeradorController : Controller
    {
        String diretorio = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDi‌​rectory, "..\\"));

        public ActionResult Indice()
        {
            try
            {
                if (Request.IsAjaxRequest())
                {
                    var parametros = JDataTable.GetDataTableParams(Request);

                    var dados = GeradorRepository.List(x => x.Tabela, parametros.SearchText, parametros.Order, parametros.Start, parametros.PageSize);
                    var total = GeradorRepository.Count(x => x.Tabela, parametros.SearchText);

                    return Json(new
                    {
                        draw = parametros.Draw,
                        recordsTotal = total,
                        recordsFiltered = total,
                        data = dados.Select(x => new { Id = x.Id, Tabela = x.Tabela, Classe = x.Classe, Area = x.Area })
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

        public ActionResult Detalhes(int id)
        {
            try
            {
                var obj = GeradorRepository.FirstOrDefault(id);
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["Exception"] = ex;
                return RedirectToAction("Indice");
            }
        }


        [HttpGet]
        public ActionResult Criar()
        {
            using (var ctx = new HackathonEntities())
            {
                List<string> results = (ctx as IObjectContextAdapter).ObjectContext.ExecuteStoreQuery<string>("SELECT name FROM sys.tables where name not in (select a.Tabela from SEGerador a) ORDER BY name ").ToList();
                ViewBag.Tabelas = new SelectList(results.Select(s => new SelectListItem { Text = s, Value = s }), "Value", "Text");

                ViewBag.Areas = new SelectList(ctx.SESistema.Select(s => new SelectListItem { Text = s.Caminho.Replace("/", ""), Value = s.Caminho.Replace("/", "") }).ToList(), "Value", "Text");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Criar(string Tabela, string Area)
        {
            Gerador gerador = new Gerador();

            gerador.Tabela = Tabela;
            gerador.Area = Area;
            gerador.Apresentacao = Tabela.Remove(0, 2);
            gerador.Classe = Tabela.Remove(0, 2);
            gerador.MensagemSucessoInsercao = "Item Inserido com sucesso!";
            gerador.MensagemSucessoAlteracao = "Alteração Realizada com sucesso!";
            gerador.MensagemSucessoExclusao = "Exclusão Realizada com sucesso!";

            gerador.MensagemErroInsercao = "Erro ao inserir o objeto!";
            gerador.MensagemErroAlteracao = "Erro ao alterar o objeto!";
            gerador.MensagemErroExclusao = "Erro ao excluir o objeto!";

            var dados = DBEstruturaRepository.List(Tabela);
            gerador.GeradorItem = new List<GeradorItem>();

            foreach (var item in dados)
            {
                gerador.GeradorItem.Add(new GeradorItem
                {
                    FieldName = item.FieldName,
                    DataType = item.DataType,
                    IsIdentity = item.IsIdentity,
                    IsNullable = item.IsNullable,
                    IsPrimaryKey = item.IsPrimaryKey,
                    MaxLength = item.MaxLength,
                    NomeApresentacao = item.FieldName,
                    TipoCSharp = Business.Util.Extensao.ToDotNetType(item.DataType, item.IsNullable),
                    ListarTabela = true,
                    FkTable = item.PkTableName,
                    FkField = item.PkColumnName
                });
            }



            GeradorRepository.Save(gerador);

            return RedirectToAction("Editar", new { Tabela = gerador.Tabela });
        }
        [HttpGet]
        public ActionResult Editar(string Tabela)
        {
            ViewBag.Validacao = new SelectList(GeradorItemValidacaoRepository.List("Texto ASC", 0, 100), "Id", "Texto");
            ViewBag.Tipo = new SelectList(GeradorItemTipoRepository.List("Tipo ASC", 0, 100), "Id", "Tipo");
            using (var ctx = new HackathonEntities())
            {
                ViewBag.Areas = new SelectList(ctx.SESistema.Select(s => new SelectListItem { Text = s.Caminho.Replace("/", ""), Value = s.Caminho.Replace("/", "") }).ToList(), "Value", "Text");
            }
            return View(GeradorRepository.BuscarPorTabela(Tabela));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Editar(Gerador gerador)
        {
            try
            {
                ViewBag.Validacao = new SelectList(GeradorItemValidacaoRepository.List("Texto ASC", 0, 100), "Id", "Texto");
                ViewBag.Tipo = new SelectList(GeradorItemTipoRepository.List("Tipo ASC", 0, 100), "Id", "Tipo");
                using (var ctx = new HackathonEntities())
                {
                    ViewBag.Areas = new SelectList(ctx.SESistema.Select(s => new SelectListItem { Text = s.Caminho.Replace("/", ""), Value = s.Caminho.Replace("/", "") }).ToList(), "Value", "Text");
                }

                this.salvarGerador(gerador);
                TempData["s"] = "Ação realizada com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["e"] = "Erro ao realiar a ação!";
            }

            return View(gerador);
        }

        private void salvarGerador(Gerador gerador)
        {
            GeradorRepository.Save(gerador);
            foreach (var item in gerador.GeradorItem)
            {
                GeradorItemRepository.Save(item);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GerarClasses(Gerador gerador)
        {
            try
            {
                this.salvarGerador(gerador);

                var classes = GeradorRepository.GerarClasses(gerador);
                var diretorioclasse = diretorio + "Simple.MVC.Business\\" + gerador.Area + "\\";

                System.IO.StreamWriter file = new System.IO.StreamWriter(diretorioclasse + gerador.Classe + ".cs");
                file.WriteLine(classes[0].ToString());
                file.Close();

                file = new System.IO.StreamWriter(diretorioclasse + gerador.Classe + "Repository.cs");
                file.WriteLine(classes[1].ToString());
                file.Close();

                TempData["s"] = "Classes geradas com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["e"] = "Erro ao realiar a ação!";
            }

            return RedirectToAction("Editar", new { Tabela = gerador.Tabela });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GerarPaginas(Gerador gerador)
        {
            try
            {
                this.salvarGerador(gerador);

                var paginas = GeradorRepository.GerarPaginas(gerador);
                var diretoriopagina = diretorio + "Simple.MVC.WEB\\Areas\\" + gerador.Area + "\\";
                System.IO.StreamWriter file = new System.IO.StreamWriter(diretoriopagina +"Controllers\\"+ gerador.Classe + "Controller.cs");
                file.WriteLine(paginas[0].ToString());
                file.Close();

                if (!Directory.Exists(diretoriopagina + "Views\\" + gerador.Classe + "\\"))
                {
                    Directory.CreateDirectory(diretoriopagina + "Views\\" + gerador.Classe + "\\");
                }

                file = new System.IO.StreamWriter(diretoriopagina + "Views\\" + gerador.Classe + "\\Indice.cshtml", false, System.Text.Encoding.UTF8);
                file.WriteLine(paginas[1].ToString());
                file.Close();

                file = new System.IO.StreamWriter(diretoriopagina + "Views\\" + gerador.Classe + "\\Detalhes.cshtml", false, System.Text.Encoding.UTF8);
                file.WriteLine(paginas[2].ToString());
                file.Close();

                file = new System.IO.StreamWriter(diretoriopagina + "Views\\" + gerador.Classe + "\\Criar.cshtml", false, System.Text.Encoding.UTF8);
                file.WriteLine(paginas[3].ToString());
                file.Close();

                file = new System.IO.StreamWriter(diretoriopagina + "Views\\" + gerador.Classe + "\\Editar.cshtml", false, System.Text.Encoding.UTF8);
                file.WriteLine(paginas[4].ToString());
                file.Close();

                file = new System.IO.StreamWriter(diretoriopagina + "Views\\" + gerador.Classe + "\\Excluir.cshtml", false, System.Text.Encoding.UTF8);
                file.WriteLine(paginas[5].ToString());
                file.Close();

                TempData["s"] = "Páginas geradas com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["e"] = "Erro ao realiar a ação!";
            }

            return RedirectToAction("Editar", new { Tabela = gerador.Tabela });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CadasatrarInformacoes(Gerador gerador)
        {
            try
            {
                Sistema sistema = SistemaRepository.FirstOrDefault(gerador.Area);
                Grupo grupo = GrupoRepository.FirstOrDefault(gerador.Area);

                PapelRepository.Save(new Papel { IdSistema = sistema.Id, Nome = gerador.Area + "." + gerador.Classe + ".Indice", Descricao = "Indice - " + gerador.Classe }, grupo);
                PapelRepository.Save(new Papel { IdSistema = sistema.Id, Nome = gerador.Area + "." + gerador.Classe + ".Detalhes", Descricao = "Detalhes - " + gerador.Classe }, grupo);
                PapelRepository.Save(new Papel { IdSistema = sistema.Id, Nome = gerador.Area + "." + gerador.Classe + ".Editar", Descricao = "Alterar - " + gerador.Classe }, grupo);
                PapelRepository.Save(new Papel { IdSistema = sistema.Id, Nome = gerador.Area + "." + gerador.Classe + ".Excluir", Descricao = "Excluir - " + gerador.Classe }, grupo);
                PapelRepository.Save(new Papel { IdSistema = sistema.Id, Nome = gerador.Area + "." + gerador.Classe + ".Criar", Descricao = "Criar - " + gerador.Classe }, grupo);

                MenuRepository.Save(sistema, gerador);
                
                TempData["s"] = "Páginas geradas com sucesso!";
            }
            catch (Exception)
            {
                TempData["e"] = "Erro ao realizar a ação!";
            }

            return RedirectToAction("Editar", new { Tabela = gerador.Tabela });
        }

        [HttpGet]
        public JsonResult Atributtos(string tabela)
        {
            try
            {
                var dados = DBEstruturaRepository.List(tabela);

                return Json(new
                {
                    type = "success",
                    text = "Itens carregados com sucesso",
                    recordsTotal = dados.Count,
                    recordsFiltered = dados.Count,
                    data = dados.Select(x => new { Id = x.Id, FieldName = x.FieldName, DataType = x.DataType, MaxLength = x.MaxLength, IsNullable = x.IsNullable, IsIdentity = x.IsIdentity, IsPrimaryKey= x.IsPrimaryKey })
                },
                JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    type = "error",
                    text = "Erro ao carregar os dados da tabela"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Excluir(int id = 0)
        {
            try
            {
                var obj = GeradorRepository.FirstOrDefault(id);
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["Exception"] = ex;
                return RedirectToAction("Indice");
            }
        }

        [HttpPost, ActionName("Excluir")]
        public ActionResult ConfirmarExclusao(int id)
        {
            try
            {
                GeradorRepository.Delete(id);
                TempData["s"] = "Exclusão Realizada com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["s"] = "Erro ao excluir o objeto!";
                TempData["Exception"] = ex;
            }
            return RedirectToAction("Indice");
        }

    }
}