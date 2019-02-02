﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASR_Admin.Models
{
    public class SlotDataAccessLayer
    {
        private readonly dbContext db = new dbContext();

        public IEnumerable<ApiSlotModel> GetSlots()
        {
            return db.Slot.Select(x => new ApiSlotModel
            {
                SlotId = x.SlotId,
                RoomName = x.Room.RoomName,
                startTime = x.StartTime,
                StaffId = db.AspNetUsers.FirstOrDefault(u => u.Id == x.StaffId).SchoolId,
                StudentId = db.AspNetUsers.FirstOrDefault(u => u.Id == x.StudentId).SchoolId
            }).ToList();
        }

        public IEnumerable<ApiSlotModel> GetSlotsForId(string id)
        {
            return GetSlots().Where(x => x.StaffId == id || x.StudentId == id).ToList();
        }

        public ApiSlotModel GetSlot(int id)
        {
            return GetSlots().FirstOrDefault(x => x.SlotId == id);
        }

        public void UpdateBooking(int id, string studentId)
        {
            var slotToUpdate = db.Slot.FirstOrDefault(x => x.SlotId == id);
            slotToUpdate.StudentId = db.AspNetUsers.FirstOrDefault(x => x.SchoolId == studentId).Id;
            
            db.Slot.Update(slotToUpdate);
            db.SaveChanges();
        }

        public void CancelBooking(int id)
        {
            var slotToUpdate = db.Slot.FirstOrDefault(x => x.SlotId == id);
            slotToUpdate.StudentId = null;

            db.Slot.Update(slotToUpdate);
            db.SaveChanges();
        }

        public void DeleteSlot(int id)
        {
            var slot = db.Slot.FirstOrDefault(x => x.SlotId == id);

            db.Slot.Remove(slot);
            
            db.SaveChanges();
        }

    
    }
}