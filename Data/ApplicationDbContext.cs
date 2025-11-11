using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ContratistasMM.Models;

namespace ContratistasMM.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<Hito> Hitos { get; set; }

        public DbSet<ArchivoHito> ArchivosHito { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Documento> Documentos { get; set; }
        public DbSet<CategoriaReferencia> CategoriasReferencia { get; set; }
        public DbSet<RecursoReferencia> RecursosReferencia { get; set; }

        public DbSet<Personal> Personal { get; set; }

        public DbSet<PreguntaFrecuente> PreguntasFrecuentes { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Hito>()
                .HasMany(h => h.PersonalAsignado)
                .WithMany(p => p.HitosAsignados)
                .UsingEntity(j => j.ToTable("HitoPersonal")); 
        }


}
