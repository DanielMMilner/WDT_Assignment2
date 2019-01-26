﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASR.Data;
using ASR.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ASR
{
    public class BookingController : Controller
    {
        private readonly AsrContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BookingController(AsrContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: Booking
        public async Task<IActionResult> Index(string date)
        {
            // If no date has been provided populate it
            if (date == null)
            {
                return Redirect($"booking?date={DateTime.Now.ToString("yyyy-MM-dd")}");
            }
            var dateTime = DateTime.Parse(date);
            var model = new BookingViewModel
            {
                Date = dateTime,
                BookingsForDate = _context.Slot.Include(s => s.Room).Include(s => s.Staff).Include(s => s.Student)
                .Where(x => x.StartTime.Date == dateTime.Date),
                MyBookings = _context.Slot.Include(s => s.Room).Include(s => s.Staff).Include(s => s.Student)
                .Where(x => x.StudentID == _userManager.GetUserId(User))
            };
            
            return View(model);
        }

        // GET: Booking/MakeBooking
        public async Task<IActionResult> MakeBooking()
        {
            var asrContext = _context.Slot.Include(s => s.Room).Include(s => s.Staff).Include(s => s.Student)
                .Where(x => x.StudentID == _userManager.GetUserId(User));
            return View(await asrContext.ToListAsync());
        }

        // GET: Booking/Book/5
        public async Task<IActionResult> Book(string slotID)
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

        // POST: Booking/Book/5
        [HttpPost, ActionName("Book")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookConfirmed(string slotID)
        {
            if (SlotExists(Int32.Parse(slotID)))
            {
                var slot = await _context.Slot.FindAsync(Int32.Parse(slotID));

                slot.StudentID = _userManager.GetUserId(User);

                _context.Update(slot);

                await _context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("Slot does not exist");
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Booking/Cancel/5
        public async Task<IActionResult> Cancel(string slotID)
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

        // POST: Booking/Cancel/5
        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelConfirmed(string slotID)
        {
            if (SlotExists(int.Parse(slotID)))
            {
                var slot = await _context.Slot.FindAsync(Int32.Parse(slotID));
                // Remove the student ID
                slot.StudentID = null;
                _context.Update(slot);
                await _context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("Slot does not exist");
            }

            return RedirectToAction(nameof(Index));
        }



        private bool SlotExists(int id)
        {
            return _context.Slot.Any(e => e.SlotID == id);
        }
    }
}
