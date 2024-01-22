using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Data;

namespace Website.Pages
{
    public class CookModel : PageModel
    {
		public double LunchCookCount { get; set; } = 0;

		public int LunchTableCount { get; set; } = 0;

		public double DinnerCookCount { get; set; } = 1;

		public int DinnerTableCount { get; set; } = 0;

		public int ButtonHit { get; set; }
        public void OnGet()
        {
        }

		public IActionResult OnPost(string DaySelect)
		{
			if (DaySelect == "M")
			{
				foreach (Member m in House.AllMembers)
				{
					if (m.TempSignUp[0] == MealStatus.In)
						LunchCookCount++;

					if (m.TempSignUp[1] == MealStatus.In)
						DinnerCookCount++;
				}
			}

			else if (DaySelect == "T")
			{
				foreach (Member m in House.AllMembers)
				{
					if (m.TempSignUp[2] == MealStatus.In)
						LunchCookCount++;

					if (m.TempSignUp[3] == MealStatus.In)
						DinnerCookCount++;
				}
			}

			else if (DaySelect == "W")
			{
				foreach (Member m in House.AllMembers)
				{
					if (m.TempSignUp[4] == MealStatus.In)
						LunchCookCount++;

					if (m.TempSignUp[5] == MealStatus.In)
						DinnerCookCount++;
				}
			}

			else if (DaySelect == "U")
			{
				foreach (Member m in House.AllMembers)
				{
					if (m.TempSignUp[6] == MealStatus.In)
						LunchCookCount++;

					if (m.TempSignUp[7] == MealStatus.In)
						DinnerCookCount++;
				}
			}
			else
			{
				foreach (Member m in House.AllMembers)
				{
					if (m.TempSignUp[8] == MealStatus.In)
						LunchCookCount++;

					if (m.TempSignUp[9] == MealStatus.In)
						DinnerCookCount++;
				}
			}
			DinnerCookCount -= 5;
			LunchTableCount = (int)Math.Ceiling(LunchCookCount / 9);
			DinnerTableCount = (int)Math.Ceiling(DinnerCookCount / 9);
			ButtonHit = 1;
			return Page();
		}
	}
}
