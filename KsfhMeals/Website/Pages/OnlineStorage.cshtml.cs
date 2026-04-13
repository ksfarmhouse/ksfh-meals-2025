using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Website.Pages
{
    public class OnlineStorageModel : PageModel
    {
        [BindProperty]
        public string? ID { get; set; }

        [HiddenInput]
        public string? MemberID { get; set; }

        public string? ErrorMessage { get; set; }

        public void OnPost()
        {
            // Validate that the ID belongs to a real member
            Member? member = GetMember(ID!);
            if (member != null)
            {
                MemberID = ID;
            }
            else
            {
                ErrorMessage = "ID not found. Please try again.";
            }
        }

        private Member? GetMember(string id)
        {
            foreach (Member m in House.AllMembers)
            {
                if (m.ID == id)
                    return m;
            }
            return null;
        }
    }
}
