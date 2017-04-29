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
    public class DBEstruturaRepository : Repository<DBEstrutura, ContextBusiness>
    {
        public static List<DBEstrutura> List(String tabela)
        {
            using (var ctx = new ContextBusiness())
            {
               return ctx.DBEstrutura.Where(x => x.TableName == tabela).OrderBy(x => x.Id).AsNoTracking().ToList();
            }
        }
    }
}
