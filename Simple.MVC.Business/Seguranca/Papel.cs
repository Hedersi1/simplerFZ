using Simple.MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Simple.MVC.Business.Seguranca
{
    [Table("SEPapel"), Serializable]
    public class Papel : Entity
    {
        [Key]
        public new Int64 Id { get; set; }

        [Display(Name="Nome")]
        public String Nome { get; set; }

        [Display(Name="Descricao")]
        public String Descricao { get; set; }

        [Display(Name="IdSistema")]
        public Int64 IdSistema { get; set; }

        public virtual ICollection<Grupo> Grupos { get; set; }
    }
}

