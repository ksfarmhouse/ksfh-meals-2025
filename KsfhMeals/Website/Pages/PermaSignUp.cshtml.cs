using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Website.Pages
{
    /// <summary>
    /// NOTE: This page is currently not being used.
    /// PageModel for managing permanent meal sign-ups.
    /// Allows members to set their default weekly meal schedule.
    /// </summary>
    public class PermaSignUpModel : PageModel
    {
        #region Properties

        /// <summary>
        /// Bound property for member ID input.
        /// </summary>
        [BindProperty]
        public string? ID { get; set; }

        /// <summary>
        /// The member currently being edited/viewed.
        /// </summary>
        public Member? MemberToShow { get; set; }

        /// <summary>
        /// Hidden property for member ID in forms.
        /// </summary>
        [HiddenInput]
        public string? MemberID { get; set; }

        /// <summary>
        /// Stores confirmation message after saving sign-up.
        /// </summary>
        public string? SaveConfirmationMessage { get; set; }

        /// <summary>
        /// Names of combo boxes for each meal slot.
        /// </summary>
        public string[] comboBoxNames { get; set; } = new string[]
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
        /// Bound property for selected meal type (if needed for future).
        /// </summary>
        [BindProperty]
        public string? SelectedMealType { get; set; }

        #endregion

        #region Handlers

        /// <summary>
        /// Handles POST when member ID is entered to display their current schedule.
        /// </summary>
        public void OnPost()
        {
            MemberToShow = GetMember(ID!);
            MemberID = ID;
        }

        /// <summary>
        /// Handles POST for saving the permanent meal sign-up.
        /// Updates both DefaultSignUp and MealSignUp arrays for the member.
        /// </summary>
        /// <returns>Page with updated confirmation message.</returns>
        public IActionResult OnPostEditSignUp()
        {
            MemberToShow = GetMember(ID!);

            for (int i = 0; i < comboBoxNames.Length; i++)
            {
                string value = Request.Form[comboBoxNames[i]]!;
                MemberToShow.DefaultSignUp[i] = value switch
                {
                    "1" => MealStatus.In,
                    "2" => MealStatus.Out,
                    "3" => MealStatus.Early,
                    "4" => MealStatus.Late,
                    "5" => MealStatus.Tardy,
                    _ => MemberToShow.DefaultSignUp[i] // fallback
                };

                MemberToShow.TempMealSignUp[i] = MemberToShow.DefaultSignUp[i];
            }

            House.Save();
            SaveConfirmationMessage = "Your meal sign-up has been saved successfully!";
            return Page();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Finds a member by ID.
        /// </summary>
        /// <param name="id">Member ID to search for.</param>
        /// <returns>The matching Member or null if not found.</returns>
        public Member GetMember(string id)
        {
            return House.AllMembers.FirstOrDefault(m => m.ID == id)!;
        }

        /// <summary>
        /// Returns a list of lunch items with the current status selected.
        /// </summary>
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
        /// Returns a list of dinner items with the current status selected (no Tardy for dinner).
        /// </summary>
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
