using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Website.Pages
{
    /// <summary>
    /// Represents the Admin page model for managing meals and members.
    /// Provides actions for editing the weekly menu, rolling over meals,
    /// resetting meals, and updating new member statuses.
    /// </summary>
    public class AdminModel : PageModel
    {
        #region Menu Properties

        /// <summary>
        /// Monday lunch input from the admin.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string? ML { get; set; }

        /// <summary>
        /// Monday dinner input from the admin.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string? MD { get; set; }

        /// <summary>
        /// Tuesday lunch input from the admin.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string? TL { get; set; }

        /// <summary>
        /// Tuesday dinner input from the admin.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string? TD { get; set; }

        /// <summary>
        /// Wednesday lunch input from the admin.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string? WL { get; set; }

        /// <summary>
        /// Wednesday dinner input from the admin.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string? WD { get; set; }

        /// <summary>
        /// Thursday lunch input from the admin.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string? THL { get; set; }

        /// <summary>
        /// Thursday dinner input from the admin.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string? THD { get; set; }

        /// <summary>
        /// Friday lunch input from the admin.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string? FL { get; set; }

        /// <summary>
        /// Friday dinner input from the admin.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string? FD { get; set; }

        /// <summary>
        /// Saturday lunch input from the admin.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string? SA { get; set; }

        /// <summary>
        /// Sunday lunch input from the admin.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string? SU { get; set; }

        #endregion

        #region Feedback Messages

        /// <summary>
        /// Message displayed after saving the weekly schedule.
        /// </summary>
        public string? SaveScheduleMessage { get; set; }

        /// <summary>
        /// Message displayed after performing a meal rollover.
        /// </summary>
        public string? SaveRolloverMessage { get; set; }

        /// <summary>
        /// Message displayed after resetting meals.
        /// </summary>
        public string? SaveResetMessage { get; set; }

        /// <summary>
        /// Message displayed after updating new members to In House status.
        /// </summary>
        public string? UpdateNMMessage { get; set; }

        #endregion

        #region POST Handlers

        /// <summary>
        /// Handles POST requests when the admin submits the weekly menu form.
        /// Saves lunch and dinner schedules if all inputs are provided.
        /// </summary>
        public void OnPost()
        {
            // Collect lunch and dinner inputs into lists
            List<string> lunch = new List<string> { ML!, TL!, WL!, THL!, FL!, SA!, SU! };
            List<string> dinner = new List<string> { MD!, TD!, WD!, THD!, FD! };

            // Check if all inputs are non-null
            if (!lunch.Contains(null!) && !dinner.Contains(null!))
            {
                // Save meals to House data
                House.AddLunchItem(lunch);
                House.AddDinnerItem(dinner);

                SaveScheduleMessage = "Meal Schedule has been saved successfully!";
            }
        }

        /// <summary>
        /// Handles POST requests for rolling over meals.
        /// Returns temporary meals to permanent signups and updates counts for out-of-house members.
        /// </summary>
        public void OnPostRollover()
        {
            foreach (Member m in House.AllMembers)
            {
                for (int i = 0; i < m.MealSignUp.Length; i++)
                {
                    bool isLunch = (i % 2 == 0) || (i >= 10); // weekday lunch OR weekend lunch
                    bool isDinner = (i % 2 == 1 && i < 10);   // only Mon–Fri dinners

                    if (m.HouseStatus == Status.OutOfHouse && m.MealSignUp[i] != MealStatus.Out )
                    {
                        if (isLunch) m.LunchCount++;
                        if (isDinner) m.DinnerCount++;
                    }

                    m.MealSignUp[i] = m.DefaultSignUp[i];
                }
            }

            House.Save();
            SaveRolloverMessage = "Meal Rollover has been saved successfully!";
        }

        /// <summary>
        /// Handles POST requests for resetting all member meals.
        /// Resets all meal signups and counts to defaults.
        /// </summary>
        /// <returns>The current page with updated message.</returns>
        public IActionResult OnPostResetMeals()
        {
            foreach (Member m in House.AllMembers)
            {
                m.MealSignUp = Enumerable.Repeat(MealStatus.Out, 12).ToArray();
                m.DefaultSignUp = Enumerable.Repeat(MealStatus.Out, 12).ToArray();
                m.DinnerCount = 0;
                m.LunchCount = 0;
            }

            House.Save();
            SaveResetMessage = "Meal Reset has been saved successfully!";

            return Page();
        }

        /// <summary>
        /// Handles POST requests to update new members to In House status.
        /// </summary>
        public void OnPostUpdateNM()
        {
            foreach (Member m in House.AllMembers)
            {
                if (m.HouseStatus == Status.NewMember)
                {
                    m.HouseStatus = Status.InHouse;
                }
            }

            House.Save();
            UpdateNMMessage = "All New Members are now In House Members";
        }

        #endregion
    }
}
