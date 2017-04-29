using Simple.MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Simple.MVC.Business.FZ
{
    [Table("Patrocinador"), Serializable]
    public class Patrocinador : Entity
    {
        [Required]
        [Display(Name="Nome")]
        public String Nome { get; set; }

        [Required]
        [Display(Name="Logomarca")]
        public String Logomarca { get; set; }
    }
}

