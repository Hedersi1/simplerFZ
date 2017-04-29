using Simple.MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Simple.MVC.Business.Seguranca
{
    [Table("SEMenu"), Serializable]
    public class Menu : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public new Int64 Id { get; set; }

        [Required]
        [Display(Name="IdSistema")]
        public Int64 IdSistema { get; set; }

        [Display(Name="IdMenuPai")]
        public Nullable<Int64> IdMenuPai { get; set; }

        [Required]
        [Display(Name="ClasseIcone")]
        public String ClasseIcone { get; set; }

        [Required]
        [Display(Name="Nome")]
        public String Nome { get; set; }

        [Required]
        [Display(Name="Acao")]
        public String Acao { get; set; }

        [Required]
        [Display(Name="Controlador")]
        public String Controlador { get; set; }

        [Display(Name="Descricao")]
        public String Descricao { get; set; }

        [InverseProperty("MenusFilhos")]
        [ForeignKey("IdMenuPai")]
        public Menu MenuPai { get; set; }
        public List<Menu> MenusFilhos { get; set; }

        public virtual ICollection<Grupo> Grupos { get; set; }
    }
}

