using Ecommerce.Core.Entities;

namespace Ecommerce.WebUI.Models
{
    public class CheckoutViewModel
    {
        public List<CartLine> CartProducts { get; set; }
        public decimal TotalPrice { get; set; }

        public string? CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalPrice { get; set; }
        public List<Address>? Addresses { get; set; }
    }
}
