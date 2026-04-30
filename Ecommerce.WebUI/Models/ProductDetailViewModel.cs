using Ecommerce.Core.Entities;

namespace Ecommerce.WebUI.Models
{
    public class ProductDetailViewModel
    {
        public Product? Product { get; set; }
        public IEnumerable<Product>? RelatedProducts { get; set; }

    }
}
