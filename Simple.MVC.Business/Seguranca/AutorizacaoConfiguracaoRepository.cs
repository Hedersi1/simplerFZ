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
    public class AutorizacaoConfiguracaoRepository : Repository<AutorizacaoConfiguracao, ContextBusiness>
    {
        public static AutorizacaoConfiguracao FirstOrDefault(string papel)
        {
            return FirstOrDefault(x => x.Papel.Nome == papel && x.Status == true, x => x.Papel);
        }
    }
}

