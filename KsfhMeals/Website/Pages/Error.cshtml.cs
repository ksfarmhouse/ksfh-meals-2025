using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace Website.Pages
{
    /// <summary>
    /// NOTE: This page is not heavily used. 
    /// Standard error page that displays a friendly message and optionally the request ID.
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        /// <summary>
        /// The unique request ID for this error.
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Determines whether to show the request ID.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        /// <summary>
        /// Private variable to hold the logger
        /// </summary>
        private readonly ILogger<ErrorModel> _logger;

        /// <summary>
        /// Constructor with logger injection.
        /// </summary>
        /// <param name="logger">Logger instance for error tracking.</param>
        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Handles GET requests to the error page.
        /// Sets the RequestId for display.
        /// </summary>
        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}
