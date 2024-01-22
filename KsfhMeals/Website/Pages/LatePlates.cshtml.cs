using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Data;

namespace Website.Pages
{
    public class LatePlatesModel : PageModel
    {
        public IEnumerable<Member> AllMembers => House.AllMembers;

        public List<Member> SelectedMembersLateLunch { get; set; } = new List<Member>();
        
        public List<Member> SelectedMembersLateDinner { get; set; } = new List<Member>();

		public List<Member> SelectedMembersEarlyLunch { get; set; } = new List<Member>();

		public List<Member> SelectedMembersEarlyDinner { get; set; } = new List<Member>();

		public int ButtonHit { get; set; } = 0;
        public void OnGet(string DaySelect)
        {
		
        }

		public IActionResult OnPost(string DaySelect)
        {
            if (DaySelect == "M")
            {
                foreach (Member m in AllMembers)
                {
                    if (m.TempSignUp[0] == MealStatus.Late)
                        SelectedMembersLateLunch.Add(m);
					else if (m.TempSignUp[0] == MealStatus.Early)
						SelectedMembersEarlyLunch.Add(m);

					if (m.TempSignUp[1] == MealStatus.Late)
						SelectedMembersLateDinner.Add(m);
					else if (m.TempSignUp[1] == MealStatus.Early)
						SelectedMembersEarlyDinner.Add(m);
                }
			}

			else if (DaySelect == "T")
			{
				foreach (Member m in AllMembers)
				{
					if (m.TempSignUp[2] == MealStatus.Late)
						SelectedMembersLateLunch.Add(m);
					else if (m.TempSignUp[2] == MealStatus.Early)
						SelectedMembersEarlyLunch.Add(m);

					if (m.TempSignUp[3] == MealStatus.Late)
						SelectedMembersLateDinner.Add(m);
					else if (m.TempSignUp[3] == MealStatus.Early)
						SelectedMembersEarlyDinner.Add(m);
				}
			}

			else if (DaySelect == "W")
			{
				foreach (Member m in AllMembers)
				{
					if (m.TempSignUp[4] == MealStatus.Late)
						SelectedMembersLateLunch.Add(m);
					else if (m.TempSignUp[4] == MealStatus.Early)
						SelectedMembersEarlyLunch.Add(m);

					if (m.TempSignUp[5] == MealStatus.Late)
						SelectedMembersLateDinner.Add(m);
					else if (m.TempSignUp[5] == MealStatus.Early)
						SelectedMembersEarlyDinner.Add(m);
				}
			}

			else if (DaySelect == "U")
			{
				foreach (Member m in AllMembers)
				{
					if (m.TempSignUp[6] == MealStatus.Late)
						SelectedMembersLateLunch.Add(m);
					else if (m.TempSignUp[6] == MealStatus.Early)
						SelectedMembersEarlyLunch.Add(m);

					if (m.TempSignUp[7] == MealStatus.Late)
						SelectedMembersLateDinner.Add(m);
					else if (m.TempSignUp[7] == MealStatus.Early)
						SelectedMembersEarlyDinner.Add(m);
				}
			}
			else
			{
				foreach (Member m in AllMembers)
				{
					if (m.TempSignUp[8] == MealStatus.Late)
						SelectedMembersLateLunch.Add(m);
					else if (m.TempSignUp[8] == MealStatus.Early)
						SelectedMembersEarlyLunch.Add(m);

					if (m.TempSignUp[9] == MealStatus.Late)
						SelectedMembersLateDinner.Add(m);
					else if (m.TempSignUp[9] == MealStatus.Early)
						SelectedMembersEarlyDinner.Add(m);
				}
			}
			ButtonHit = 1;
			return Page();
		}
    }
}
