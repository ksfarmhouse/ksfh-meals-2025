using Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Website.Pages
{
    /// <summary>
    /// PageModel for displaying the weekly meal menu.
    /// Shows weekday and weekend lunch and dinner menus.
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        /// <summary>
        /// Constructor with logger injection.
        /// </summary>
        /// <param name="logger">Logger for diagnostic purposes.</param>
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Gets the lunch menu for the week (Monday–Sunday).
        /// </summary>
        public IEnumerable<string> LunchMenu => House.Lunch;

        /// <summary>
        /// Gets the dinner menu for weekdays (Monday–Friday).
        /// </summary>
        public IEnumerable<string> DinnerMenu => House.Dinner;
    }
}
