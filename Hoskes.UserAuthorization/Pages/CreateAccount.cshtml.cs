using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Hoskes.Account.Core;
using Hoskes.UserAuthorization.Services;

namespace Hoskes.UserAuthorization.Pages
{
    public class CreateAccountModel : PageModel
    {
        private readonly UserService _userService;

        public CreateAccountModel(UserService userService)
        {
            _userService = userService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public User User { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _userService.RegisterUserAsync(User);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            } 

            return RedirectToPage("./Index");
        }
    }
}
