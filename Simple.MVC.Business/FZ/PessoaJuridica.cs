using Simple.MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Simple.MVC.Business.FZ
{
    [Table("PessoaJuridica"), Serializable]
    public class PessoaJuridica : Pessoa
    {
        [Required]
        [Display(Name="CNPJ")]
        public String CNPJ { get; set; }
    }
}

