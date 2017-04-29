using Simple.MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Simple.MVC.Business.Seguranca
{
    [Table("SEGrupo"), Serializable]
    public class Grupo : Entity
    {

        [Display(Name="SId")]
        public String SId { get; set; }

        [Display(Name="Guid")]
        public String Guid { get; set; }

        [Display(Name="Nome")]
        public String Nome { get; set; }

        [Display(Name="Descricao")]
        public String Descricao { get; set; }

        [Display(Name="Origem")]
        public String Origem { get; set; }

        public virtual ICollection<Papel> Papeis { get; set; }
        public virtual ICollection<Menu> Menus { get; set; }
    }
}

