using Simple.MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Simple.MVC.Business.FZ
{
    [Table("Pais"), Serializable]
    public class Pais : Entity
    {

        [Required]
        [Display(Name="Nome")]
        public String Nome { get; set; }
    }
}

