using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityService.Models;
using IdentityService.Pages.Account.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace IdentityService.Pages.Register
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class Index : PageModel
    {
        private const string Message = "User created a new account with password: {password}";
        private readonly ILogger<Index> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public Index(ILogger<Index> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [BindProperty]
        public RegisterViewModel Input { get; set; }
        [BindProperty]
        public bool RegisterSuccess { get; set; }

        public IActionResult OnGet(string returnUrl = null)
        {
            Input = new RegisterViewModel { ReturnUrl = returnUrl };
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (Input.Button != "register") return Redirect("~/");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.UserName,
                    Email = Input.Email,
                    EmailConfirmed = true
                };

                var res = await _userManager.CreateAsync(user, Input.Password);
                if (res.Succeeded)
                {
                    await _userManager.AddClaimsAsync(user, new Claim[] { new(JwtClaimTypes.Name, Input.FullName) });

                    RegisterSuccess = true;
                }
            }
            _logger.LogInformation(message: Message, args: Input.Password);
            return Page();
        }
    }
}