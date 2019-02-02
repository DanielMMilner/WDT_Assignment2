using ASR.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASR.Areas.Identity.Pages.Account
{
    public class SuccessModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        public string Name { get; set; }
        public string Email { get; set; }
        public string SchoolID { get; set; }
        public string Role { get; set; }

        public SuccessModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public void OnGetSuccess(string name, string email, string schoolID, bool staffRole)
        {
            Name = name;
            Email = email;
            SchoolID = schoolID;
            if (staffRole)
            {
                Role = "Staff";
            }
            else
            {
                Role = "Student";
            }
        }
    }
}