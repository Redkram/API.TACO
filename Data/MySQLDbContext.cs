using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Data
{
    public class MySQLDbContext : DbContext
    {
        public DbSet<VIEW_MS_ALL_USERS_DATA> DbVIEW_MS_USER_DATA { get; set; }
        public MySQLDbContext(DbContextOptions<MySQLDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Obtiene todas las propiedades de tipo DbSet en el DbContext
            var dbSetProperties = this.GetType().GetProperties()
                .Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));

            foreach (var dbSetProperty in dbSetProperties)
            {
                // Mapea automáticamente cada entidad a una tabla con el mismo nombre que la entidad
                var entityType = dbSetProperty.PropertyType.GetGenericArguments()[0];
                modelBuilder.Entity(entityType).ToTable(entityType.Name);
            }

            modelBuilder.Entity<VIEW_MS_ALL_USERS_DATA>(entity =>
            {
                entity.HasKey(e => e.PERSONNELNUMBER);
            });

            base.OnModelCreating(modelBuilder);  // Llamada a la base para cualquier configuración adicional
        }
    }
}
