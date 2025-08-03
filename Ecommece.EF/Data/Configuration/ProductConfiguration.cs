using Ecommece.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.EF.Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        /// <summary>
        /// Configures the properties and relationships of the <see cref="Product"/> entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the <see cref="Product"/> entity.</param>
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
                .Property(p => p.Price)
                .HasColumnType("decimal(18,3)");

            builder
                .HasOne(p => p.ProductBrand)
                .WithMany()
                .HasForeignKey(p => p.ProductBrandId);

            builder
                .HasOne(p => p.ProductType)
                .WithMany()
                .HasForeignKey(p => p.ProductTypeId);
        }
    }
}
