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
    public class PapelRepository : Repository<Papel, ContextBusiness>
    {
        public static void Save(Papel obj, Grupo grupo)
        {
            using (var ctx = new ContextBusiness())
            {
                if (PapelRepository.FirstOrDefault(obj.Nome) == null)
                {
                    obj.Grupos = ctx.Grupo.Where(s => s.Id == grupo.Id).ToList();
                    ctx.Entry(obj).State = EntityState.Added;
                    ctx.SaveChanges();
                }
            }
        }

        public static Papel FirstOrDefault(String nome)
        {
            using (var ctx = new ContextBusiness())
            {
                return ctx.Papel.Where(s => s.Nome == nome).FirstOrDefault();
            }
        }
    }
}

