using System;

namespace Ecommerce.Core.Entities
{
    public class Coupon : IEntity
    {
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;
         
        public int DiscountPercent { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int? UsageLimit { get; set; }
        public int UsedCount { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}