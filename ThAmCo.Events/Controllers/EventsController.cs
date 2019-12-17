using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Events.Data;
using ThAmCo.Events.Models;


namespace ThAmCo.Events.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventsDbContext _context;

        public EventsController(EventsDbContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            return View(await _context.Events.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                                        .Select(e => new EventDetailsViewModel
                                        {
                                            EventId = e.Id,
                                            Title = e.Title,
                                            Date = e.Date,
                                            Duration = e.Duration,
                                            TypeId = e.TypeId,
                                            Venue = e.Venue,
                                            VenueCost = e.VenueCost.ToString("C2"),

                                            Guest = _context.Guests
                                                            .Where(g => g.EventId == e.Id)
                                                            .Select(g => new EventGuestViewModel
                                                            {
                                                                GuestId = g.Customer.Id,
                                                                FirstName = g.Customer.FirstName,
                                                                LastName = g.Customer.Surname,
                                                                Attended = g.Attended
                                                            })

                                        })
                                        .FirstOrDefaultAsync(i => i.EventId == id);

            //var @event = await _context.Events
            //    .Include(g => g.Bookings)
            //    .ThenInclude( c => c.Customer)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Date,Duration,TypeId")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            var edit = new EventsEditViewModel() { Id = @event.Id, Title = @event.Title, Duration = @event.Duration };
            return View(edit);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Duration")] EventsEditViewModel @event)
        {
            var @eventVM = await _context.Events.FindAsync(id);
            if (@eventVM == null)
            {
                return NotFound();
            }

            if (id != @event.Id)
            {
                return NotFound();
            }

            @eventVM.Duration = @event.Duration;
            @eventVM.Title = @event.Title;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@eventVM);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(@eventVM);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AvailableVenue(int? eventId)
        {
            if(eventId == null)
            {
                return NotFound();
            }

            var thisEvent = await _context.Events.FindAsync(eventId);

            String eventType = thisEvent.TypeId;
            DateTime beginDate = thisEvent.Date;
            DateTime endDate = thisEvent.Date.Add(thisEvent.Duration.Value);

            var availableVenue = new List<AvailableVenueModel>().AsEnumerable();

            HttpClient client = new HttpClient();
            client.BaseAddress = new System.Uri("http://localhost:22263/");

            

            HttpResponseMessage response = await client.GetAsync("api/Availability?eventType=" + eventType
                + "&beginDate=" + beginDate.ToString("yyyy-MM-dd") +
                "&endDate=" + endDate.ToString("yyyy-MM-dd"));

            if (response.IsSuccessStatusCode)
            {
                availableVenue = await response.Content.ReadAsAsync<IEnumerable<AvailableVenueModel>>();

                if (availableVenue.Count() == 0)
                {
                    ViewBag.NullVenues = true;
                    Debug.WriteLine("No venues available");
                }
            }
            else
            {
                Debug.WriteLine("bad response ");
                return BadRequest();
            }

            ViewData["EventTitle"] = thisEvent.Title;
            ViewData["EventId"] = thisEvent.Id;
            ViewData["EventDate"] = thisEvent.Date;

            return View(availableVenue);
        }

        public async Task<IActionResult> MakeReservation(int? eventId, DateTime EventDate, string VenueCode)
        {
            if(eventId == null || VenueCode == null)
            {
                return NotFound();
            }

            var thisEvent = await _context.Events.FindAsync(eventId);

            HttpClient client = new HttpClient();
            client.BaseAddress = new System.Uri("http://localhost:22263/");

            var reservation = new MakeReservationViewModel
            {
                EventDate = EventDate,
                VenueCode = VenueCode
            };

            HttpResponseMessage response = await client.PostAsJsonAsync("api/reservations/", reservation);

            if(response.IsSuccessStatusCode)
            {
                if(!String.IsNullOrEmpty(thisEvent.VenueCode))
                {
                    var reference = thisEvent.VenueCode + EventDate.ToString("yyyy/MM/dd");
                    await client.DeleteAsync("api/reservations/" + reference);


                }

                thisEvent.VenueCode = reservation.VenueCode;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest();
            }
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }

       

    }
}
