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
    public class ProductPictureConfiguration : IEntityTypeConfiguration<ProductPicture>
    {
        public void Configure(EntityTypeBuilder<ProductPicture> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.PictureUrl).HasMaxLength(1000).IsRequired();
        }
    }

}
