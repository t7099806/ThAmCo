using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Data
{
    public class Staffings
    {
        [Key, Column(Order = 0)]
        public int StaffId { get; set; }

        public Staff Staff { get; set; }

        public int EventId { get; set; }

        public Event Event { get; set; }
             

    }
}
