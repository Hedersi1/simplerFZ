using Simple.MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Simple.MVC.Business.Seguranca
{
    [Table("SEUsuario"), Serializable]
    public class Usuario : Entity
    {
        [Display(Name="Guid")]
        public String Guid { get; set; }

        [Display(Name="Sid")]
        public String Sid { get; set; }

        [Display(Name="Login")]
        public String Login { get; set; }

        [Display(Name="Senha")]
        public String Senha { get; set; }

        [Display(Name="Salt")]
        public String Salt { get; set; }

        [Display(Name="DataCriacao")]
        public Nullable<DateTime> DataCriacao { get; set; }

        [Display(Name="UltimaAtividade")]
        public Nullable<DateTime> UltimaAtividade { get; set; }

        [Display(Name="UltimoLogin")]
        public Nullable<DateTime> UltimoLogin { get; set; }

        [Display(Name="UltimaTrocaSenha")]
        public Nullable<DateTime> UltimaTrocaSenha { get; set; }

        [Display(Name="EstaOnline")]
        public Nullable<Boolean> EstaOnline { get; set; }

        [Display(Name="LoginRM")]
        public String LoginRM { get; set; }

        [Display(Name="Token")]
        public String Token { get; set; }

        [Display(Name="DataExpiracaoToken")]
        public Nullable<DateTime> DataExpiracaoToken { get; set; }

        [Display(Name="MudaSenhaProximoLogon")]
        public Boolean MudaSenhaProximoLogon { get; set; }

        [Display(Name="Situacao")]
        public Int32 Situacao { get; set; }

        [Display(Name="DataExpiracaoUsuario")]
        public Nullable<DateTime> DataExpiracaoUsuario { get; set; }
    }
}

