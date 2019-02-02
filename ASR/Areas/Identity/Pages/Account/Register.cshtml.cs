using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ASR.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ASR.Data;
using System.Text.RegularExpressions;

namespace ASR.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            [RegularExpression(@"^(e\d{5}@rmit.edu.au)|(s\d{7}@student.rmit.edu.au)$", ErrorMessage = "Invalid staff or student login format")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Key]
            public string SchoolID { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/Identity/Account/Success");
            if (ModelState.IsValid)
            {
                bool isStaff = false;
                string regex;

                if (Input.Email.StartsWith('e'))
                {
                    isStaff = true;
                    regex = @"^(e\d{5})";
                }
                else
                {
                    regex = @"^(s\d{7})";
                }

                Regex r = new Regex(regex, RegexOptions.IgnoreCase);
                Match m = r.Match(Input.Email);

                if (m.Success)
                {
                    Input.SchoolID = m.ToString();
                }
                else
                {
                    return Page();
                }

                var user = new AppUser { UserName = Input.Email, Email = Input.Email, SchoolID = Input.SchoolID, Name = Input.Name };

                try
                {
                    var result = await _userManager.CreateAsync(user, Input.Password);

                    if (isStaff)
                    {
                        if (user != null && !await _userManager.IsInRoleAsync(user, Constants.Staff))
                            await _userManager.AddToRoleAsync(user, Constants.Staff);
                    }
                    else
                    {
                        if (user != null && !await _userManager.IsInRoleAsync(user, Constants.Student))
                            await _userManager.AddToRoleAsync(user, Constants.Student);
                    }

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");

                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { userId = user.Id, code = code },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        await _signInManager.SignInAsync(user, isPersistent: false);

                        return RedirectToPage("Success", "Success", new { name = user.Name, email = user.Email, schoolID = user.SchoolID, staffRole = isStaff});                    
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, $"The SchoolID '{user.SchoolID}' is already in use");
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
