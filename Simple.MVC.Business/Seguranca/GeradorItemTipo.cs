using Simple.MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simple.MVC.Business.Seguranca
{
    [Table("SEGeradorItemTipo"), Serializable]
    public class GeradorItemTipo : Entity
    {
        public String Tipo { get; set; }
        public String Validacao { get; set; }

    }
}
