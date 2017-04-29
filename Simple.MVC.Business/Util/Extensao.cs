using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Simple.MVC.Business.Util
{
    public static class Extensao
    { 
        public static string TarefaTipoRepeticaoToLabel(string ttr)
        {
            string saida = "";

            switch (ttr)
            {
                case "SemRecorrencia":
                    saida = "Sem recorrência";
                    break;
                case "Diario":
                    saida = "Diário";
                    break;
                default:
                    saida = ttr;
                    break;
            }

            return saida;
        }

        public static String GeradorItemToDotNetType(Seguranca.GeradorItem item)
        {
            return ToDotNetType(item.DataType, item.IsNullable);
        }

        public static String  ToDotNetType(String dbtype, Boolean isnullable)
        {
            string saida = "";

            switch (dbtype)
            {
                case "bigint": saida = "Int64"; break;
                case "binário": saida = "Byte[]"; break;
                case "bit": saida = "Boolean"; break;
                case "char": saida = "Char[]"; break;
                case "Date ": saida = "DateTime"; break;
                case "datetime": saida = "DateTime"; break;
                case "datetime2": saida = "DateTime"; break;
                case "datetimeoffset": saida = "DateTimeOffset"; break;
                case "decimal": saida = "Decimal"; break;
                case "varbinary": saida = "Byte[]"; break;
                case "float": saida = "Double"; break;
                case "image ": saida = "Byte[]"; break;
                case "int": saida = "Int32"; break;
                case "money": saida = "Decimal"; break;
                case "nchar": saida = "String"; break;
                case "ntext": saida = "String"; break;
                case "numérico": saida = "Decimal"; break;
                case "nvarchar": saida = "String"; break;
                case "real": saida = "Single"; break;
                case "rowversion": saida = "Byte[]"; break;
                case "smalldatetime": saida = "DateTime"; break;
                case "smallint": saida = "Int16"; break;
                case "smallmoney": saida = "Decimal"; break;
                case "sql_variant": saida = "String"; break;
                case "text": saida = "String"; break;
                case "hora": saida = "TimeSpan"; break;
                case "carimbo data/hora": saida = "Byte[]"; break;
                case "tinyint": saida = "Byte"; break;
                case "uniqueidentifier": saida = "Guid"; break;
                case "varchar": saida = "String"; break;
                case "xml": saida = "Xml"; break;
                default: break;
            }
            if (isnullable && saida != "String")
            {
                saida = "Nullable<" + saida + ">";
            }
            return saida;
        }
    }
}
