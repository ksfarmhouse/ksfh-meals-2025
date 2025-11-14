using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Website.Pages
{
    /// <summary>
    /// NOTE: This page is currently not being used.
    /// PageModel for the Privacy Policy page.
    /// Currently a placeholder page to display the site's privacy policy.
    /// </summary>
    public class PrivacyModel : PageModel
    {
        /// <summary>
        /// Private variable to hold the logger
        /// </summary>
        private readonly ILogger<PrivacyModel> _logger;

        /// <summary>
        /// Constructor accepting a logger.
        /// </summary>
        /// <param name="logger">Logger instance for this page.</param>
        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }
    }
}
