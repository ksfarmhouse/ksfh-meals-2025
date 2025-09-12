using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
//using Newtonsoft.Json;

namespace Website.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            ViewData["ActivePage"] = "Menu";
        }

        public IEnumerable<string> LunchMenu => House.Lunch;
        public IEnumerable<string> DinnerMenu => House.Dinner;
    }
}