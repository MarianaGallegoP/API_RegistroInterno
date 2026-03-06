using API_RegistroInterno.Entities;
using Microsoft.EntityFrameworkCore;

namespace API_RegistroInterno.Context
{
    /// <summary>
    /// Contexto de base de datos para DbOyD (tblClientes).
    /// </summary>
    public class OyDDbContext : DbContext
    {
        public OyDDbContext(DbContextOptions<OyDDbContext> options) : base(options)
        {
        }

        public DbSet<Cliente> tblClientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("tblClientes", "dbo");
                entity.HasNoKey(); // Solo se usan estos campos para validación; la tabla tiene más columnas.

                entity.Property(e => e.TipoDocumento).HasColumnName("strTipoIdentificacion");
                entity.Property(e => e.Documento).HasColumnName("strNroDocumento");
                entity.Property(e => e.Nombres).HasColumnName("strNombre");
                entity.Property(e => e.Celular).HasColumnName("strTelefono1");
                entity.Property(e => e.PrimerApellido).HasColumnName("strApellido1");
                entity.Property(e => e.SegundoApellido).HasColumnName("strApellido2");
                entity.Property(e => e.FechaExpedicion).HasColumnName("dtmFechaExpedicionDoc");
                entity.Property(e => e.Correo).HasColumnName("strEMail");
            });
        }
    }
}
