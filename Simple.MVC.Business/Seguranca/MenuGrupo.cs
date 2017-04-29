using Simple.MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Simple.MVC.Business.Seguranca
{
    [Table("SEMenuGrupo"), Serializable]
    public class MenuGrupo : Entity
    {

        [Display(Name="IdMenu")]
        public Int64 IdMenu { get; set; }

        [Display(Name="IdGrupo")]
        public Int32 IdGrupo { get; set; }
    }
}

