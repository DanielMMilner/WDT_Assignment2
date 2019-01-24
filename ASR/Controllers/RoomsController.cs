using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ASR.Data;
using Microsoft.AspNetCore.Authorization;

namespace ASR.Controllers
{
    [Authorize(Roles = Constants.Staff)]
    public class RoomsController : Controller
    {
        private readonly AsrContext _context;

        public RoomsController(AsrContext context)
        {
            _context = context;
        }

        // GET: Rooms
        public IActionResult Index()
        {
            ViewData["RoomID"] = new SelectList(_context.Room, "RoomID", "RoomID");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Availability(string RoomID, DateTime Date)
        {
            if (RoomID == null)
            {
                return NotFound();
            }

            var slot = _context.Slot.Where(x => x.RoomID == RoomID && x.StartTime.DayOfYear == Date.DayOfYear).ToList();
            if (slot == null)
            {
                return NotFound();
            }

            ViewData["Date"] = String.Format("{0:dddd, MMMM d, yyyy}", Date);
            return View(slot);
        }
    }
}
