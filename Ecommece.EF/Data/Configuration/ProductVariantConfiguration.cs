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
    public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
    {
        public void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            builder.HasKey(v => v.Id);

            builder.Property(v => v.Price).HasColumnType("decimal(18,2)").IsRequired(false);
            builder.Property(v => v.SKU).HasMaxLength(100).IsRequired(false);

            builder.HasOne(v => v.Product)
                   .WithMany(p => p.Variants)
                   .HasForeignKey(v => v.ProductId);

            builder.HasOne(v => v.Color)
                   .WithMany(c => c.Variants)
                   .HasForeignKey(v => v.ColorId);

            builder.HasOne(v => v.Size)
                   .WithMany(s => s.Variants)
                   .HasForeignKey(v => v.SizeId);

            // index to ensure uniqueness per product-color-size
            builder.HasIndex(v => new { v.ProductId, v.ColorId, v.SizeId }).IsUnique();
        }
    }

}
