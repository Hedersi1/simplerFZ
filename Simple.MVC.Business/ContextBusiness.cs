using Simple.MVC.Core.Data;
using System.Data.Entity;
using Simple.MVC.Business.Seguranca;
using Simple.MVC.Business.FZ;

namespace Simple.MVC.Business
{
    public class ContextBusiness : ContextCore
    {
        
        public ContextBusiness() : base("Default") { Database.SetInitializer<ContextBusiness>(null); }

        //Segurança
        public DbSet<DBEstrutura> DBEstrutura { get; set; }
        public DbSet<Gerador> Gerador { get; set; }
        public DbSet<GeradorItem> GeradorItem { get; set; }
        public DbSet<GeradorItemValidacao> GeradorItemValidacao { get; set; }
        public DbSet<GeradorItemTipo> GeradorItemTipo { get; set; }
        public DbSet<Papel> Papel { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<Sistema> Sistema { get; set; }
        public DbSet<Grupo> Grupo { get; set; }
        public DbSet<AutorizacaoConfiguracao> AutorizacaoConfiguracao { get; set; }
        public DbSet<Autorizacao> Autorizacao { get; set; }

        //FZ
        public DbSet<AgentePerfil> AgentePerfil { get; set; }
        public DbSet<Pessoa> Pessoa { get; set; }
        public DbSet<PessoaFisica> PessoaFisica { get; set; }
        public DbSet<PessoaStatus> PessoaStatus { get; set; }
        public DbSet<PessoaJuridica> PessoaJuridica { get; set; }
        public DbSet<Pais> Pais { get; set; }
        public DbSet<Estado> Estado { get; set; }
        public DbSet<Cidade> Cidade { get; set; }
        public DbSet<Patrocinador> Patrocinador { get; set; }
        public DbSet<Beneficiado> Beneficiado { get; set; }
        public DbSet<Doacao> Doacao { get; set; }

        public DbSet<Beneficio> Beneficio { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Papel>().
            HasMany(p => p.Grupos).
            WithMany(g => g.Papeis).
            Map(t => t.MapLeftKey("IdPapel")
                .MapRightKey("IdGrupo")
                .ToTable("SEPapelGrupo"));

            modelBuilder.Entity<Menu>().
            HasMany(p => p.Grupos).
            WithMany(g => g.Menus).
            Map(t => t.MapLeftKey("IdMenu")
               .MapRightKey("IdGrupo")
               .ToTable("SEMenuGrupo"));

            base.OnModelCreating(modelBuilder);
        }
    }
}
