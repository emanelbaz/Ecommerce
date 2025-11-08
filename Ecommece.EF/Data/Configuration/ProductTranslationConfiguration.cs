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
    public class ProductTranslationConfiguration : IEntityTypeConfiguration<ProductTranslation>
    {
        public void Configure(EntityTypeBuilder<ProductTranslation> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Language).HasMaxLength(5).IsRequired();
            builder.Property(t => t.Name).HasMaxLength(500).IsRequired();
            builder.Property(t => t.Description).HasColumnType("nvarchar(max)").IsRequired(false);

            // Unique index to prevent dup translations for same product/lang
            builder.HasIndex(t => new { t.ProductId, t.Language }).IsUnique();
        }
    }

}
