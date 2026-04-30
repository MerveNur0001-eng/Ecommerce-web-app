using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities
{
    public class ProductImage : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Image Name"), StringLength(240)]
        public string? Name { get; set; }

        [Display(Name = "Image Description"), StringLength(240)]
        public string? Description { get; set; }

        [Display(Name = "Product")]
        public int? ProductId { get; set; }

        public Product? Product { get; set; }

    }
}
