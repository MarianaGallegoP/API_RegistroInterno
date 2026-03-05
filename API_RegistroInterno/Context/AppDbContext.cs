using API_RegistroInterno.Entities;
using Microsoft.EntityFrameworkCore;

namespace API_RegistroInterno.Context
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }

        public DbSet<UsuarioClientesExternos> tblUsuariosClientesExternos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UsuarioClientesExternos>(entity =>
            {

                entity.ToTable("tblUsuariosClientesExternos", "dbo");
                entity.HasKey(e => e.id);

                entity.Property(e => e.id)
                                .HasColumnName("intId");

                entity.Property(e => e.tipoDocumento)
                                .HasColumnName("vchTipoDocumento");

                entity.Property(e => e.documento)
                                .HasColumnName("vchDocumento");

                entity.Property(e => e.expedicion)
                                .HasColumnName("dtmExpidicion");

                entity.Property(e => e.nombre1)
                                .HasColumnName("vchNombre1");

                entity.Property(e => e.nombre2)
                                .HasColumnName("vchNombre2");

                entity.Property(e => e.apellido1)
                                .HasColumnName("vchApellido1");

                entity.Property(e => e.apellido2)
                                .HasColumnName("vchApellido2");

                entity.Property(e => e.nombre)
                                .HasColumnName("vchNombre");

                entity.Property(e => e.email)
                                .HasColumnName("vchEmail");

                entity.Property(e => e.celular)
                                .HasColumnName("vchCelular")
                                .IsRequired();

                entity.Property(e => e.clave)
                                .HasColumnName("vchClave")
                                .IsRequired();

                entity.Property(e => e.intentosFallidos)
                                .HasColumnName("intIntentosFallidos");

                entity.Property(e => e.ultimoAcceso)
                              .HasColumnName("dtmUltimoAcceso")
                              .IsRequired();

                entity.Property(e => e.estado)
                      .HasColumnName("intEstado");

                entity.Property(e => e.creacion)
                                .HasColumnName("dtmCreacion");

                entity.Property(e => e.actualizacion)
                                .HasColumnName("dtmActualizacion")
                                .IsRequired();


            });
        }
    }
}
