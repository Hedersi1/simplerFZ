using Simple.MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simple.MVC.Business.Seguranca
{
    [Table("SEGerador"), Serializable]
    public class Gerador : Entity
    {
        public String Tabela { get; set; }
        public String Classe { get; set; }
        public String Area { get; set; }
        public String MensagemSucessoInsercao { get; set; }
        public String MensagemSucessoAlteracao { get; set; }
        public String MensagemSucessoExclusao { get; set; }
        public String MensagemErroInsercao { get; set; }
        public String MensagemErroAlteracao { get; set; }
        public String MensagemErroExclusao { get; set; }
        public String  Apresentacao { get; set; }

        public List<GeradorItem> GeradorItem { get; set; }
    }
}
