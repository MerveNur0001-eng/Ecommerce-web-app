using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities
{
    public class OrderLine : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Order Id")]
        public int OrderId { get; set; }

        [Display(Name = "Order")]
        public Order? Order { get; set; }

        [Display(Name = "Product Id")]
        public int ProductId { get; set; }

        [Display(Name = "Product")]
        public Product? Product { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }
    }
}
