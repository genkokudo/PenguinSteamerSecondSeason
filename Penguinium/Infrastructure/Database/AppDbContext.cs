using Microsoft.EntityFrameworkCore;
using Penguinium.Entities;

namespace Penguinium.Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<TestEntity> Tests { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }
    }
}
