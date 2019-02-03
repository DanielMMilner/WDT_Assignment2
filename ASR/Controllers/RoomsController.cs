using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ASR.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

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
            ViewData["RoomName"] = new SelectList(_context.Room, "RoomName", "RoomName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Availability(DateTime date)
        {
            // Get all room ids used on the day
            var roomIdsOnDate = _context.Slot.Where(x => x.StartTime.Date == date.Date).Select(x => x.RoomID).ToList();

            var excludeRoomIds = roomIdsOnDate.GroupBy(x => x).Where(g => g.Count() >= 2).Select(x => x.Key);

            //Return the rooms which do not have the max number of bookings.
            var rooms = _context.Room.Where(x => !excludeRoomIds.Contains(x.RoomID));

            ViewData["Date"] = string.Format("{0:dddd, MMMM d, yyyy}", date);

            return View(rooms);
        }
    }
}
