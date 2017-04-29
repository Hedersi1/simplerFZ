using Simple.MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Simple.MVC.Business.Seguranca
{
    [Table("SEGeradorItemValidacao"), Serializable]
    public class GeradorItemValidacao : Entity
    {
        public String Texto { get; set; }
        public String Validacao { get; set; }
    }
}
