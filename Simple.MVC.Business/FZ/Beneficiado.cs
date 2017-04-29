using Simple.MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Simple.MVC.Business.FZ
{
    [Table("Beneficiado"), Serializable]
    public class Beneficiado : Entity
    {

        [Required]
        [Display(Name="Responsável")]
        public String NomeResponsavel { get; set; }

        [Display(Name="CPF")]
        public String CPF { get; set; }

        [Required]
        [Display(Name="Endereço")]
        public String Endereco { get; set; }

        [Display(Name="Complemento")]
        public String Complemento { get; set; }

        [Required]
        [Display(Name="Número")]
        public String Numero { get; set; }

        [Required]
        [Display(Name="Bairro")]
        public String Bairro { get; set; }

        [Required]
        [Display(Name="Cidade")]
        public Int32 IdCidade { get; set; }

        [Display(Name="Latitude")]
        public String Latitude { get; set; }

        [Required]
        [Display(Name="Situação")]
        public Int32 IdSituacaoBeneficiado { get; set; }

        [Display(Name="Longitude")]
        public String Longitude { get; set; }

        [ForeignKey("IdSituacaoBeneficiado")]
        public SituacaoBeneficiado SituacaoBeneficiado { get; set; }

        [ForeignKey("IdCidade")]
        public Cidade Cidade { get; set; }

        public int IdAgenteDistribuidor { get; set; }
    }
}

