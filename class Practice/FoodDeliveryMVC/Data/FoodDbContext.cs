using FoodDeliveryMVC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryMVC.Data
{
    public class FoodDbContext : DbContext
    {
        public FoodDbContext(DbContextOptions<FoodDbContext> options)
            : base(options)
        {
        }
        public DbSet<Category>Categories { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<ProductsSold> ProductsSold { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductsSold>().HasKey(ps => new { ps.ProductID, ps.SaleID });
        }
    }
}
