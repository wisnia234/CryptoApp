#nullable disable

using System.ComponentModel.DataAnnotations;
using CryptoApp.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CryptoApp.Server.Areas.Identity.Pages.Account.Manage;

public class IndexModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public IndexModel(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    
    public string Username { get; set; }
   
    [TempData]
    public string StatusMessage { get; set; }
   
    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        
        [Display(Name = nameof(Password))]
        public string Password { get; set; }
        public string UserName { get; set; }
    }

    private async Task LoadAsync(ApplicationUser user)
    {
        var userName = await _userManager.GetUserNameAsync(user);
        /*var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

        Username = userName;*/

        Input = new InputModel
        {
            UserName = userName
        };
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        await LoadAsync(user);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        if (!ModelState.IsValid)
        {
            await LoadAsync(user);
            return Page();
        }

        var userName = await _userManager.GetUserNameAsync(user);
        if(!string.Equals(Username,userName))
        {
            var setUserNameResult = await _userManager.SetUserNameAsync(user, Input.UserName);
            if (!setUserNameResult.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to set username.";
                return RedirectToPage();
            }
        }
        StatusMessage = "Your profile has been updated \n Please logout ang login again to refresh your username";
        return RedirectToPage();
    }
}
