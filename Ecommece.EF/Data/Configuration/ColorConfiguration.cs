using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommece.Core.Models;

namespace Ecommece.EF.Data.Configuration
{
    public class ColorConfiguration : IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.NameEn).HasMaxLength(100).IsRequired();
            builder.Property(c => c.NameAr).HasMaxLength(100).IsRequired(false);
        }
    }

}
