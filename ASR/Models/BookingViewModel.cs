using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASR.Models
{
    public class BookingViewModel
    {
        public IEnumerable<Slot> MyBookings;
        public IEnumerable<Slot> BookingsForDate;
        public DateTime Date;
    }
}
