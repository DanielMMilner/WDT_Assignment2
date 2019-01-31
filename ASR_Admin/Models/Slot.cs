using System;
using System.Collections.Generic;

namespace ASR_Admin.Models
{
    public partial class Slot
    {
        public int SlotId { get; set; }
        public string RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public string StaffId { get; set; }
        public string StudentId { get; set; }

        public Room Room { get; set; }
        public AspNetUsers Staff { get; set; }
        public AspNetUsers Student { get; set; }
    }
}
