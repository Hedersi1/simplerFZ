using Simple.MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Simple.MVC.Business.FZ;

namespace Simple.MVC.Business.FZ
{
    [Table("Doacao"), Serializable]
    public class Doacao : Entity
    {

        [Required]
        [Display(Name="Agente Doador")]
        public Int32 IdAgenteDoador { get; set; }

        [Display(Name="Agente Fome Zero")]
        public Nullable<Int32> IdAgenteFZ { get; set; }

        [Required]
        [Display(Name="Data")]
        public DateTime Data { get; set; }

        [Required]
        [Display(Name="Possui Alimento Perecível?")]
        public Boolean PossuiPerecivel { get; set; }

        [Required]
        [Display(Name="Alimentos Necessitam de Refrigeração?")]
        public Boolean NecessitaRefrigeracao { get; set; }

        [Required]
        [Display(Name="Alimentos Necessitam de Congelamento?")]
        public Boolean NecessitaCongelamento { get; set; }

        [Required]
        [Display(Name="Latitude")]
        public String Latitude { get; set; }

        [Required]
        [Display(Name = "Longitude")]
        public String Longitude { get; set; }

        [Required]
        [Display(Name="Possui algum alimento com data de vencimento próxima?")]
        public Boolean PossuiVencimentoProximo { get; set; }

        [Required]
        [Display(Name="Peso Aproximado?")]
        public Double PesoAproximado { get; set; }

        [Required]
        [Display(Name="Período Disponível - Início")]
        public DateTime InicioDisponibilidadeEntrega { get; set; }

        [Required]
        [Display(Name="Período Disponível - Término")]

        public DateTime TerminoDisponibilidadeEntrega { get; set; }
        [ForeignKey("IdAgenteDoador")]

        public Pessoa Pessoa { get; set; }
        [ForeignKey("IdAgenteFZ")]
        public PessoaFisica PessoaFisica { get; set; }
    }
}

