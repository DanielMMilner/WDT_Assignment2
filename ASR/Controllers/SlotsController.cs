﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASR.Data;
using ASR.Models;
using System.Globalization;

namespace ASR.Controllers
{
    public class SlotsController : Controller
    {
        private readonly AsrContext _context;

        public SlotsController(AsrContext context)
        {
            _context = context;
        }

        // GET: Slots
        public async Task<IActionResult> Index()
        {
            var asrContext = _context.Slot.Include(s => s.Room).Include(s => s.Staff).Include(s => s.Student);
            return View(await asrContext.ToListAsync());
        }

        // GET: Slots/Details/5
        public async Task<IActionResult> Details(string roomId, DateTime startTime)
        {
            if (roomId == null)
            {
                return NotFound();
            }

            var slot = await _context.Slot
                .Include(s => s.Room)
                .Include(s => s.Staff)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.RoomID == roomId && m.StartTime == startTime);
            if (slot == null)
            {
                return NotFound();
            }

            return View(slot);
        }

        // GET: Slots/Create
        public IActionResult Create()
        {
            ViewData["RoomID"] = new SelectList(_context.Room, "RoomID", "RoomID");
            ViewData["StaffID"] = new SelectList(_context.Staff, "StaffID", "StaffID");
            ViewData["StudentID"] = new SelectList(_context.Student, "StudentID", "StudentID");
            return View();
        }

        // POST: Slots/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomID,StartTime,StaffID,StudentID")] Slot slot)
        {
            if (ModelState.IsValid)
            {
                _context.Add(slot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomID"] = new SelectList(_context.Room, "RoomID", "RoomID", slot.RoomID);
            ViewData["StaffID"] = new SelectList(_context.Staff, "StaffID", "StaffID", slot.StaffID);
            ViewData["StudentID"] = new SelectList(_context.Student, "StudentID", "StudentID", slot.StudentID);
            return View(slot);
        }

        // GET: Slots/Edit/5
        public async Task<IActionResult> Edit(string roomID, DateTime startTime)
        {
            if (roomID == null)
            {
                return NotFound();
            }

            var slot = await _context.Slot.FindAsync(roomID, startTime);
            if (slot == null)
            {
                return NotFound();
            }
            ViewData["RoomID"] = new SelectList(_context.Room, "RoomID", "RoomID", slot.RoomID);
            ViewData["StaffID"] = new SelectList(_context.Staff, "StaffID", "StaffID", slot.StaffID);
            ViewData["StudentID"] = new SelectList(_context.Student, "StudentID", "StudentID", slot.StudentID);
            return View(slot);
        }

        // POST: Slots/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string roomID, DateTime startTime, [Bind("RoomID,StartTime,StaffID,StudentID")] Slot slot)
        {
            if (roomID != slot.RoomID)
            {
                return NotFound();
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
                    if (!SlotExists(slot.RoomID, slot.StartTime))
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
            ViewData["StaffID"] = new SelectList(_context.Staff, "StaffID", "StaffID", slot.StaffID);
            ViewData["StudentID"] = new SelectList(_context.Student, "StudentID", "StudentID", slot.StudentID);
            return View(slot);
        }

        // GET: Slots/Delete/5
        public async Task<IActionResult> Delete(string roomId, DateTime startTime)
        {
            if (roomId == null)
            {
                return NotFound();
            }

            var slot = await _context.Slot
                .FirstOrDefaultAsync(m => m.RoomID == roomId && m.StartTime == startTime);
            if (slot == null)
            {
                return NotFound();
            }

            return View(slot);
        }

        // POST: Slots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string roomId, DateTime startTime)
        {            
            if (SlotExists(roomId, startTime))
            {
                var slot = await _context.Slot.FindAsync(roomId, startTime);
                _context.Slot.Remove(slot);
                await _context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("Slot does not exist");
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SlotExists(string roomId, DateTime startTime)
        {
            return _context.Slot.Any(e => e.RoomID == roomId && e.StartTime == startTime);
        }
    }
}