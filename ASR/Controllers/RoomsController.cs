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
        public IActionResult Availability(string RoomID, DateTime StartDate, DateTime EndDate)
        {
            if (RoomID == null)
            {
                return NotFound();
            }

            var slot = _context.Slot.Where(x => x.RoomID == RoomID && x.StartTime.Date >= StartDate.Date && x.StartTime.Date <= EndDate.Date).ToList();
            if (slot == null)
            {
                return NotFound();
            }

            ViewData["StartDate"] = String.Format("{0:dddd, MMMM d, yyyy}", StartDate);
            ViewData["EndDate"] = String.Format("{0:dddd, MMMM d, yyyy}", EndDate);

            return View(slot);
        }
    }
}
