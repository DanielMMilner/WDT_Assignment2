using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASR.Models
{
    public class Staff
    {
        [Required, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string StaffID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
