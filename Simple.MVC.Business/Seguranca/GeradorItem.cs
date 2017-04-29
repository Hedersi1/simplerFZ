using Simple.MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simple.MVC.Business.Seguranca
{
    [Table("SEGeradorItem"), Serializable]
    public class GeradorItem : Entity
    {
        public int IdGerador { get; set; }
        public int? IdGeradorItemTipo { get; set; }
        public int? IdValidacao1 { get; set; }
        public int? IdValidacao2 { get; set; }
        public String NomeApresentacao { get; set; }
        public String FieldName { get; set; }
        public String DataType { get; set; }
        public String TipoCSharp { get; set; }
        public Int16 MaxLength { get; set; }
        public bool IsNullable { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool ListarTabela { get; set; }

        [ForeignKey("IdGerador")]
        public Gerador Gerador { get; set; }

        [ForeignKey("IdGeradorItemTipo")]
        public GeradorItemTipo GeradorItemTipo { get; set; }

        [ForeignKey("IdValidacao1")]
        public GeradorItemValidacao GeradorItemValidacao1 { get; set; }

        [ForeignKey("IdValidacao2")]
        public GeradorItemValidacao GeradorItemValidacao2 { get; set; }

        public String FkTable { get; set; }
        public String FkField { get; set; }

    }
}
