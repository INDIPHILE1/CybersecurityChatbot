using Microsoft.EntityFrameworkCore;

namespace POEPART2
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Log> Logs { get; set; }

        public ApplicationDbContext()
        {
            // This will create the database automatically if it doesn't exist
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=database.db");
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}