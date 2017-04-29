using Simple.MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Simple.MVC.Business.FZ
{
    [Table("DoacaoBeneficiario"), Serializable]
    public class DoacaoBeneficiario : Entity
    {

        [Required]
        [Display(Name="Beneficiado")]
        public Int32 IdBeneficiado { get; set; }

        [Required]
        [Display(Name="Agente Fome Zero")]
        public Int32 IdAgenteFZ { get; set; }

        [Required]
        [Display(Name="Data")]
        public DateTime Data { get; set; }
        [ForeignKey("IdBeneficiado")]
        public Beneficiado Beneficiado { get; set; }
        [ForeignKey("IdAgenteFZ")]
        public PessoaFisica PessoaFisica { get; set; }
    }
}

