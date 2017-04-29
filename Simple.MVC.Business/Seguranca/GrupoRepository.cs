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
    public class GrupoRepository : Repository<Grupo, ContextBusiness>
    {
        public static Grupo FirstOrDefault(String area)
        {
            using (var ctx  = new ContextBusiness())
            {
                return ctx.Grupo.Where(s => s.Origem.Contains(area) && s.Nome.Contains("administrador")).FirstOrDefault();
            }
        }
    }
}

