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
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(oi => oi.Id);

            builder.Property(oi => oi.Price)
                   .HasColumnType("decimal(18,2)");

            builder.Property(oi => oi.ProductName)
                   .HasMaxLength(200);

            builder.Property(oi => oi.PictureUrl)
                   .HasMaxLength(1000);

            // Optional: index on ProductVariantId for performance
            builder.HasIndex(oi => oi.ProductVariantId);
        }
    }
}
