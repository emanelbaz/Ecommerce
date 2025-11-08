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
    public class SizeConfiguration : IEntityTypeConfiguration<Size>
    {
        public void Configure(EntityTypeBuilder<Size> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Name).HasMaxLength(50).IsRequired();
            builder.HasIndex(s => s.Name).IsUnique();
        }
    }

}
