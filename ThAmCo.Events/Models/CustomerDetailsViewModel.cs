using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ThAmCo.Events.Models
{
    public class CustomerDetailsViewModel
    {
        public int CustomerID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string SurName { get; set; }

        public string Email { get; set; }

        public IEnumerable<EventsViewModel> Events {get; set;}

    }
}
