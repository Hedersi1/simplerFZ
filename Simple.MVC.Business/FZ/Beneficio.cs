using Simple.MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Simple.MVC.Business.FZ
{
    [Table("Beneficio"), Serializable]
    public class Beneficio : Entity
    {

        [Display(Name="Agente Parceiro")]
        public Int32 IdAgenteParceiro { get; set; }

        [Display(Name="Descrição")]
        public String Descricao { get; set; }

        [Display(Name="Valor")]
        public Double Valor { get; set; }

        [Display(Name="EhPorcentagem")]
        public Boolean EhPorcentagem { get; set; }
        [ForeignKey("IdAgenteParceiro")]
        public PessoaJuridica PessoaJuridica { get; set; }
    }
}

