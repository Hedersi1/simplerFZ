using Simple.MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Simple.MVC.Business.Seguranca;

namespace Simple.MVC.Business.FZ
{
    [Table("Pessoa"), Serializable]
    public class Pessoa : Entity
    {

        [Required]
        [Display(Name="Nome")]
        public String Nome { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name="E-mail")]
        public String Email { get; set; }

        [Display(Name="Status")]
        public Int32 IdPessoaStatus { get; set; }

        [Display(Name="Usuario")]
        public Nullable<Int32> IdUsuario { get; set; }

        [Required]
        [Display(Name="Cidade")]
        public Int32 IdCidade { get; set; }

        [Display(Name="Endereço")]
        public String Endereco { get; set; }

        [Display(Name="Complemento")]
        public String Complemento { get; set; }

        [Display(Name="Número")]
        public String Numero { get; set; }

        [Display(Name="Bairro")]
        public String Bairro { get; set; }

        [Display(Name="Latitude")]
        public String Latitude { get; set; }
        [Display(Name = "Longitude")]
        public String Longitude { get; set; }
        [ForeignKey("IdPessoaStatus")]
        public PessoaStatus PessoaStatus { get; set; }
        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; }
        [ForeignKey("IdCidade")]
        public Cidade Cidade { get; set; }
    }
}

