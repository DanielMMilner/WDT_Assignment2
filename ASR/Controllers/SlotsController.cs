using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASR.Data;
using ASR.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace ASR.Controllers
{
    [Authorize(Roles = Constants.Staff)]
    public class SlotsController : Controller
    {
        private readonly AsrContext _context;
        private readonly UserManager<AppUser> _userManager;

        public SlotsController(AsrContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(DateTime StartDate, DateTime EndDate)
        {
            var dateNotSet = 1;

            if (StartDate.Year == dateNotSet || EndDate.Year == dateNotSet)
            {
                //no filter used.
                var asrContext = _context.Slot.Include(s => s.Room).Include(s => s.Staff).Include(s => s.Student).OrderBy(x => x.StartTime);
                return View(await asrContext.ToListAsync());
            }
            else
            {
                var Slots = _context.Slot.Where(x => x.StartTime.Date >= StartDate.Date && x.StartTime.Date <= EndDate.Date).OrderBy(x => x.StartTime);
                return View(await Slots.ToListAsync());
            }
        }

        // GET: Slots/Details/5
        public async Task<IActionResult> Details(string slotID)
        {
            if (slotID == null)
            {
                return NotFound();
            }

            var slot = await _context.Slot
                .Include(s => s.Room)
                .Include(s => s.Staff)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.SlotID.ToString() == slotID);
            if (slot == null)
            {
                return NotFound();
            }

            //get the name of the staff memeber
            if (slot.StaffID != null)
            {
                ViewData["StaffName"] = _context.AppUser.FirstOrDefault(x => x.Id == slot.StaffID).Name;
            }

            //get the name of the student memeber
            if (slot.StudentID != null)
            {
                ViewData["StudentName"] = _context.AppUser.FirstOrDefault(x => x.Id == slot.StudentID).Name;
            }

            return View(slot);
        }

        public async Task<IActionResult> Schedule()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var slots = _context.Slot.Where(x => x.StaffID == user.Id).OrderBy(x => x.StartTime);

            return View(slots.ToList());
        }

        // GET: Slots/Create
        public IActionResult Create()
        {
            ViewData["RoomName"] = new SelectList(_context.Room, "RoomName", "RoomName");
            ViewData["StaffID"] = new SelectList(_context.AppUser.Where(x => x.SchoolID.StartsWith('e')), "SchoolID", "SchoolID");
            ViewData["StudentID"] = new SelectList(_context.AppUser.Where(x => x.SchoolID.StartsWith('s')), "SchoolID", "SchoolID");
            return View();
        }

        // POST: Slots/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string RoomName, [Bind("RoomID,StartTime,StaffID,StudentID")] Slot slot)
        {
            //find the correct room id from the room name.
            slot.RoomID = _context.Room.FirstOrDefault(x => x.RoomName == RoomName).RoomID;

            //set the slot ID to the correct primary keys.
            slot.StaffID = FindPrimaryKeyFromSchoolID(slot.StaffID);
            if (slot.StudentID != null)
            {
                slot.StudentID = FindPrimaryKeyFromSchoolID(slot.StudentID);
            }

            if (!SlotCreationBusinessRules(slot))
            {
                return Create();
            }

            if (ModelState.IsValid)
            {
                _context.Add(slot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return Create();
        }

        private bool SlotCreationBusinessRules(Slot slot)
        {
            bool pass = true;
            if (slot.StartTime.Minute != 0)
            {
                ModelState.AddModelError(string.Empty, "Bookings can only be made at the top of the hour. i.e 1:00pm, 2:00pm etc.");
                pass = false;
            }

            if (slot.StartTime.Hour < 9 || slot.StartTime.Hour > 14)
            {
                ModelState.AddModelError(string.Empty, "Bookings can only be made during school hours 9am-2pm.");
                pass = false;
            }

            if (slot.StartTime < DateTime.Now)
            {
                ModelState.AddModelError(string.Empty, "Bookings can not be made in the past.");
                pass = false;
            }

            if (_context.Slot.Where(x => x.StaffID == slot.StaffID && x.StartTime.Date == slot.StartTime.Date).Count() >= 4)
            {
                ModelState.AddModelError(string.Empty, "This staff member already has the maximum number of bookings on this day.");
                pass = false;
            }

            if (_context.Slot.Where(x => x.RoomID == slot.RoomID && x.StartTime.Date == slot.StartTime.Date).Count() >= 2)
            {
                ModelState.AddModelError(string.Empty, "This room has reached its maximum number of bookings on this day.");
                pass = false;
            }

            if (_context.Slot.Any(x => x.StaffID == slot.StaffID && x.StartTime == slot.StartTime))
            {
                ModelState.AddModelError(string.Empty, "This staff member already has a slot at this time.");
                pass = false;
            }
            return pass;
        }

        // GET: Slots/Edit/5
        public async Task<IActionResult> Edit(string slotID)
        {
            if (slotID == null)
            {
                return NotFound();
            }

            var slot = await _context.Slot.FindAsync(Int32.Parse(slotID));
            if (slot == null)
            {
                return NotFound();
            }

            ViewData["RoomName"] = new SelectList(_context.Room, "RoomName", "RoomName", slot.Room.RoomName);
            ViewData["StaffID"] = new SelectList(_context.AppUser.Where(x => x.SchoolID.StartsWith('e')), "SchoolID", "SchoolID");
            ViewData["StudentID"] = new SelectList(_context.AppUser.Where(x => x.SchoolID.StartsWith('s')), "SchoolID", "SchoolID");
            ViewData["StartTime"] = new SelectList(_context.Slot, "StartTime", "StartTime", slot.StartTime);
            return View(slot);
        }

        // POST: Slots/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string slotID, string RoomName, [Bind("SlotID,RoomID,StartTime,StaffID,StudentID")] Slot slot)
        {
            if (slotID != slot.SlotID.ToString())
            {
                return NotFound();
            }

            //find the correct room id from the room name.
            slot.RoomID = _context.Room.FirstOrDefault(x => x.RoomName == RoomName).RoomID;

            //set the slot ID to the correct primary keys
            slot.StaffID = FindPrimaryKeyFromSchoolID(slot.StaffID);

            if (slot.StudentID != null)
            {
                slot.StudentID = FindPrimaryKeyFromSchoolID(slot.StudentID);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(slot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SlotExists(slot.SlotID.ToString()))
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

            ViewData["RoomName"] = new SelectList(_context.Room, "RoomName", "RoomName", RoomName);
            ViewData["StaffID"] = new SelectList(_context.AppUser.Where(x => x.SchoolID.StartsWith('e')), "SchoolID", "SchoolID");
            ViewData["StudentID"] = new SelectList(_context.AppUser.Where(x => x.SchoolID.StartsWith('s')), "SchoolID", "SchoolID");
            return View(slot);
        }

        // GET: Slots/Delete/5
        public async Task<IActionResult> Delete(string slotID)
        {
            if (slotID == null)
            {
                return NotFound();
            }

            var slot = await _context.Slot.FirstOrDefaultAsync(m => m.SlotID.ToString() == slotID);
            if (slot == null)
            {
                return NotFound();
            }

            return View(slot);
        }

        // POST: Slots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string slotID)
        {
            if (SlotExists(slotID))
            {
                var slot = await _context.Slot.FindAsync(Int32.Parse(slotID));

                if (slot.StudentID != null)
                {
                    return RedirectToAction(nameof(Index));
                }

                _context.Slot.Remove(slot);
                await _context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("Slot does not exist");
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SlotExists(string slotID)
        {
            return _context.Slot.Any(e => e.SlotID.ToString() == slotID);
        }

        private string FindPrimaryKeyFromSchoolID(string schoolID)
        {
            //Find the primary key of the row which contains the school ID
            var key = _context.AppUser.FirstOrDefault(x => x.SchoolID == schoolID);

            return key.Id;
        }
    }
}
