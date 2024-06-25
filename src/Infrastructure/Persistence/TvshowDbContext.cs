using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tvshow.Domain;
using Tvshow.Domain.Common;

namespace Tvshow.Infrastructure.Persistence
{
    public class TvshowDbContext : IdentityDbContext<Usuario> {

        // Constructor del contexto de la base de datos
        public TvshowDbContext(DbContextOptions<TvshowDbContext> options) : base(options)
        {}

        // Define un DbSet para la entidad TvShow en la base de datos
        public DbSet<TvShow> TvShows { get; set; }

        // Sobrescribe el método SaveChangesAsync para agregar metadatos de auditoría antes de guardar cambios
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var userName = "system"; // Nombre de usuario predeterminado para auditoría

            // Itera sobre todas las entidades rastreadas por el ChangeTracker
            foreach (var entry in ChangeTracker.Entries<BaseDomainModel>())
            {
                switch (entry.State)
                {
                    // Cuando se agrega una nueva entidad
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now; // Establece la fecha de creación
                        entry.Entity.CreatedBy = userName; // Establece el creador
                        break;

                    // Cuando se modifica una entidad existente
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now; // Actualiza la fecha de última modificación
                        entry.Entity.LastModifiedBy = userName;  // Actualiza el último usuario modificador
                        break;
                }
            }
            // Guarda los cambios en la base de datos y devuelve el número de entidades afectadas
            return base.SaveChangesAsync(cancellationToken);
        }

        // Configuración del modelo de datos utilizando Fluent API
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);// Llama al método base para configuraciones predeterminadas

            // Configuración adicional del modelo usuario
            builder.Entity<Usuario>().Property(x => x.Id).HasMaxLength(36);
            builder.Entity<Usuario>().Property(x => x.NormalizedUserName).HasMaxLength(90);
            builder.Entity<IdentityRole>().Property(x => x.Id).HasMaxLength(36);
            builder.Entity<IdentityRole>().Property(x => x.NormalizedName).HasMaxLength(90);
        }
    }
}
