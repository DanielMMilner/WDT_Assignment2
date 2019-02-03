using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ASR.Data;
using Microsoft.AspNetCore.Authorization;

namespace ASR.Controllers
{
    [Authorize(Roles = Constants.Student)]
    public class StaffController : Controller
    {
        private readonly AsrContext _context;

        public StaffController(AsrContext context)
        {
            _context = context;
        }

        // GET: Rooms
        public IActionResult Index()
        {
            ViewData["SchoolID"] = new SelectList(_context.AppUser.Where(x => x.SchoolID.StartsWith('e')), "SchoolID", "SchoolID");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Availability(string SchoolID, DateTime StartDate, DateTime EndDate)
        {
            var StaffID = FindPrimaryKeyFromSchoolID(SchoolID);

            var slot = _context.Slot.Where(x => x.StaffID == StaffID && x.StartTime.Date >= StartDate.Date && x.StartTime.Date <= EndDate.Date && x.StudentID == null).OrderBy(x=>x.StartTime).ToList();
            if (slot == null)
            {
                return NotFound();
            }

            ViewData["StartDate"] = String.Format("{0:dddd, MMMM d, yyyy}", StartDate);
            ViewData["EndDate"] = String.Format("{0:dddd, MMMM d, yyyy}", EndDate);

            return View(slot);
        }


        private string FindPrimaryKeyFromSchoolID(string schoolID)
        {
            //Find the primary key of the row which contains the school ID
            var key = _context.AppUser.FirstOrDefault(x => x.SchoolID == schoolID);

            return key.Id;
        }
    }
}
