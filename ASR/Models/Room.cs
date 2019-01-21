using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASR.Models
{
    public class Room
    {
        [Required, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string RoomID { get; set; }

        //public virtual ICollection<Slot> Slots { get; set; }
    }
}
