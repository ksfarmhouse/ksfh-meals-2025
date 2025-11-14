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
        /// Dropdown for dynamically changing status based on current member.
        /// </summary>
        public List<SelectListItem>? Statuses { get; set; }

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
        /// Bound property for the selected status when editing an existing member.
        /// </summary>
        [BindProperty] public string? SelectedEditStatus { get; set; }

        /// <summary>
        /// Unique names for each member's combo box in the table.
        /// </summary>
        public List<string> ComboBoxNames { get; set; } = new List<string>();

        /// <summary>
        /// Index used to track combo box names when rendering table rows.
        /// </summary>
        public int Index { get; set; }

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
        public void OnGet()
        {
            // strange issues requiring reposting items for combo box, look into it later
            Items = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "In House" },
                new SelectListItem { Value = "2", Text = "Out Of House" },
                new SelectListItem { Value = "3", Text = "New Member" },
                new SelectListItem { Value = "4", Text = "Alumni" }
            };

            foreach (Member m in House.AllMembers)
            {
                ComboBoxNames.Add("Member" + m.ID);
            }
        }

        /// <summary>
        /// Adds a new member to the house.
        /// </summary>
        /// <returns>Returns the same page with updated members.</returns>
        public IActionResult OnPostNew()
        {
            Status status = new Status();
            if (SelectedStatus == "1") status = Status.InHouse;
            if (SelectedStatus == "2") status = Status.OutOfHouse;
            if (SelectedStatus == "3") status = Status.NewMember;
            if (SelectedStatus == "4") status = Status.Alumni;
            Member newMember = new Member(ID!, First!, Last!, status);
            House.AddMember(newMember);

            Items = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "In House" },
                new SelectListItem { Value = "2", Text = "Out Of House" },
                new SelectListItem { Value = "3", Text = "New Member" },
                new SelectListItem { Value = "4", Text = "Alumni" }
            };

            foreach (Member m in House.AllMembers)
            {
                ComboBoxNames.Add("Member" + m.ID);
            }

            Index = 0;

            return Page();
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
                string value = Request.Form["Member" + m.ID]!;

                m.HouseStatus = value switch
                {
                    "1" => Status.InHouse,
                    "2" => Status.OutOfHouse,
                    "3" => Status.NewMember,
                    _ => Status.Alumni
                };

                // Reset meal signups
                m.MealSignUp = Enumerable.Repeat(MealStatus.Out, 12).ToArray();
                m.DefaultSignUp = Enumerable.Repeat(MealStatus.Out, 12).ToArray();

                ComboBoxNames.Add("Member" + m.ID);
            }

            House.Save();
            return Page();
        }

        /// <summary>
        /// Removes a member from the house.
        /// </summary>
        /// <param name="RemoveMember">The ID of the member to remove.</param>
        /// <returns>Returns the same page with updated members.</returns>
        public IActionResult OnPostRemove(string RemoveMember)
        {
            House.RemoveMember(RemoveMember);
            Items = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "In House" },
                new SelectListItem { Value = "2", Text = "Out Of House" },
                new SelectListItem { Value = "3", Text = "New Member" },
                new SelectListItem { Value = "4", Text = "Alumni" }
            };
            Index = 0;
            foreach (Member m in House.AllMembers)
            {
                ComboBoxNames.Add("Member" + m.ID);
            }
            return Page();
        }

        #endregion

        #region Helper Methods
        /// <summary>
        /// Returns a list of statuses in the fixed order, reordered so the current status appears first.
        /// </summary>
        /// <param name="status">The current status of the member.</param>
        /// <returns>List of SelectListItems for the dropdown.</returns>
        public List<SelectListItem> GetOtherStatus(Status status)
        {
            if (status == Status.InHouse)
            {
                Statuses = new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "In House" },
                    new SelectListItem { Value = "2", Text = "Out Of House" },
                    new SelectListItem { Value = "3", Text = "New Member" },
                    new SelectListItem { Value = "4", Text = "Alumni" }
                };
                return Statuses;
            }

            else if (status == Status.NewMember)
            {
                Statuses = new List<SelectListItem>
                {
                    new SelectListItem { Value = "3", Text = "New Member" },
                    new SelectListItem { Value = "1", Text = "In House" },
                    new SelectListItem { Value = "2", Text = "Out Of House" },
                    new SelectListItem { Value = "4", Text = "Alumni" }
                };
                return Statuses;
            }

            else if (status == Status.OutOfHouse)
            {
                Statuses = new List<SelectListItem>
                {
                    new SelectListItem { Value = "2", Text = "Out Of House" },
                    new SelectListItem { Value = "3", Text = "New Member" },
                    new SelectListItem { Value = "1", Text = "In House" },
                    new SelectListItem { Value = "4", Text = "Alumni" }
                };
                return Statuses;
            }
            else
            {
                Statuses = new List<SelectListItem>
                {
                    new SelectListItem { Value = "4", Text = "Alumni" },
                    new SelectListItem { Value = "3", Text = "New Member" },
                    new SelectListItem { Value = "2", Text = "Out Of House" },
                    new SelectListItem { Value = "1", Text = "In House" }
                };
                return Statuses;
            }
        }

        #endregion
    }
}