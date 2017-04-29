using Simple.MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Simple.MVC.Business.Seguranca
{
    [Table("SESistema"), Serializable]
    public class Sistema : Entity
    {
        [Key]
        public new Int64 Id { get; set; }

        [Display(Name="Nome")]
        public string Nome { get; set; }

        [Display(Name="NomeExibicao")]
        public String NomeExibicao { get; set; }

        [Display(Name="Descricao")]
        public String Descricao { get; set; }

        [Display(Name="Caminho")]
        public String Caminho { get; set; }

        [Display(Name="IdAplicacao")]
        public String IdAplicacao { get; set; }

        [Display(Name="PaginaAutenticacao")]
        public String PaginaAutenticacao { get; set; }

        [Display(Name="PaginaRetorno")]
        public String PaginaRetorno { get; set; }

        [Display(Name="Icone")]
        public String Icone { get; set; }
    }
}

