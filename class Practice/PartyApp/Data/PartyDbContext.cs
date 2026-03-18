using Microsoft.EntityFrameworkCore;
using PartyApp.Models;

namespace PartyApp.Data
{
    public class PartyDbContext : DbContext
    {
        public PartyDbContext(DbContextOptions<PartyDbContext> options) : base(options)
        {
        }

        public DbSet<Party> Parties { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Party>().HasData(
                new Party { Id = 1, Name = "External Party 1", IsExternal = true },
                new Party { Id = 2, Name = "Internal Party 1", IsExternal = false },
                new Party { Id = 3, Name = "Internal Party 2", IsExternal = false }
            );
        }
    }
}
