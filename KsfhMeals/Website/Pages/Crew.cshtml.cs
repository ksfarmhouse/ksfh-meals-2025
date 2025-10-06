using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Data;

namespace Website.Pages
{
    public class CookModel : PageModel
    {

        public string? SeasonAndYear { get; set; } = "Fall 2025";

        public int ButtonHit { get; set; }
        public void OnGet()
        {
        }

		public IActionResult OnPost(string DaySelect)
		{
			
			ButtonHit = 1;
			return Page();
		}

	}
}
