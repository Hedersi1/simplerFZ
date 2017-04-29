using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Simple.MVC.WEB.Models
{
    public class AutorizacaoModel
    {
        [Required]
        public string login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }

        public string papel { get; set; }

        public string formId { get; set; }
    }
}