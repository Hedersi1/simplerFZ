using Simple.MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Simple.MVC.Business.FZ
{
    [Table("Estado"), Serializable]
    public class Estado : Entity
    {

        [Required]
        [Display(Name="Nome")]
        public String Nome { get; set; }

        [Required]
        [Display(Name="País")]
        public Int32 IdPais { get; set; }

        [Display(Name="Sigla")]
        public String Sigla { get; set; }
        [ForeignKey("IdPais")]
        public is is { get; set; }
    }
}

