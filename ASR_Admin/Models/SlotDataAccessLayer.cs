using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASR_Admin.Models
{
    public class SlotDataAccessLayer
    {
        private readonly dbContext db = new dbContext();

        public IEnumerable<Slot> GetSlots()
        {
            return db.Slot.ToList();
        }

        public IEnumerable<Slot> GetSlots(string staffID)
        {
            return db.Slot.Where(x => x.StudentId == staffID).ToList();
        }
    }
}
