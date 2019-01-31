using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASR.Models
{
    public class Room
    {
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string RoomID { get; set; }

        [Required]
        public string RoomName { get; set; }
    }
}
