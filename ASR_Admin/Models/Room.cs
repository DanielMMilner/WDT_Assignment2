using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASR_Admin.Models
{
    public partial class Room
    {
        public Room()
        {
            Slot = new HashSet<Slot>();
        }

        public int RoomId { get; set; }
        public string RoomName { get; set; }

        public ICollection<Slot> Slot { get; set; }
    }
}
