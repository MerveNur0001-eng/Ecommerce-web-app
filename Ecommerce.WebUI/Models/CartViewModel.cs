using Ecommerce.Core.Entities;

namespace Ecommerce.WebUI.Models
{
    public class CartViewModel
    {
        public List<CartLine>? CartLines { get; set; }
        public decimal TotalPrice { get; set; }

        public string? CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalPrice { get; set; }
    }
}
