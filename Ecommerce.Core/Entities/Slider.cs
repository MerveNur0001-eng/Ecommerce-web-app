using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities
{
    public class Slider:IEntity
    {
        public int Id { get; set; }

        [MaxLength(250)]

        public string Title { get; set; }
        public string Description { get; set; }

        public string? Image { get; set; }
        public string Link { get; set; }
       
    
    }
}
