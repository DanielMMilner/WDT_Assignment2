using Microsoft.EntityFrameworkCore;
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

        public ApiSlotModel GetSlot(int id)
        {
            return GetSlots().FirstOrDefault(x => x.SlotId == id);
        }
        
    }
}
