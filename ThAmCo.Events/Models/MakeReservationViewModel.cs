﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Models
{
    public class MakeReservationViewModel
    {
        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }

        [Required, MinLength(5), MaxLength(5)]
        public string VenueCode { get; set; }
    }
}
