using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Hoskes.Account.Core;
using Hoskes.UserAuthorization.Services;

namespace Hoskes.UserAuthorization.Pages
{
    public class LoginAccountModel : PageModel
    {
        private readonly UserService _userService;

        public LoginAccountModel(UserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public UserLogin User { get; set; } = default!;

        public IActionResult OnGet(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Validate user credentials
                var user = await _userService.LoginAsync(User.Email, User.Password);
                if (user != null)
                {
                    // Create user claims
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Email)
                        // Add additional claims as needed
                    };

                    // Create claims identity
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Sign in the user
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        new AuthenticationProperties
                        {
                            IsPersistent = true, // Remember me
                            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) // Set session timeout
                        });

                    // Redirect to the return URL if it's local, otherwise to the profile page
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToPage("/Profile");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
