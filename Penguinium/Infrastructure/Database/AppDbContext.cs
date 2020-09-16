using Microsoft.EntityFrameworkCore;
using Penguinium.Entities;


namespace Penguinium.Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Test> Tests { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        // TODO:これ、ホスティングするからいらないっぽい
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;Integrated Security=true;Database=PenguinSteamer";  // local.settings.jsonとかと同じにすること
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
