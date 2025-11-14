using Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Website.Pages
{
    /// <summary>
    /// PageModel for the Treasurer page.
    /// Displays members' meal counts and their house status.
    /// </summary>
    public class TreasurerModel : PageModel
    {
        /// <summary>
        /// Returns all members in the house.
        /// </summary>
        public IEnumerable<Member> AllMembers => House.AllMembers;
    }
}
