using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Website.Pages
{
    /// <summary>
    /// PageModel for weekly meal sign-ups.
    /// Allows a member to enter their ID, view their default meal plan,
    /// and adjust their meal attendance for the week.
    /// </summary>
    public class MealSignUpsModel : PageModel
    {
        #region Properties

        /// <summary>
        /// Bound property for the ID entered by the user.
        /// </summary>
        [BindProperty]
        public string? ID { get; set; }

        /// <summary>
        /// The member being displayed for sign-up edits.
        /// </summary>
        public Member? MemberToShow { get; set; }

        /// <summary>
        /// Hidden property to hold member ID for form submission.
        /// </summary>
        [HiddenInput]
        public string? MemberID { get; set; }

        /// <summary>
        /// Confirmation message displayed after saving sign-ups.
        /// </summary>
        public string? SaveConfirmationMessage { get; set; }

        /// <summary>
        /// Names of the dropdowns used in the meal sign-up form.
        /// Matches indices of Member.MealSignUp array.
        /// </summary>
        public string[] comboBoxNames = new string[]
        {
            "ML", "MD",  // Monday
            "TL", "TD",  // Tuesday
            "WL", "WD",  // Wednesday
            "THL", "THD",// Thursday
            "FL", "FD",  // Friday
            "SA",        // Saturday
            "SU"         // Sunday
        };

        /// <summary>
        /// Bound property for selected meal type (optional for future use).
        /// </summary>
        [BindProperty]
        public string? SelectedMealType { get; set; }

        #endregion

        #region POST Handlers

        /// <summary>
        /// Handles POST requests when a member enters their ID.
        /// Sets the member to display and stores the ID for hidden input.
        /// </summary>
        public void OnPost()
        {
            MemberToShow = GetMember(ID!);
            MemberID = ID;
        }

        /// <summary>
        /// Handles POST requests to save a member's meal sign-up changes.
        /// Updates the MealSignUp array based on form input and saves the house data.
        /// </summary>
        /// <returns>The page with confirmation message.</returns>
        public IActionResult OnPostEditSignUp()
        {
            MemberToShow = GetMember(ID!);

            for (int i = 0; i < comboBoxNames.Length; i++)
            {
                string value = Request.Form[comboBoxNames[i]]!;

                MemberToShow.TempMealSignUp[i] = value switch
                {
                    "1" => MealStatus.In,
                    "2" => MealStatus.Out,
                    "3" => MealStatus.Early,
                    "4" => MealStatus.Late,
                    "5" => MealStatus.Tardy,
                    _ => MemberToShow.TempMealSignUp[i] // fallback to existing value
                };
            }

            House.Save();
            SaveConfirmationMessage = "Your meal sign-up has been saved successfully!";
            return Page();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Retrieves a member from the house by ID.
        /// </summary>
        /// <param name="id">Member ID to search for.</param>
        /// <returns>The Member object if found, otherwise null.</returns>
        public Member GetMember(string id)
        {
            foreach (Member m in House.AllMembers)
            {
                if (m.ID == id)
                    return m;
            }
            return null!;
        }

        /// <summary>
        /// Returns a list of SelectListItem for lunch dropdowns, with the current status selected.
        /// Includes "Tardy" option.
        /// </summary>
        /// <param name="currentStatus">Current MealStatus of the member.</param>
        /// <returns>List of SelectListItem for lunch.</returns>
        public List<SelectListItem> GetLunchItems(MealStatus currentStatus)
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "In", Selected = currentStatus == MealStatus.In },
                new SelectListItem { Value = "2", Text = "Out", Selected = currentStatus == MealStatus.Out },
                new SelectListItem { Value = "3", Text = "Early", Selected = currentStatus == MealStatus.Early },
                new SelectListItem { Value = "4", Text = "Late", Selected = currentStatus == MealStatus.Late },
                new SelectListItem { Value = "5", Text = "Tardy", Selected = currentStatus == MealStatus.Tardy }
            };
        }

        /// <summary>
        /// Returns a list of SelectListItem for dinner dropdowns, with the current status selected.
        /// No "Tardy" option for dinner.
        /// </summary>
        /// <param name="currentStatus">Current MealStatus of the member.</param>
        /// <returns>List of SelectListItem for dinner.</returns>
        public List<SelectListItem> GetDinnerItems(MealStatus currentStatus)
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "In", Selected = currentStatus == MealStatus.In },
                new SelectListItem { Value = "2", Text = "Out", Selected = currentStatus == MealStatus.Out },
                new SelectListItem { Value = "3", Text = "Early", Selected = currentStatus == MealStatus.Early },
                new SelectListItem { Value = "4", Text = "Late", Selected = currentStatus == MealStatus.Late }
            };
        }

        #endregion
    }
}
