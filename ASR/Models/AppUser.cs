using Microsoft.AspNetCore.Identity;

namespace ASR.Models
{
    public class AppUser : IdentityUser
    {
        public string StaffID { get; set; }
        public virtual Staff Staff { get; set; }

        public string StudentID { get; set; }
        public virtual Student Student { get; set; }
    }
}
