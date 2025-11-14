using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Website.Pages
{
    /// <summary>
    /// PageModel for the password authorization page.
    /// Handles verifying the admin password before granting access to secured pages.
    /// </summary>
    public class AuthorizeModel : PageModel
    {

        /// <summary>
        /// Handles POST requests when the user submits the password form.
        /// Checks if the password is correct and either redirects or shows an error.
        /// </summary>
        /// <param name="password">The password entered by the user.</param>
        /// <returns>
        /// Redirects to the Admin page if password is correct; otherwise reloads the current page with an error.
        /// </returns>
        public IActionResult OnPost(string password)
        {
            // Compare the entered password with the expected password
            if (password == "ksfh1921")
            {
                // Password is correct — grant access to Admin page
                return RedirectToPage("/Admin");
            }
            else
            {
                // Password is incorrect — display an error message on the form
                ModelState.AddModelError("password", "Invalid password");
                return Page();
            }
        }
    }
}
