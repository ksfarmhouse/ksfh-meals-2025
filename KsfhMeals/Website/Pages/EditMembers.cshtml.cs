using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Website.Pages
{
    /// <summary>
    /// PageModel for editing and managing house members.
    /// Allows adding new members, editing existing members' statuses, and removing members.
    /// </summary>
    public class EditMembersModel : PageModel
    {
        #region Properties

        /// <summary>
        /// Dropdown items for house statuses in fixed order.
        /// </summary>
        public List<SelectListItem> Items { get; set; } = new List<SelectListItem>
        {
                new SelectListItem { Value = "1", Text = "In House" },
                new SelectListItem { Value = "2", Text = "Out Of House" },
                new SelectListItem { Value = "3", Text = "New Member" },
                new SelectListItem { Value = "4", Text = "Alumni" }
        };

        /// <summary>
        /// Bound property for the first name when adding a new member.
        /// </summary>
        [BindProperty] public string? First { get; set; }

        /// <summary>
        /// Bound property for the last name when adding a new member.
        /// </summary>
        [BindProperty] public string? Last { get; set; }

        /// <summary>
        /// Bound property for the ID when adding a new member.
        /// </summary>
        [BindProperty] public string? ID { get; set; }

        /// <summary>
        /// Bound property for the selected status when adding a new member.
        /// </summary>
        [BindProperty] public string? SelectedStatus { get; set; }

        /// <summary>
        /// Returns all members in the house.
        /// </summary>
        public IEnumerable<Member> AllMembers => House.AllMembers;

        #endregion

        #region GET and POST Handlers

        /// <summary>
        /// Handles GET requests for the Edit Members page.
        /// Initializes combo box names for all members.
        /// </summary>
        public void OnGet() {}

        /// <summary>
        /// Adds a new member to the house.
        /// </summary>
        /// <returns>Returns the same page with updated members.</returns>
        public IActionResult OnPostNew()
        {
            if (!string.IsNullOrEmpty(ID) && !string.IsNullOrEmpty(First) && !string.IsNullOrEmpty(Last))
            {
                Status status = SelectedStatus switch
                {
                    "1" => Status.InHouse,
                    "2" => Status.OutOfHouse,
                    "3" => Status.NewMember,
                    _ => Status.Alumni
                };

                Member newMember = new Member(ID, First, Last, status);
                House.AddMember(newMember);
                House.Save();
            }

            return RedirectToPage();
        }

        /// <summary>
        /// Edits existing members' statuses based on the selected values in the dropdowns.
        /// Resets meal signups for all members.
        /// </summary>
        /// <returns>Returns the same page with updated members.</returns>
        public IActionResult OnPostEdit()
        {
            foreach (Member m in House.AllMembers)
            {
                string? newValue = Request.Form["Member" + m.ID];

                if (!string.IsNullOrEmpty(newValue))
                {
                    Status oldStatus = m.HouseStatus;

                    Status updatedStatus = newValue switch
                    {
                        "1" => Status.InHouse,
                        "2" => Status.OutOfHouse,
                        "3" => Status.NewMember,
                        _ => Status.Alumni
                    };

                    // ONLY update if the status actually changed
                    if (oldStatus != updatedStatus)
                    {
                        m.HouseStatus = updatedStatus;

                        bool wasActive = (oldStatus == Status.InHouse || oldStatus == Status.NewMember);
                        bool isActiveNow = (updatedStatus == Status.InHouse || updatedStatus == Status.NewMember);

                        if (wasActive != isActiveNow)
                        {
                            if (isActiveNow)
                            {
                                // Moving To In-House/New Member: Set to 'In'
                                m.TempMealSignUp = Enumerable.Repeat(MealStatus.In, 12).ToArray();
                                m.DefaultSignUp = Enumerable.Repeat(MealStatus.In, 12).ToArray();
                            }
                            else
                            {
                                // Moving To Out-Of-House/Alumni: Set to 'Out'
                                m.TempMealSignUp = Enumerable.Repeat(MealStatus.Out, 12).ToArray();
                                m.DefaultSignUp = Enumerable.Repeat(MealStatus.Out, 12).ToArray();
                            }
                        }
                    }
                }
            }

            House.Save();
            return RedirectToPage();
        }

        /// <summary>
        /// Removes a member from the house.
        /// </summary>
        /// <param name="RemoveMember">The ID of the member to remove.</param>
        /// <returns>Returns the same page with updated members.</returns>
        public IActionResult OnPostRemove(string RemoveMember)
        {
            if (!string.IsNullOrEmpty(RemoveMember))
            {
                House.RemoveMember(RemoveMember);
                House.Save();
            }
            return RedirectToPage();
        }

        #endregion

        #region Helper Methods
        /// <summary>
        /// Returns a list of statuses in the fixed order, reordered so the current status appears first.
        /// </summary>
        /// <param name="status">The current status of the member.</param>
        /// <returns>List of SelectListItems for the dropdown.</returns>
        public List<SelectListItem> GetOtherStatus(Status currentStatus)
        {
            // Reordering the list so the current status is selected by default
            var list = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "In House", Selected = (currentStatus == Status.InHouse) },
                new SelectListItem { Value = "2", Text = "Out Of House", Selected = (currentStatus == Status.OutOfHouse) },
                new SelectListItem { Value = "3", Text = "New Member", Selected = (currentStatus == Status.NewMember) },
                new SelectListItem { Value = "4", Text = "Alumni", Selected = (currentStatus == Status.Alumni) }
            };

            // Move the selected item to the top of the list
            var selectedItem = list.FirstOrDefault(x => x.Selected);
            if (selectedItem != null)
            {
                list.Remove(selectedItem);
                list.Insert(0, selectedItem);
            }

            return list;
        }

        #endregion
    }
}