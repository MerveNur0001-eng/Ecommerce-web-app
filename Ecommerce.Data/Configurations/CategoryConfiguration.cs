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
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(x => x.Name)
                 .IsRequired()
                 .HasMaxLength(50);
             builder.Property(x => x.Image)
                 .HasMaxLength(50);
            builder.HasData(
                new Category
                {
                    Name = "Electronic",
                    Id = 1,
                    IsActive = true,
                    IsTopMenu = true,
                    ParentId = null,
                    OrderNo = 1,

                },
                new Category
                {
                    Name = "Computer",
                    Id = 2,
                    IsActive = true,
                    IsTopMenu = true,
                    ParentId = null,
                    OrderNo = 2,
                });

        }
    }
}
