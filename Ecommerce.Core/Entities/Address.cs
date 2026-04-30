using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities
{
    public class Address : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Address Title"), StringLength(50), Required(ErrorMessage = "{0} field is required!")]
        public string Title { get; set; }

        [Display(Name = "City"), StringLength(50), Required(ErrorMessage = "{0} field is required!")]
        public string City { get; set; }

        [Display(Name = "District"), StringLength(50), Required(ErrorMessage = "{0} field is required!")]
        public string District { get; set; }

        [Display(Name = "Full Address"), DataType(DataType.MultilineText), Required(ErrorMessage = "{0} field is required!")]
        public string OpenAddress { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Billing Address")]
        public bool IsBillingAddress { get; set; }

        [Display(Name = "Delivery Address")]
        public bool IsDeliveryAddress { get; set; }

        [Display(Name = "Creation Date"), ScaffoldColumn(false)]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [ScaffoldColumn(false)]
        public Guid? AddressGuid { get; set; } = Guid.NewGuid();

        public int? AppUserId { get; set; }

        public AppUser? AppUser { get; set; }
    }
}
