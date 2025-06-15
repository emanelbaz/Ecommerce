using Ecommerce.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.EF.Data
{
    public class Context:DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
    }
}
