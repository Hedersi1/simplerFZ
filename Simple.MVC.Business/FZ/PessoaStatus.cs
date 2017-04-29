using Simple.MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Simple.MVC.Business.FZ
{
    [Table("PessoaStatus"), Serializable]
    public class PessoaStatus : Entity
    {

        [Required]
        [Display(Name="Descrição")]
        public String Descricao { get; set; }
    }
}

