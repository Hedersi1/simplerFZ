using Simple.MVC.Core.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;

namespace Simple.MVC.Business.Seguranca
{
    public class SistemaRepository : Repository<Sistema, ContextBusiness>
    {
        public static Sistema FirstOrDefault(string area)
        {
            using (var ctx = new ContextBusiness())
            {
                return ctx.Sistema.Where(s => s.Caminho.Contains(area)).FirstOrDefault();
            }
        }
    }
}

