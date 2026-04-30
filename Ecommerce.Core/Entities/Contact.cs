using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities
{
    public class Contact:IEntity
    {
        public int Id { get; set; }
        [Display(Name = "Name"), Required(ErrorMessage = "{0} field cannot be left blank!")]
        public string Name { get; set; }

        [Display(Name = "Surname"), Required(ErrorMessage = "{0} field cannot be left blank!")]
        public string Surname { get; set; }

        public string? Email { get; set; }

        [Display(Name = "Phone")]
        public string? Phone { get; set; }

        [Display(Name = "Message"), Required(ErrorMessage = "{0} field cannot be left blank!")]
        public string Message { get; set; }

        [Display(Name = "Registration Date"), ScaffoldColumn(false)]
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
