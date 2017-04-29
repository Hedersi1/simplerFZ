using Simple.MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Simple.MVC.Business.FZ
{
    [Table("PessoaFisica"), Serializable]
    public class PessoaFisica : Pessoa
    {
        [Display(Name="CPF")]
        public String CPF { get; set; }

        [Display(Name="Agente Distribuidor")]
        public Nullable<Int32> IdAgenteDistribuidor { get; set; }
        [ForeignKey("IdAgenteDistribuidor")]
        public PessoaJuridica PessoaJuridica { get; set; }
    }
}

