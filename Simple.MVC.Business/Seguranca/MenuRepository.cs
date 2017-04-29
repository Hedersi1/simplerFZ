using Simple.MVC.Core.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simple.MVC.Business.Seguranca
{
    public class MenuRepository : Repository<Menu, ContextBusiness>
    {
        public static Menu FirstOrDefault(Int64 idsistema, String nome)
        {
            using (var ctx = new ContextBusiness())
            {
                return ctx.Menu.Where(s => s.Nome == nome && s.IdSistema == idsistema).FirstOrDefault();
            }
        }

        public static void Save(Sistema sistema, Gerador gerador)
        {
            using (var ctx = new ContextBusiness())
            {
                Grupo grupo = ctx.Grupo.Where(s => s.Origem.Contains(gerador.Area) && s.Nome.Contains("administrador")).FirstOrDefault();

                if (MenuRepository.FirstOrDefault(sistema.Id, gerador.Classe) == null)
                {
                    Menu menu = new Menu
                    {
                        IdSistema = sistema.Id,
                        Nome = gerador.Classe,
                        ClasseIcone = "fa-chevron-right",
                        Descricao = gerador.Classe,
                        Grupos = new List<Grupo>()
                    };
                    menu.Grupos.Add(grupo);

                    menu.MenusFilhos = new List<Menu>();
                    menu.MenusFilhos.Add(new Menu
                    {
                        IdSistema = sistema.Id,
                        Nome = "Gerenciar",
                        ClasseIcone = "fa-chevron-right",
                        Descricao = "Gerenciar " + gerador.Classe,
                        Acao = "Indice",
                        Controlador = gerador.Classe,
                        Grupos = new List<Grupo> { grupo }
                    });

                    ctx.Entry(menu).State = EntityState.Added;
                    ctx.SaveChanges();
                }
            }
        }
    }
}

