using Microsoft.EntityFrameworkCore;
using Penguinium.Infrastructure.Database;

namespace BlizzardHaunt.Infrastructure.Database
{
    public class ApplicationDbContext : AppDbContext
    {
        public ApplicationDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }
    }
}
