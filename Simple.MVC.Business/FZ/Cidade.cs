using Simple.MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Simple.MVC.Business.FZ
{
    [Table("Cidade"), Serializable]
    public class Cidade : Entity
    {

        [Required]
        [Display(Name="Nome")]
        public String Nome { get; set; }

        [Required]
        [Display(Name="Estado")]
        public Int32 IdEstado { get; set; }
        [ForeignKey("IdEstado")]
        public tado tado { get; set; }
    }
}

