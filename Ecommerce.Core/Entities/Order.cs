using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities
{
    public class Order : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Order No"), StringLength(50)]
        public string OrderNumber { get; set; }

        [Display(Name = "Order Total")]
        public decimal TotalPrice { get; set; }

        public decimal DiscountAmount { get; set; }
        public decimal FinalPrice { get; set; }

        public int? CouponId { get; set; }
        public Coupon? Coupon { get; set; }

        [Display(Name = "User Id")]
        public int AppUserId { get; set; }

        [Display(Name = "Customer"), StringLength(50)]
        public string CustomerId { get; set; }

        [Display(Name = "Billing Address"), StringLength(300)]
        public string BillingAddress { get; set; }

        [Display(Name = "Delivery Address"), StringLength(300)]
        public string DeliveryAddress { get; set; }

        
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }
        public List<OrderLine>? OrderLines { get; set; }
       
        [Display(Name = "Customer")]

        public AppUser? AppUser { get; set; }
       
        [Display(Name = "Order Status")]

        public EnumOrderState OrderState { get; set; }
    }
    public enum EnumOrderState
    {
        [Display(Name = "Pending Approval")]
        Waiting,

        [Display(Name = "Approved")]
        Approved,

        [Display(Name = "Shipped")]
        Shipped,

        [Display(Name = "Completed")]
        Completed,

        [Display(Name = "Cancelled")]
        Cancelled,

        [Display(Name = "Returned")]
        Returned
    }
}
