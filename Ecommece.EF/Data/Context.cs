using Ecommece.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ecommece.EF.Data
{
    public class Context:DbContext
    {
        public Context() 
        {
        }
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }

        public DbSet<AppUser> Users { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Order>().OwnsOne(o => o.ShippingAddress, a => { a.WithOwner(); });
           

        }
    }
}
