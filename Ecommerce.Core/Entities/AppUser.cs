using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Ecommerce.Core.Entities
{
    public class AppUser : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        public string Name { get; set; }

        [Display(Name = "Last Name")]
        public string Surname { get; set; }

        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string? Phone { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Username")]
        public string? UserName { get; set; }

        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; }

        [Display(Name = "Is Admin?")]
        public bool IsAdmin { get; set; }

        [Display(Name = "Registration Date"), ScaffoldColumn(false)]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [ScaffoldColumn(false)]
        public Guid? UserGuid { get; set; } = Guid.NewGuid();

        public List<Address>? Addresses { get; set; } = new();
    }
}