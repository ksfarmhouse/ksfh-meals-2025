using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Data;

namespace Website.Pages
{
    /// <summary>
    /// PageModel for managing crew schedules for meals.
    /// Displays inputs for active members and new members for each meal and day.
    /// </summary>
    public class CookModel : PageModel
    {
        /// <summary>
        /// Represents the current season and year displayed on the page.
        /// </summary>
        public string? SeasonAndYear { get; set; } = "Fall 2025";

        /// <summary>
        /// Tracks which button (or action) was triggered on the page.
        /// </summary>
        public int ButtonHit { get; set; }

        /// <summary>
        /// Handles POST requests when a day is selected or a form action is triggered.
        /// </summary>
        /// <param name="DaySelect">The day selected by the user (currently unused).</param>
        /// <returns>The same page with updated state.</returns>
        public IActionResult OnPost(string DaySelect)
        {
            ButtonHit = 1;

            return Page();
        }
    }
}
