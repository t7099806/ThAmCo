﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.Data
{
    public class Event
    {
       // public bool isActive { get; set; }

        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan? Duration { get; set; }

        [Required, MaxLength(3), MinLength(3)]
        public string TypeId { get; set; }

        [MinLength(5), MaxLength(5)]
        public string VenueCode { get; set; }

        public List<GuestBooking> Bookings { get; set; }

        public decimal VenueCost { get; set; }

        public string Venue { get; set; }

        


    }
}
