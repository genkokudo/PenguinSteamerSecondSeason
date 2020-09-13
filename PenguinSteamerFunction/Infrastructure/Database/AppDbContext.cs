using Microsoft.EntityFrameworkCore;
using PenguinSteamerFunction.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PenguinSteamerFunction.Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        public DbSet<Test> Tests { get; set; }
    }
}
