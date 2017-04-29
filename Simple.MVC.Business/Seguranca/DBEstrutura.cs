using Simple.MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simple.MVC.Business.Seguranca
{
    [Table("DBEstrutura"), Serializable]
    public class DBEstrutura : Entity
    {
        public String TableName { get; set; }
        public String FieldName { get; set; }
        //public int IdDataType { get; set; }
        public String DataType { get; set; }
        public Int16 MaxLength { get; set; }
        public bool IsNullable { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsPrimaryKey { get; set; }
        public String ConstraintName { get; set; }
        public String PkTableName { get; set; }
        public String PkColumnName { get; set; }


    }
}
