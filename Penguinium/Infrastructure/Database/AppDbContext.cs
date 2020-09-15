using Microsoft.EntityFrameworkCore;
using Penguinium.Entities;

namespace Penguinium.Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        public DbSet<Test> Tests { get; set; }
    }
}
