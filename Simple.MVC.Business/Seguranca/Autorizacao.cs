using Simple.MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Simple.MVC.Business.Seguranca
{
    [Table("SEAutorizacao"), Serializable]
    public class Autorizacao : Entity
    {
        public int IdUsuarioLogado { get; set; }

        public int IdAutorizacaoConfiguracao { get; set; }

        public int IdUsuarioAutorizador { get; set; }

        public DateTime Data { get; set; }

        public string Ip { get; set; }
    }
}

