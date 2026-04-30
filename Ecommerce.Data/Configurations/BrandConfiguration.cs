using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Data.Configurations
{
    internal class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.Property(x => x.Name)
                  .IsRequired()
                  .HasMaxLength(50);

            builder.Property(x => x.Description)
                  .IsRequired();

            builder.Property(x => x.Logo)
                  .HasColumnType("nvarchar(500)")
                  .HasMaxLength(500);

        }
    }
}
