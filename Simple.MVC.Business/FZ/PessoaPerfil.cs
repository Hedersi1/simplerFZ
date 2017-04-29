using Simple.MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Simple.MVC.Business.FZ
{
    [Table("PessoaPerfil"), Serializable]
    public class PessoaPerfil : Entity
    {

        [Display(Name="Pessoa")]
        public Int32 IdPessoa { get; set; }

        [Display(Name="Perfil")]
        public Int32 IdAgentePerfil { get; set; }
        [ForeignKey("IdPessoa")]
        public Pessoa Pessoa { get; set; }
        [ForeignKey("IdAgentePerfil")]
        public AgentePerfil AgentePerfil { get; set; }
    }
}

