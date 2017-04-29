using Simple.MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Simple.MVC.Business.Seguranca
{
    [Table("SEPapelGrupo"), Serializable]
    public class PapelGrupo : Entity
    {

        [Display(Name="IdPapel")]
        public Int64 IdPapel { get; set; }

        [Display(Name="IdGrupo")]
        public Int32 IdGrupo { get; set; }
    }
}

