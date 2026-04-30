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
    internal class ProductConfiguration: IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Name)
                          .IsRequired()
                          .HasMaxLength(100);
            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(750);
            builder.Property(x => x.Price)
            .HasPrecision(18, 2);
            builder.Property(x => x.Image)
                   .HasMaxLength(100);
            builder.Property(x => x.ProductCode)
                 .IsRequired()
                 .HasMaxLength(50);


        }
}
}
