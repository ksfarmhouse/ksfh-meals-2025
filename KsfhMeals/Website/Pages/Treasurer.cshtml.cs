using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Website.Pages
{
    public class TreasurerModel : PageModel
    {
        public IEnumerable<Member> AllMembers => House.AllMembers;

        public void OnGet()
        {
            ViewData["ActivePage"] = "Treasurer";
        }
    }
}
