using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Website.Pages
{
    public class AuthorizeModel : PageModel
    {
        public void OnGet()
        {
            // Handle GET requests
        }

        public IActionResult OnPost(string password)
        {
            // Check if the entered password is correct
            if (password == "ksfh1921")
            {
                // Grant access
                return RedirectToPage("/Admin");
            }
            else
            {
                // Show an error
                ModelState.AddModelError("password", "Invalid password");
                return Page();
            }
        }
    }
}
