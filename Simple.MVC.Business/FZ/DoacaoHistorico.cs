using Simple.MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Simple.MVC.Business.FZ
{
    [Table("DoacaoHistorico"), Serializable]
    public class DoacaoHistorico : Entity
    {

        [Required]
        [Display(Name="Doação")]
        public Int32 IdDoacao { get; set; }

        [Required]
        [Display(Name="Agente Fome Zero")]
        public Int32 IdAgenteFZ { get; set; }

        [Required]
        [Display(Name="Data")]
        public DateTime Data { get; set; }

        [Required]
        [Display(Name="Situação")]
        public Int32 IdSituacaoDoacao { get; set; }
        [ForeignKey("Id")]
        public PessoaFisica PessoaFisica { get; set; }
        [ForeignKey("IdDoacao")]
        public Doacao Doacao { get; set; }
        [ForeignKey("IdSituacaoDoacao")]
        public SituacaoDoacao SituacaoDoacao { get; set; }
    }
}

