using Simple.MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Simple.MVC.Business.Seguranca
{
    [Table("SEAutorizacaoConfiguracao"), Serializable]
    public class AutorizacaoConfiguracao : Entity
    {
        public long IdPapel { get; set; }

        [ForeignKey("IdPapel")]
        public Papel Papel { get; set; }

        public bool Status { get; set; }
    }
}

