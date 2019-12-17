using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Models
{
    public class EventDetailsViewModel
    {

        public int EventId { get; set; }
        public string Title { get; set; }

        public DateTime Date { get; set; }
        public TimeSpan? Duration { get; set; }

        [Required, MaxLength(3), MinLength(3)]
        public string TypeId { get; set; }


        public string Venue { get; set; }

        public string VenueCost { get; set; }

        public IEnumerable<EventGuestViewModel> Guest { get; set; }
    }
}
