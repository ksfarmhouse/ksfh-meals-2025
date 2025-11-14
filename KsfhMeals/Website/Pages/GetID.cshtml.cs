using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Website.Pages
{
    /// <summary>
    /// PageModel for looking up a member's ID by their full name.
    /// </summary>
    public class GetIDModel : PageModel
    {
        /// <summary>
        /// Bound property for the user's full name input.
        /// </summary>
        [BindProperty]
        public string? FullNameInput { get; set; }

        /// <summary>
        /// The member found matching the input, or null if not found.
        /// </summary>
        public Member? FoundMember { get; set; }

        /// <summary>
        /// Message shown when lookup fails or input is invalid.
        /// </summary>
        public string? LookupResult { get; set; }

        /// <summary>
        /// All members in the house.
        /// </summary>
        public IEnumerable<Member> AllMembers => House.AllMembers;

        /// <summary>
        /// Handles POST requests to lookup a member by full name.
        /// </summary>
        public void OnPost()
        {
            if (string.IsNullOrWhiteSpace(FullNameInput))
            {
                LookupResult = "Please enter a valid name.";
                return;
            }

            string input = Normalize(FullNameInput);

            FoundMember = AllMembers.FirstOrDefault(m =>
                Normalize(m.FullName) == input);

            if (FoundMember == null)
                LookupResult = "User not found";
        }

        /// <summary>
        /// Normalizes a name string: trims, removes extra spaces, converts to lowercase.
        /// </summary>
        /// <param name="name">The name to normalize.</param>
        /// <returns>A normalized version of the name.</returns>
        private string Normalize(string name)
        {
            return string.Join(" ",
                name.Trim()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            ).ToLower();
        }
    }
}
