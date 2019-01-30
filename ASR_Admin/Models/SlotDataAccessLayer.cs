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
                RoomName = x.RoomId,
                startTime = x.StartTime,
                StaffId = db.AspNetUsers.FirstOrDefault(u => u.Id == x.StaffId).SchoolId,
                StudentId = db.AspNetUsers.FirstOrDefault(u => u.Id == x.StudentId).SchoolId
            }).ToList();
        }
        
    }
}
