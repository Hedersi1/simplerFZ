using Simple.MVC.Core.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Text;

namespace Simple.MVC.Business.FZ
{
    public class PessoaRepository : Repository<Pessoa, ContextBusiness>
    {
        public static List<Pessoa> ListAgenteDoador(string term, string order, int skip, int take)
        {
            return List(x => x.PessoaPerfil.Any(p => p.IdAgentePerfil == 1) && x.Nome.StartsWith(term), order, skip,
                take, x => x.PessoaStatus, x => x.Usuario, x => x.Cidade);
        }

        public static int CountAgenteDoador(string term)
        {
            using (var ctx = new ContextBusiness())
            {
                return ctx.Pessoa.Where(x => x.PessoaPerfil.Any(p => p.IdAgentePerfil == 1) && x.Nome.StartsWith(term))
                    .Count();
            }
           
        }

        public static List<Pessoa> ListAgenteFomeZero(string term, string order, int skip, int take)
        {
            return List(x => x.PessoaPerfil.Any(p => p.IdAgentePerfil == 2) && x.Nome.StartsWith(term), order, skip,
                take, x => x.PessoaStatus, x => x.Usuario, x => x.Cidade);
        }

        public static int CountAgenteFomeZero(string term)
        {
            using (var ctx = new ContextBusiness())
            {
                return ctx.Pessoa.Where(x => x.PessoaPerfil.Any(p => p.IdAgentePerfil == 2) && x.Nome.StartsWith(term))
                    .Count();
            }

        }
    }
}

