using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Controllers
{
    public class StaffingsController : Controller
    {

        private readonly EventsDbContext _context;

        public StaffingsController(EventsDbContext context)
        {
            _context = context;
        }



        public IActionResult Index()
        {
            return View();
        }


    }
}