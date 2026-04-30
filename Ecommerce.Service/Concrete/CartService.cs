using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Core.Entities;
using Ecommerce.Service.Abstract;

namespace Ecommerce.Service.Concrete
{
    public class CartService : ICartService
    {
        public List<CartLine> CartLines { get; set; } = new();
       public decimal DiscountPercent { get; set; } = 0;
        public string? CouponCode { get; set; }
        public void AddProduct(Product product, int quantity)
        {
            var _product = CartLines.FirstOrDefault(p => p.Product.Id == product.Id);    
            if (_product != null)
            {
                _product.Quantity += quantity;
            }
            else
            {
                CartLines.Add(new CartLine { Product = product, Quantity = quantity });
            }
        }

        public void ClearAll()
        {
            CartLines.Clear();
        }

        public void RemoveProduct(Product product)
        {
            CartLines.RemoveAll(p => p.Product.Id == product.Id);
        }

        public decimal TotalPrice()
        {
            return CartLines.Sum(c=>c.Product.Price * c.Quantity);
        }

        public void UpdateProduct(Product product, int quantity)
        {
            var _product = CartLines.FirstOrDefault(p => p.Product.Id ==  product.Id);
            if (_product != null)
            {
                _product.Quantity = quantity;
            }
            else
            {
                CartLines.Add(new CartLine { Product = product, Quantity = quantity });
            }
        }
        public void ApplyCoupon(string code, decimal discount)
        {
            CouponCode = code;
            DiscountPercent = discount; // %10, %20 gibi
        }
        public void ClearCoupon()
        {
            CouponCode = null;
            DiscountPercent = 0;
        }
        public decimal FinalPrice()
        {
            var total = TotalPrice();
            var discountAmount = total * DiscountPercent / 100;

            var result = total - discountAmount;

            return result < 0 ? 0 : result;
        }
        public decimal DiscountAmount()
        {
            var total = TotalPrice();
            return total * DiscountPercent / 100;
        }
    }
}
