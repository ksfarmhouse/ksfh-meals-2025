using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Data;

namespace Website.Pages
{
    public class EarlyLatePlatesModel : PageModel
    {
        public IEnumerable<Member> AllMembers => House.AllMembers;

        public List<Member> SelectedMembersLateLunch { get; set; } = new List<Member>();

        public List<Member> SelectedMembersLateDinner { get; set; } = new List<Member>();

        public List<Member> SelectedMembersEarlyLunch { get; set; } = new List<Member>();

        public List<Member> SelectedMembersEarlyDinner { get; set; } = new List<Member>();

        public double LunchCookCount { get; set; } = -2; //take out crew

        public int LunchTableCount { get; set; } = 0;

        public double DinnerCookCount { get; set; } = -4; //take out crew add Mom T

        public int DinnerTableCount { get; set; } = 0;

        public int ButtonHit { get; set; } = 0;

        public string ?SelectedDay { get; set; }

        public void OnGet(string DaySelect)
        {
            ViewData["ActivePage"] = "Late/Early Plates";
        }

        /// <summary>
        /// When button is clicked, 4 boxes displaying the days early and late lunch and dinner plates and how many people will be at the meal
        /// </summary>
        /// <param name="DaySelect">The day selected</param>
        /// <returns>the same page with the 4 new boxes</returns>
        public IActionResult OnPost(string DaySelect)
        {
            SelectedDay = DaySelect;

            int lunchIndex = 0;
            int dinnerIndex = 1;


            switch (DaySelect)
            {
                //Monday
                case "M":
                    lunchIndex = 0; dinnerIndex = 1;
                    break;
                //Tuesday
                case "T":
                    lunchIndex = 2; dinnerIndex = 3;
                    break;

                //Wednesday
                case "W":
                    lunchIndex = 4; dinnerIndex = 5;
                    break;
                //Thursday
                case "TH":
                    lunchIndex = 6; dinnerIndex = 7;
                    break;
                //Friday
                case "F":
                    lunchIndex = 8; dinnerIndex = 9;
                    break;
                case "SA":
                    lunchIndex = 10; dinnerIndex = -1;
                    break;
                case "SU":
                    lunchIndex = 11; dinnerIndex = -1;
                    break;
                default:
                    lunchIndex = 0; dinnerIndex = 1;
                    break;
            }

            // Loop through all members and populate lists/counts
            foreach (Member m in AllMembers)
            {
                // Lunch
                if (m.TempSignUp[lunchIndex] == MealStatus.Late)
                    SelectedMembersLateLunch.Add(m);
                else if (m.TempSignUp[lunchIndex] == MealStatus.Early)
                    SelectedMembersEarlyLunch.Add(m);

                if (m.TempSignUp[lunchIndex] == MealStatus.In)
                    LunchCookCount++;

                if (dinnerIndex != -1)
                {
                    // Dinner
                    if (m.TempSignUp[dinnerIndex] == MealStatus.Late)
                        SelectedMembersLateDinner.Add(m);
                    else if (m.TempSignUp[dinnerIndex] == MealStatus.Early)
                        SelectedMembersEarlyDinner.Add(m);

                    if (m.TempSignUp[dinnerIndex] == MealStatus.In)
                        DinnerCookCount++;
                }

            }

            LunchTableCount = (int)Math.Ceiling(LunchCookCount / 9);
            DinnerTableCount = (int)Math.Ceiling(DinnerCookCount / 9);
            ButtonHit = 1;
            return Page();
        }
    }
}

