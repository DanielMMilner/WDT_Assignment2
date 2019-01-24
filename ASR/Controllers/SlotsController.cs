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

        // GET: Slots
        public async Task<IActionResult> Index()
        {
            var asrContext = _context.Slot.Include(s => s.Room).Include(s => s.Staff).Include(s => s.Student);
            return View(await asrContext.ToListAsync());
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

            return View(slot);
        }

        public async Task<IActionResult> Schedule()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var slots = _context.Slot.Where(x => x.StaffID == user.Id);

            return View(slots.ToList());
        }

        // GET: Slots/Create
        public IActionResult Create()
        {
            ViewData["RoomID"] = new SelectList(_context.Room, "RoomID", "RoomID");
            ViewData["StaffID"] = new SelectList(_context.AppUser.Where(x => x.SchoolID.StartsWith('e')), "SchoolID", "SchoolID");
            ViewData["StudentID"] = new SelectList(_context.AppUser.Where(x => x.SchoolID.StartsWith('s')), "SchoolID", "SchoolID");
            return View();
        }

        // POST: Slots/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomID,StartTime,StaffID,StudentID")] Slot slot)
        {
            //set the slot ID to the correct primary keys
            slot.StaffID = FindPrimaryKeyFromSchoolID(slot, slot.StaffID);
            slot.StudentID = FindPrimaryKeyFromSchoolID(slot, slot.StudentID);

            if (ModelState.IsValid)
            {
                _context.Add(slot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomID"] = new SelectList(_context.Room, "RoomID", "RoomID", slot.RoomID);
            ViewData["StaffID"] = new SelectList(_context.AppUser, "StaffID", "StaffID", slot.StaffID);
            ViewData["StudentID"] = new SelectList(_context.AppUser, "StudentID", "StudentID", slot.StudentID);

            return View(slot);
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
            ViewData["RoomID"] = new SelectList(_context.Room, "RoomID", "RoomID", slot.RoomID);
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
        public async Task<IActionResult> Edit(string slotID, [Bind("SlotID,RoomID,StartTime,StaffID,StudentID")] Slot slot)
        {
            if (slotID != slot.SlotID.ToString())
            {
                return NotFound();
            }

            //set the slot ID to the correct primary keys
            slot.StaffID = FindPrimaryKeyFromSchoolID(slot, slot.StaffID);
            slot.StudentID = FindPrimaryKeyFromSchoolID(slot, slot.StudentID);

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
            ViewData["RoomID"] = new SelectList(_context.Room, "RoomID", "RoomID", slot.RoomID);
            ViewData["StaffID"] = new SelectList(_context.Slot, "StaffID", "StaffID", slot.StaffID);
            ViewData["StudentID"] = new SelectList(_context.Slot, "StudentID", "StudentID", slot.StudentID);
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

        private string FindPrimaryKeyFromSchoolID(Slot slot, string schoolID)
        {
            //Find the primary key of the row which contains the school ID
            var key = _context.AppUser.FirstOrDefault(x => x.SchoolID == schoolID);

            return key.Id;
        }
    }
}
