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
    }
}
