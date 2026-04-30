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
    internal class SliderConfiguration:IEntityTypeConfiguration<Slider>
    {
        public void Configure(EntityTypeBuilder<Slider> builder)
        {
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(250);
            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(750);
            builder.Property(x => x.Image)
                .HasMaxLength(100);
           builder.Property(x => x.Link)
                .IsRequired()
                .HasMaxLength(100);
        }
}
}
