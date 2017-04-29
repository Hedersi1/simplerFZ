using Simple.MVC.Core.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Simple.MVC.Business.Seguranca
{
    public class GeradorRepository : Repository<Gerador, ContextBusiness>
    {
        public static Gerador BuscarPorTabela(String tabela)
        {
            return FirstOrDefault(x => x.Tabela == tabela, x => x.GeradorItem);
        }

        public static List<String> GerarPaginas(Gerador gerador)
        {
            List<String> retorno = new List<String>();
            retorno.Add(GeradorRepository.GerarControlador(gerador).ToString());
            retorno.Add(GeradorRepository.GerarIndice(gerador).ToString());
            retorno.Add(GeradorRepository.GerarDetalhes(gerador).ToString());
            retorno.Add(GeradorRepository.GerarCriar(gerador).ToString());
            retorno.Add(GeradorRepository.GerarEditar(gerador).ToString());
            retorno.Add(GeradorRepository.GerarExcluir(gerador).ToString());
            return retorno;
        }

        public static List<String> GerarClasses(Gerador gerador)
        {
            StringBuilder classe = new StringBuilder();
            classe.AppendLine("using Simple.MVC.Core.Entity;");
            classe.AppendLine("using System;");
            classe.AppendLine("using System.Collections.Generic;");
            classe.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
            classe.AppendLine("using System.ComponentModel.DataAnnotations;");
            classe.AppendLine("");
            classe.AppendLine("namespace Simple.MVC.Business." + gerador.Area);
            classe.AppendLine("{");
            classe.AppendLine("    [Table(\"" + gerador.Tabela + "\"), Serializable]");
            classe.AppendLine("    public class " + gerador.Classe + " : Entity");
            classe.AppendLine("    {");

            foreach (var item in gerador.GeradorItem)
            {
                if (item.FieldName != "Id")
                {
                    classe.AppendLine("");
                    if (item.IdGeradorItemTipo != null)
                    {
                        var tipo = GeradorItemTipoRepository.FirstOrDefault(item.IdGeradorItemTipo.GetValueOrDefault());
                        if (tipo.Validacao != "")
                        {
                            classe.AppendLine("        " + tipo.Validacao);
                        }
                    }

                    if (item.IdValidacao1 != null)
                    {
                        var validacao = GeradorItemValidacaoRepository.FirstOrDefault(item.IdValidacao1.GetValueOrDefault());
                        classe.AppendLine("        " + validacao.Validacao);
                    }

                    if (item.IdValidacao2 != null)
                    {
                        var validacao = GeradorItemValidacaoRepository.FirstOrDefault(item.IdValidacao2.GetValueOrDefault());
                        classe.AppendLine("        " + validacao.Validacao);
                    }

                    classe.AppendLine("        [Display(Name=\"" + item.NomeApresentacao + "\")]");
                    classe.AppendLine("        public " + item.TipoCSharp + " " + item.FieldName + " { get; set; }");
                }
            }

            foreach (var item in gerador.GeradorItem.Where(x => x.FkTable != null))
            {
                classe.AppendLine("        [ForeignKey(\""+item.FieldName+"\")]");
                classe.AppendLine("        public "+item.FkTable.Remove(0, 2)+ " " + item.FkTable.Remove(0, 2) + " { get; set; }");
            }
            classe.AppendLine("    }");
            classe.AppendLine("}");

            StringBuilder repositorio = new StringBuilder();
            repositorio.AppendLine("using Simple.MVC.Core.Repository;");
            repositorio.AppendLine("using System;");
            repositorio.AppendLine("using System.Collections.Generic;");
            repositorio.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
            repositorio.AppendLine("");
            repositorio.AppendLine("namespace Simple.MVC.Business." + gerador.Area);
            repositorio.AppendLine("{");
            repositorio.AppendLine("    public class " + gerador.Classe + "Repository : Repository<" + gerador.Classe + ", ContextBusiness>");
            repositorio.AppendLine("    {");
            repositorio.AppendLine("    }");
            repositorio.AppendLine("}");

            List<String> retorno = new List<String>();
            retorno.Add(classe.ToString());
            retorno.Add(repositorio.ToString());

            return retorno;
        }


        private static  StringBuilder GerarControlador(Gerador gerador)
        {
            String includes = "";
            String selectfk = "";
            String select = "";

            var tabela = gerador.GeradorItem.Where(s => s.ListarTabela == true).ToList();
            var fk = gerador.GeradorItem.Where(s => s.FkTable != null).ToList();

            foreach (var item in tabela)
            {
                select += ", " + item.FieldName + " = x." + item.FieldName + "" + (item.TipoCSharp.Contains("Nullable") ? ".ValueOrDefault()" : "");
            }

            foreach (var item in fk)
            {
                selectfk += ", " + item.FkTable.Remove(0, 2) + " = x." + item.FkTable.Remove(0, 2) ;
                includes += ", x => x." + item.FkTable.Remove(0, 2) + " ";
            }

            StringBuilder controlador = new StringBuilder();
            controlador.AppendLine("using Simple.MVC.Business."+gerador.Area+";");
            controlador.AppendLine("using Simple.MVC.Business.Util;");
            controlador.AppendLine("using Simple.MVC.Security.Data;");
            controlador.AppendLine("using Simple.MVC.WEB.Models;");
            controlador.AppendLine("using System;");
            controlador.AppendLine("using System.Collections.Generic;");
            controlador.AppendLine("using System.Linq;");
            controlador.AppendLine("using System.Web;");
            controlador.AppendLine("using System.Web.Mvc;");
            controlador.AppendLine("");
            controlador.AppendLine("namespace Simple.MVC.WEB." + gerador.Area + ".Controllers");
            controlador.AppendLine("{");
            controlador.AppendLine("    public class " + gerador.Classe + "Controller : Controller");
            controlador.AppendLine("    {");
            controlador.AppendLine("        [Autorize(Roles = \"" + gerador.Area + "." + gerador.Classe + ".Indice\")]");
            controlador.AppendLine("        public ActionResult Indice()");
            controlador.AppendLine("        {");
            controlador.AppendLine("            try");
            controlador.AppendLine("            {");
            controlador.AppendLine("                if (Request.IsAjaxRequest())");
            controlador.AppendLine("                {");
            controlador.AppendLine("                    var parametros = JDataTable.GetDataTableParams(Request);");
            controlador.AppendLine("");
            controlador.AppendLine("                    var dados = " + gerador.Classe + "Repository.List(x => x."+ tabela.FirstOrDefault().FieldName + ", parametros.SearchText, parametros.Order, parametros.Start, parametros.PageSize"+ includes + ");");
            controlador.AppendLine("                    var total = " + gerador.Classe + "Repository.Count(x => x." + tabela.FirstOrDefault().FieldName + ", parametros.SearchText);");
            controlador.AppendLine("");
            controlador.AppendLine("                    return Json(new");
            controlador.AppendLine("                    {");
            controlador.AppendLine("                        draw = parametros.Draw,");
            controlador.AppendLine("                        recordsTotal = total,");
            controlador.AppendLine("                        recordsFiltered = total,");
            controlador.AppendLine("                        data = dados.Select(x => new {Id = x.Id"+ select + selectfk + " })");
            controlador.AppendLine("                    },");
            controlador.AppendLine("                    JsonRequestBehavior.AllowGet);");
            controlador.AppendLine("                }");
            controlador.AppendLine("            }");
            controlador.AppendLine("            catch (Exception ex)");
            controlador.AppendLine("            {");
            controlador.AppendLine("                TempData[\"Exception\"] = ex;");
            controlador.AppendLine("            }");
            controlador.AppendLine("            return View();");
            controlador.AppendLine("        }");
            controlador.AppendLine("");
            controlador.AppendLine("        [Autorize(Roles = \"" + gerador.Area + "." + gerador.Classe + ".Detalhes\")]");
            controlador.AppendLine("        public ActionResult Detalhes(int id)");
            controlador.AppendLine("        {");
            controlador.AppendLine("            try");
            controlador.AppendLine("            {");
            controlador.AppendLine("                var obj = " + gerador.Classe + "Repository.FirstOrDefault(id"+ includes + ");");
            controlador.AppendLine("                return View(obj);");
            controlador.AppendLine("            }");
            controlador.AppendLine("            catch (Exception ex)");
            controlador.AppendLine("            {");
            controlador.AppendLine("                TempData[\"Exception\"] = ex;");
            controlador.AppendLine("                return RedirectToAction(\"Indice\");");
            controlador.AppendLine("            }");
            controlador.AppendLine("        }");
            controlador.AppendLine("");
            controlador.AppendLine("        [Autorize(Roles = \"" + gerador.Area + "." + gerador.Classe + ".Criar\")]");
            controlador.AppendLine("        public ActionResult Criar()");
            controlador.AppendLine("        {");
            controlador.AppendLine("            CarregarViewBags();");
            controlador.AppendLine("            return View(new " + gerador.Classe + "());");
            controlador.AppendLine("        }");
            controlador.AppendLine("");
            controlador.AppendLine("        [HttpPost]");
            controlador.AppendLine("        [Autorize(Roles = \"" + gerador.Area + "." + gerador.Classe + ".Criar\")]");
            controlador.AppendLine("        public ActionResult Criar(" + gerador.Classe + " obj)");
            controlador.AppendLine("        {");
            controlador.AppendLine("            try");
            controlador.AppendLine("            {");
            controlador.AppendLine("                CarregarViewBags();");
            controlador.AppendLine("                if (ModelState.IsValid)");
            controlador.AppendLine("                {");
            controlador.AppendLine("                    " + gerador.Classe + "Repository.Save(obj);");
            controlador.AppendLine("                    TempData[\"s\"] = \""+gerador.MensagemSucessoInsercao+"\";");
            controlador.AppendLine("");
            controlador.AppendLine("                    return RedirectToAction(\"Criar\");");
            controlador.AppendLine("                }");
            controlador.AppendLine("            }");
            controlador.AppendLine("            catch (Exception ex)");
            controlador.AppendLine("            {");
            controlador.AppendLine("                TempData[\"e\"] = \"" + gerador.MensagemErroInsercao + "\";");
            controlador.AppendLine("                TempData[\"Exception\"] = ex;");
            controlador.AppendLine("            }");
            controlador.AppendLine("");
            controlador.AppendLine("            return View(obj);");
            controlador.AppendLine("        }");
            controlador.AppendLine("");
            controlador.AppendLine("        [Autorize(Roles = \"" + gerador.Area + "." + gerador.Classe + ".Editar\")]");
            controlador.AppendLine("        public ActionResult Editar(int id)");
            controlador.AppendLine("        {");
            controlador.AppendLine("            try");
            controlador.AppendLine("            {");
            controlador.AppendLine("                CarregarViewBags();");
            controlador.AppendLine("                var obj = " + gerador.Classe + "Repository.FirstOrDefault(id);");
            controlador.AppendLine("                return View(obj);");
            controlador.AppendLine("            }");
            controlador.AppendLine("            catch (Exception ex)");
            controlador.AppendLine("            {");
            controlador.AppendLine("                TempData[\"Exception\"] = ex;");
            controlador.AppendLine("                return RedirectToAction(\"Indice\");");
            controlador.AppendLine("            }");
            controlador.AppendLine("        }");
            controlador.AppendLine("");
            controlador.AppendLine("        [HttpPost]");
            controlador.AppendLine("        [Autorize(Roles = \"" + gerador.Area + "." + gerador.Classe + ".Editar\")]");
            controlador.AppendLine("        public ActionResult Editar(" + gerador.Classe + " obj)");
            controlador.AppendLine("        {");
            controlador.AppendLine("            try");
            controlador.AppendLine("            {");
            controlador.AppendLine("                if (ModelState.IsValid)");
            controlador.AppendLine("                {");
            controlador.AppendLine("                    " + gerador.Classe + "Repository.Save(obj);");
            controlador.AppendLine("                    TempData[\"s\"] = \"" + gerador.MensagemSucessoAlteracao + "\";");
            controlador.AppendLine("                    return RedirectToAction(\"Indice\");");
            controlador.AppendLine("                }");
            controlador.AppendLine("            }");
            controlador.AppendLine("            catch (Exception ex)");
            controlador.AppendLine("            {");
            controlador.AppendLine("                TempData[\"e\"] = \"" + gerador.MensagemErroAlteracao + "\";");
            controlador.AppendLine("                TempData[\"Exception\"] = ex;");
            controlador.AppendLine("            }");
            controlador.AppendLine("            CarregarViewBags();");
            controlador.AppendLine("            return View(obj);");
            controlador.AppendLine("        }");
            controlador.AppendLine("");
            controlador.AppendLine("        [Autorize(Roles = \"" + gerador.Area + "." + gerador.Classe + ".Excluir\")]");
            controlador.AppendLine("        public ActionResult Excluir(int id = 0)");
            controlador.AppendLine("        {");
            controlador.AppendLine("            try");
            controlador.AppendLine("            {");
            controlador.AppendLine("                var obj = " + gerador.Classe + "Repository.FirstOrDefault(id" + includes + ");");
            controlador.AppendLine("                return View(obj);");
            controlador.AppendLine("            }");
            controlador.AppendLine("            catch (Exception ex)");
            controlador.AppendLine("            {");
            controlador.AppendLine("                TempData[\"Exception\"] = ex;");
            controlador.AppendLine("                return RedirectToAction(\"Indice\");");
            controlador.AppendLine("            }");
            controlador.AppendLine("        }");
            controlador.AppendLine("");
            controlador.AppendLine("        [HttpPost, ActionName(\"Excluir\")]");
            controlador.AppendLine("        [Autorize(Roles = \"" + gerador.Area + "." + gerador.Classe + ".Excluir\")]");
            controlador.AppendLine("        public ActionResult ConfirmarExclusao(int id)");
            controlador.AppendLine("        {");
            controlador.AppendLine("            try");
            controlador.AppendLine("            {");
            controlador.AppendLine("                " + gerador.Classe + "Repository.Delete(id);");
            controlador.AppendLine("                TempData[\"s\"] = \"" + gerador.MensagemSucessoExclusao + "\";");
            controlador.AppendLine("            }");
            controlador.AppendLine("            catch (Exception ex)");
            controlador.AppendLine("            {");
            controlador.AppendLine("                TempData[\"e\"] = \"" + gerador.MensagemErroExclusao + "\";");
            controlador.AppendLine("                TempData[\"Exception\"] = ex;");
            controlador.AppendLine("            }");
            controlador.AppendLine("            return RedirectToAction(\"Indice\");");
            controlador.AppendLine("        }");
            controlador.AppendLine("        public void CarregarViewBags()");
            controlador.AppendLine("        {");
            foreach (var item in fk)
            {
                controlador.AppendLine("            ViewBag."+item.FieldName+" = "+item.FkTable.Remove(0, 2)+"Repository.List(\"" +item.FkField+" ASC\", 0, 150).Select(x => new { x.Id, x."+ item.FkField + " });");
            }
            controlador.AppendLine("        }");
            controlador.AppendLine("    }");
            controlador.AppendLine("}");
            controlador.AppendLine("");
            return controlador;
        }

        private static StringBuilder GerarIndice(Gerador gerador)
        {
            var tabela = gerador.GeradorItem.Where(s => s.ListarTabela == true).ToList();

            StringBuilder index = new StringBuilder();
            index.AppendLine("@model List<Simple.MVC.Business."+ gerador.Area+ "." + gerador.Classe + ">");
            index.AppendLine("");
            index.AppendLine("@{ ViewBag.Title = \"" + gerador.Apresentacao + "\"; }");
            index.AppendLine("<div class=\"page-header\">");
            index.AppendLine("    <h1>@ViewBag.Title</h1>");
            index.AppendLine("    <div class=\"pull-right novo-page-header\">");
            index.AppendLine("        <a title=\"" + gerador.Classe + "\" href=\"@Url.Action(\"Criar\", \"" + gerador.Classe + "\")\"><i title=\"Novo\" class=\"fa fa-plus-circle fa-3x\"></i><span class=\"hide\"></span></a>");
            index.AppendLine("    </div>");
            index.AppendLine("</div>");
            index.AppendLine("");
            index.AppendLine("@RenderPage(\"~/Views/Shared/_filterIndex.cshtml\")");
            index.AppendLine("");
            index.AppendLine("<table id=\"tbDados\" class=\"table table-hover\">");
            index.AppendLine("    <thead class=\"tb-primary\">");
            index.AppendLine("        <tr>");
            foreach (var item in tabela)
            {
                index.AppendLine("            <th>" + item.NomeApresentacao + "</th>");
            }
            index.AppendLine("            <th class=\"acoes\">Ações</th>");
            index.AppendLine("        </tr>");
            index.AppendLine("    </thead>");
            index.AppendLine("</table>");
            index.AppendLine("");
            index.AppendLine("<div class=\"hidden\" id=\"action-form-list\">");
            index.AppendLine("    <a class=\"btnModal\" href=\"javascript:;\" data-href=\"@Url.Action(\"Detalhes\", \"" + gerador.Classe + "\")/{_Id_}\"> <i title=\"Visualizar\" class=\"fa fa-search fa-lg\"></i><span class=\"hide\">Visualizar</span></a> |");
            index.AppendLine("    <a href=\"@Url.Action(\"Editar\", \"" + gerador.Classe + "\")/{_Id_}\"> <i title=\"Editar\" class=\"fa fa-edit fa-lg\"></i><span class=\"hide\"></span></a> |");
            index.AppendLine("    <a class=\"btnModal\" href=\"javascript:;\" data-href=\"@Url.Action(\"Excluir\", \"" + gerador.Classe + "\")/{_Id_}\"><i title=\"Excluir\" class=\"fa fa-times fa-lg icon-red\"></i><span class=\"hide\"></span></a>");
            index.AppendLine("</div>");
            index.AppendLine("@section head{");
            index.AppendLine("    <link href=\"~/Content/datatables/dataTables.bootstrap.css\" rel=\"stylesheet\" />");
            index.AppendLine("}");
            index.AppendLine("@section scripts{");
            index.AppendLine("<script type=\"text/javascript\" src=\"~/Content/datatables/jquery.dataTables.js\"></script>");
            index.AppendLine("<script type=\"text/javascript\" src=\"~/Content/datatables/dataTables.bootstrap.js\"></script>");
            index.AppendLine("<script type=\"text/javascript\" src=\"~/Content/datatables/CustomTable.js\"></script>");
            index.AppendLine("<script type=\"text/javascript\">");
            index.AppendLine("    $(document).ready(function () {");
            index.AppendLine("        ct = new CustomTable();");
            index.AppendLine("        ct.ActionUrl = '@Url.Action(\"Indice\")';");
            index.AppendLine("        ct.CustomParameters = [{ 'name': 'searchText', 'element': $(\"#filtro\") }],");
            index.AppendLine("        ct.Columns = [");
            foreach (var item in tabela)
            {
                if (item.FkTable == null)
                {
                    index.AppendLine("            { \"data\": \"" + item.FieldName + "\" },");
                }
                else
                {
                    index.AppendLine("            { \"data\": \"" + item.FkTable.Remove(0, 2) +"."+ item.FkField + "\" },");
                }
            }
            index.AppendLine("            {");
            index.AppendLine("                \"data\": \"Id\",");
            index.AppendLine("                \"sortable\": false,");
            index.AppendLine("                \"searchable\": false,");
            index.AppendLine("                \"className\": \"text-center\",");
            index.AppendLine("                \"render\": function (data, type, full, meta) {");
            index.AppendLine("                    return $('#action-form-list').html().replace(/{_Id_}/g, data);");
            index.AppendLine("                }");
            index.AppendLine("            }");
            index.AppendLine("        ];");
            index.AppendLine("");
            index.AppendLine("        ct.Bind();");
            index.AppendLine("");
            index.AppendLine("        $('#frmFiltro').on('submit', function (evt) {");
            index.AppendLine("            evt.preventDefault();");
            index.AppendLine("            ct.Refresh();");
            index.AppendLine("        });");
            index.AppendLine("    });");
            index.AppendLine("</script>");
            index.AppendLine("}");
            return index;
        }
        private static StringBuilder GerarDetalhes(Gerador gerador)
        {
            StringBuilder detalhes = new StringBuilder();
            detalhes.AppendLine("@model Simple.MVC.Business." + gerador.Area + "." + gerador.Classe + "");
            detalhes.AppendLine("@using Simple.MVC.Business.Util;");
            detalhes.AppendLine("");
            detalhes.AppendLine("@{");
            detalhes.AppendLine("    ViewBag.Title = \"Detalhes - "+ gerador.Apresentacao + "\";");
            detalhes.AppendLine("    Layout = null;");
            detalhes.AppendLine("}");
            detalhes.AppendLine("");
            detalhes.AppendLine("<div class=\"modal-dialog\">");
            detalhes.AppendLine("    <div class=\"modal-content\">");
            detalhes.AppendLine("        <div class=\"modal-header\">");
            detalhes.AppendLine("            <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">&times;</button>");
            detalhes.AppendLine("            <h4 class=\"modal-title\">@ViewBag.Title</h4>");
            detalhes.AppendLine("        </div>");
            detalhes.AppendLine("        <div class=\"modal-body\">");
            detalhes.AppendLine("            <dl class=\"dl-horizontal\">");
            foreach (var item in gerador.GeradorItem)
            {
                detalhes.AppendLine("                <dt>" + item.NomeApresentacao + ":</dt>");
                if (item.FkTable != null)
                {
                    detalhes.AppendLine("                <dd>&nbsp;@Model." + item.FkTable.Remove(0,2) + "."+item.FkField +"</dd>");
                }
                else
                {
                    detalhes.AppendLine("                <dd>&nbsp;@Model." + item.FieldName + "</dd>");
                }
            }
            detalhes.AppendLine("            </dl>");
            detalhes.AppendLine("        </div>");
            detalhes.AppendLine("    </div>");
            detalhes.AppendLine("</div>");
            return detalhes;
        }

        private static StringBuilder GerarCriar(Gerador gerador)
        {
            StringBuilder criar = new StringBuilder();
            criar.AppendLine("@model Simple.MVC.Business." + gerador.Area + "." + gerador.Classe + "");
            criar.AppendLine("@using Simple.MVC.Business.Util;");
            criar.AppendLine("@{ ");
            criar.AppendLine("    ViewBag.Title = \"Criar - "+ gerador.Apresentacao + "\";");
            criar.AppendLine("}");
            criar.AppendLine("<div class=\"page-header\">");
            criar.AppendLine("    <h1>@ViewBag.Title</h1>");
            criar.AppendLine("</div>");
            criar.AppendLine("@using (Html.BeginForm())");
            criar.AppendLine("{");
            criar.AppendLine("    @Html.ValidationSummary(\"Validação\", new { @class=\"alert alert-danger summary\" })");
            criar.AppendLine("");
            criar.AppendLine("    <div class=\"row\">");
            foreach (var item in gerador.GeradorItem)
            {
                if (item.FieldName != "Id")
                {
                    criar.AppendLine("        <div class=\"col-xs-12\">");
                    criar.AppendLine("            <div class=\"form-group\">");
                    criar.AppendLine("                @Html.LabelFor(m => m." + item.FieldName + ")"+((!item.IsNullable)? "*" : "")+"");
                    if (item.FkTable != null)
                    {
                        criar.AppendLine("                @Html.DropDownListFor(model => model." + item.FieldName + ", new SelectList(ViewBag." + item.FieldName + ", \"Id\", \""+item.FkField+"\", Model." + item.FieldName + "), \"Selecione... \", new { @class = \"form-control\"})");
                    }
                    else
                    {
                        criar.AppendLine("                @Html.EditorFor(model => model." + item.FieldName + ", new { htmlAttributes = new { @class = \"form-control\"} })");
                    }
                    criar.AppendLine("            </div>");
                    criar.AppendLine("        </div>");
                }
            }
            criar.AppendLine("    </div>");
            criar.AppendLine("    <div class=\"form-group well\">");
            criar.AppendLine("        <button type=\"submit\" class=\"btn btn-primary\">Salvar</button>");
            criar.AppendLine("        <a href=\"@Url.Action(\"Indice\")\" class=\"btn btn-default\">Voltar</a>");
            criar.AppendLine("    </div>");
            criar.AppendLine("}");
            criar.AppendLine("");
            criar.AppendLine("@section scripts{}");
            return criar;
        }

        private static StringBuilder GerarEditar(Gerador gerador)
        {
            StringBuilder editar = new StringBuilder();
            editar.AppendLine("@model Simple.MVC.Business." + gerador.Area + "." + gerador.Classe + "");
            editar.AppendLine("@using Simple.MVC.Business.Util;");
            editar.AppendLine("@{ ");
            editar.AppendLine("    ViewBag.Title = \"Editar - " + gerador.Apresentacao + "\";");
            editar.AppendLine("}");
            editar.AppendLine("<div class=\"page-header\">");
            editar.AppendLine("    <h1>@ViewBag.Title</h1>");
            editar.AppendLine("</div>");
            editar.AppendLine("@using (Html.BeginForm())");
            editar.AppendLine("{");
            editar.AppendLine("@Html.HiddenFor(m => m.Id)");
            editar.AppendLine("    @Html.ValidationSummary(\"Validação\", new { @class=\"alert alert-danger summary\" })");
            editar.AppendLine("");
            editar.AppendLine("    <div class=\"row\">");
            foreach (var item in gerador.GeradorItem)
            {
                if (item.FieldName != "Id")
                {
                    editar.AppendLine("        <div class=\"col-xs-12\">");
                    editar.AppendLine("            <div class=\"form-group\">");
                    editar.AppendLine("                @Html.LabelFor(m => m." + item.FieldName + ")" + ((!item.IsNullable) ? "*" : ""));
                    if (item.FkTable != null)
                    {
                        editar.AppendLine("                @Html.DropDownListFor(model => model." + item.FieldName + ", new SelectList(ViewBag." + item.FieldName + ", \"Id\", \"" + item.FkField + "\", Model." + item.FieldName + "), \"Selecione... \", new { @class = \"form-control\"})");
                    }
                    else
                    {
                        editar.AppendLine("                @Html.EditorFor(model => model." + item.FieldName + ", new { htmlAttributes = new { @class = \"form-control\"} })");
                    }
                    editar.AppendLine("            </div>");
                    editar.AppendLine("        </div>");
                }
            }
            editar.AppendLine("    </div>");
            editar.AppendLine("    <div class=\"form-group well\">");
            editar.AppendLine("        <button type=\"submit\" class=\"btn btn-primary\">Salvar</button>");
            editar.AppendLine("        <a href=\"@Url.Action(\"Indice\")\" class=\"btn btn-default\">Voltar</a>");
            editar.AppendLine("    </div>");
            editar.AppendLine("}");
            editar.AppendLine("");
            editar.AppendLine("@section scripts{}");
            return editar;
        }

        private static StringBuilder GerarExcluir(Gerador gerador)
        {
            StringBuilder excluir = new StringBuilder();
            excluir.AppendLine("@model Simple.MVC.Business." + gerador.Area + "." + gerador.Classe + "");
            excluir.AppendLine("@using Simple.MVC.Business.Util;");
            excluir.AppendLine("@{");
            excluir.AppendLine("    ViewBag.Title = \"Excluir - "+ gerador.Apresentacao + "\";");
            excluir.AppendLine("    Layout = null;");
            excluir.AppendLine("}");
            excluir.AppendLine("");
            excluir.AppendLine("<div class=\"modal-dialog\">");
            excluir.AppendLine("    <div class=\"modal-content\">");
            excluir.AppendLine("        <div class=\"modal-header\">");
            excluir.AppendLine("            <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">&times;</button>");
            excluir.AppendLine("            <h4 class=\"modal-title\">@ViewBag.Title</h4>");
            excluir.AppendLine("        </div>");
            excluir.AppendLine("        <div class=\"modal-body\">");
            excluir.AppendLine("            <dl class=\"dl-horizontal\">");
            foreach (var item in gerador.GeradorItem)
            {
                excluir.AppendLine("                <dt>" + item.NomeApresentacao + ":</dt>");
                if (item.FkTable != null)
                {
                    excluir.AppendLine("                <dd>&nbsp;@Model." + item.FkTable.Remove(0, 2) + "." + item.FkField + "</dd>");
                }
                else
                {
                    excluir.AppendLine("                <dd>&nbsp;@Model." + item.FieldName + "</dd>");
                }
            }
            excluir.AppendLine("            </dl>");
            excluir.AppendLine("        </div>");
            excluir.AppendLine("        <div class=\"modal-footer\">");
            excluir.AppendLine("            @using (Html.BeginForm())");
            excluir.AppendLine("            {");
            excluir.AppendLine("                @Html.HiddenFor(m => m.Id)");
            excluir.AppendLine("                <p>");
            excluir.AppendLine("                    <button type=\"submit\" class=\"btn btn-danger\">Excluir</button>");
            excluir.AppendLine("                    <button type=\"button\" class=\"btn btn-default\" data-dismiss=\"modal\">Fechar</button>");
            excluir.AppendLine("                </p>");
            excluir.AppendLine("            }");
            excluir.AppendLine("        </div>");
            excluir.AppendLine("    </div>");
            excluir.AppendLine("</div>");

            return excluir;
        }
    }
}
