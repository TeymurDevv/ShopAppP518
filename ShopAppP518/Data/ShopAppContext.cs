using Microsoft.EntityFrameworkCore;
using ShopAppP518.Data.Configurations;
using ShopAppP518.Entities;
using System.Reflection;

namespace ShopAppP518.Data
{
    public class ShopAppContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public ShopAppContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
