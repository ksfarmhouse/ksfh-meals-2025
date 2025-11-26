using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Data;

namespace Website.Pages
{
    /// <summary>
    /// PageModel for displaying Early and Late Plates for each day of the week.
    /// Populates lists of members for early/late lunch and dinner and calculates table and cook counts.
    /// </summary>
    public class EarlyLatePlatesModel : PageModel
    {
        #region Properties

        /// <summary>
        /// All members in the house.
        /// </summary>
        public IEnumerable<Member> AllMembers => House.AllMembers;

        /// <summary>
        /// Members selected for late lunch on the chosen day.
        /// </summary>
        public List<Member> SelectedMembersLateLunch { get; set; } = new List<Member>();

        /// <summary>
        /// Members selected for late dinner on the chosen day.
        /// </summary>
        public List<Member> SelectedMembersLateDinner { get; set; } = new List<Member>();

        /// <summary>
        /// Members selected for early lunch on the chosen day.
        /// </summary>
        public List<Member> SelectedMembersEarlyLunch { get; set; } = new List<Member>();

        /// <summary>
        /// Members selected for early dinner on the chosen day.
        /// </summary>
        public List<Member> SelectedMembersEarlyDinner { get; set; } = new List<Member>();

        /// <summary>
        /// Number of members who will cook for lunch.
        /// Starts negative to remove crew automatically.
        /// </summary>
        public double LunchCookCount { get; set; } = -2;

        /// <summary>
        /// Number of tables needed for lunch.
        /// </summary>
        public int LunchTableCount { get; set; } = 0;

        /// <summary>
        /// Number of members who will cook for dinner.
        /// Starts negative to adjust crew and Mom T automatically.
        /// </summary>
        public double DinnerCookCount { get; set; } = -4;

        /// <summary>
        /// Number of tables needed for dinner.
        /// </summary>
        public int DinnerTableCount { get; set; } = 0;

        /// <summary>
        /// Number of members who are tardy for lunch.
        /// </summary>
        public int LunchTardyCount { get; set; } = 0;

        /// <summary>
        /// Tracks whether a day button has been clicked.
        /// </summary>
        public int ButtonHit { get; set; } = 0;

        /// <summary>
        /// The day selected by the user (M, T, W, TH, F, SA, SU).
        /// </summary>
        public string? SelectedDay { get; set; }

        #endregion

        #region POST Handler

        /// <summary>
        /// Handles POST requests when a day button is clicked.
        /// Populates early/late lunch and dinner member lists and calculates cook and table counts.
        /// </summary>
        /// <param name="DaySelect">The day selected by the user.</param>
        /// <returns>The same page with updated member lists and counts.</returns>
        public IActionResult OnPost(string DaySelect)
        {
            SelectedDay = DaySelect;

            int lunchIndex = 0;
            int dinnerIndex = 1;

            switch (DaySelect)
            {
                case "M": lunchIndex = 0; dinnerIndex = 1; break;
                case "T": lunchIndex = 2; dinnerIndex = 3; break;
                case "W": lunchIndex = 4; dinnerIndex = 5; break;
                case "TH": lunchIndex = 6; dinnerIndex = 7; break;
                case "F": lunchIndex = 8; dinnerIndex = 9; break;
                case "SA": lunchIndex = 10; dinnerIndex = -1; break; // no dinner on Saturday
                case "SU": lunchIndex = 11; dinnerIndex = -1; break; // no dinner on Sunday
                default: lunchIndex = 0; dinnerIndex = 1; break;
            }

            // Populate member lists and cook counts
            foreach (Member m in AllMembers)
            {
                // Lunch
                switch (m.TempMealSignUp[lunchIndex])
                {
                    case MealStatus.Late: SelectedMembersLateLunch.Add(m); break;
                    case MealStatus.Early: SelectedMembersEarlyLunch.Add(m); break;
                    case MealStatus.In: LunchCookCount++; break;
                    case MealStatus.Tardy: LunchTardyCount++; break;
                }

                // Dinner (no Tardy)
                if (dinnerIndex != -1)
                {
                    switch (m.TempMealSignUp[dinnerIndex])
                    {
                        case MealStatus.Late: SelectedMembersLateDinner.Add(m); break;
                        case MealStatus.Early: SelectedMembersEarlyDinner.Add(m); break;
                        case MealStatus.In: DinnerCookCount++; break;
                    }
                }
            }

            // Ensure counts are not negative
            LunchCookCount = Math.Max(0, LunchCookCount);
            DinnerCookCount = Math.Max(0, DinnerCookCount);

            // Calculate number of tables (9 members per table)
            LunchTableCount = (int)Math.Ceiling(LunchCookCount / 9);
            DinnerTableCount = (int)Math.Ceiling(DinnerCookCount / 9);

            ButtonHit = 1;

            return Page();
        }

        #endregion
    }
}
