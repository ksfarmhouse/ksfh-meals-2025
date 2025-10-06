using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Website.Pages
{
    public class PermaSignUpModel : PageModel
    {

        [BindProperty]
        public string? ID { get; set; }

        public Member? MemberToShow { get; set; }

        [HiddenInput]
        public string? MemberID { get; set; }




        public void OnGet()
        {
            ViewData["ActivePage"] = "Permanent Sign Up";
        }

        public void OnPost()
        {
            MemberToShow = GetMember(ID!);
            MemberID = ID;
        }

        public Member GetMember(string id)
        {
            foreach (Member m in House.AllMembers)
            {
                if (m.ID == id)
                {
                    return m;
                }
            }
            return null!;
        }

        public string? SaveConfirmationMessage { get; set; }

        // Returns lunch items with the current status selected
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

        // Returns dinner items with the current status selected (no Tardy)
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

        [BindProperty]
        public string? SelectedMealType { get; set; }

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
                    _ => MemberToShow.DefaultSignUp[i] // fallback to existing value
                };

                MemberToShow.TempSignUp[i] = MemberToShow.DefaultSignUp[i];

                SaveConfirmationMessage = "Your meal sign-up has been saved successfully!";
            }
            
            House.Save();
            return Page();
        }
    }
}
