using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASR.Models
{
    public class AppUser : IdentityUser
    {
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string SchoolID { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
