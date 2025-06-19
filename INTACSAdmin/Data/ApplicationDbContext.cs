using Microsoft.EntityFrameworkCore;
using INTACSAdmin.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Sqlite;

namespace INTACSAdmin.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Username).HasColumnType("TEXT").IsRequired();
                entity.Property(e => e.Email).HasColumnType("TEXT").IsRequired();
                entity.Property(e => e.Password).HasColumnType("TEXT").IsRequired();
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                if (connectionString.Contains(".db"))
                {
                    optionsBuilder.UseSqlite(connectionString);
                }
                else
                {
                    optionsBuilder.UseSqlServer(connectionString);
                }
            }
        }
    }
}
