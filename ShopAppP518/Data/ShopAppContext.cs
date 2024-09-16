using Microsoft.EntityFrameworkCore;
using ShopAppP518.Entities;

namespace ShopAppP518.Data
{
    public class ShopAppContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public ShopAppContext(DbContextOptions options) : base(options)
        {
        }
    }
}
