using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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


        public IActionResult Create([FromQuery] int? eventId)
        {
            if(eventId == null)
            {
                return BadRequest();
            }
            var staff = _context.Staffs.ToList();
            var unavailableStaff = _context.Staffings
                                            .Where(s => s.EventId == eventId)
                                            .ToList();
            staff.RemoveAll(s => unavailableStaff.Any(g => g.StaffId == s.StaffId));

            ViewData["StaffId"] = new SelectList(staff, "Id", "FirstName");
               
            var list = _context.Events.Find(eventId);
            

            if (list == null)
            {
                return BadRequest();
            }

            ViewData["EventTitle"] = list.Title;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StaffId,EventId")] Staffing staffing)
        {
            if(ModelState.IsValid)
            {
                if(_context.Staffings.Any(s => s.StaffId == staffing.StaffId && s.EventId == staffing.EventId))
                {
                    ModelState.AddModelError(string.Empty, "Already staff member");
                }
                else
                {
                    _context.Add(staffing);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Events");
                }
            }

            var eventI = await _context.Events.FindAsync(staffing.EventId);
            if(eventI == null)
            {
                return BadRequest();
            }


            return View(staffing);
        }

        public async Task<IActionResult> Delete(int? staffId, int? eventId)
        {
            if(staffId == null || eventId == null)
            {
                return BadRequest();
            }

            var staffBooking = await _context.Staffings
                                .Include(e => e.Event)
                                .Include(s => s.Staff)
                                .FirstOrDefaultAsync(s => s.EventId == eventId && s.StaffId == staffId);

            if(staffBooking == null)
            {
                return NotFound();
            }

            var eventBooking = _context.Events.Find(eventId);
            if(eventBooking == null)
            {
                return BadRequest();
            }

            ViewData["EventTitle"] = eventBooking.Title;
            

                    
              
            return View(staffBooking);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("StaffId,EventId")] Staffing staffing)
        {
            var staffinDel = await _context.Staffings
                                            .Where(e => e.EventId == staffing.EventId)
                                            .Where(s => s.StaffId == staffing.StaffId)
                                            .FirstOrDefaultAsync();
            _context.Staffings.Remove(staffinDel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Events", new { id = staffing.EventId });
           
        }
        public IActionResult Index()
        {
            return View();
        }


    }
}