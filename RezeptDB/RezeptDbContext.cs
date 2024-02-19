using Microsoft.EntityFrameworkCore;
using Rezeptmanager.Entities;

namespace Rezeptmanager.RezeptDB
{
    public class RezeptDbContext : DbContext
    {
        public RezeptDbContext(DbContextOptions<RezeptDbContext> options) : base(options)
        {
        }

        public DbSet<Rezept> Rezepte { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
