using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASR.Models
{
    public class Slot
    {
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SlotID { get; set; }

        [Required]
        [Display(Name = "Room ID")]
        public int RoomID { get; set; }
        public virtual Room Room { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [Required]
        public string StaffID { get; set; }
        public virtual AppUser Staff { get; set; }
        
        public string StudentID { get; set; }
        public virtual AppUser Student { get; set; }
    }
}
