using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography.X509Certificates;

namespace Website.Pages
{
    public class MealSignUpsModel : PageModel
    {

        [BindProperty]
        public string? ID { get; set; }

        public Member? MemberToShow { get; set; }
        
        [HiddenInput]
        public string MemberID { get; set; }

        public void OnGet()
        {
        }

        public void OnPost() 
        {
            MemberToShow = GetMember(ID);
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
            return null;
        }

        public List<SelectListItem> GetItems(MealStatus status)
        {
            List<SelectListItem> items;
            if (status == MealStatus.In)
            {
                items = new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "In" },
                    new SelectListItem { Value = "2", Text = "Out" },
                    new SelectListItem { Value = "3", Text = "Early" },
                    new SelectListItem { Value = "4", Text = "Late" }
                };
                return items;
            }

            else if (status == MealStatus.Out)
            {
                items = new List<SelectListItem>
                {
                    new SelectListItem { Value = "2", Text = "Out" },
                    new SelectListItem { Value = "1", Text = "In" },
                    new SelectListItem { Value = "3", Text = "Early" },
                    new SelectListItem { Value = "4", Text = "Late" }
                };
                return items;
            }

            else if (status == MealStatus.Early)
            {
                items = new List<SelectListItem>
                {
                    new SelectListItem { Value = "3", Text = "Early" },
                    new SelectListItem { Value = "1", Text = "In" },
                    new SelectListItem { Value = "2", Text = "Out" },
                    new SelectListItem { Value = "4", Text = "Late" }
                };
                return items;
            }
            else
            {
                items = new List<SelectListItem>
                {
                    new SelectListItem { Value = "4", Text = "Late" },
                    new SelectListItem { Value = "1", Text = "In" },
                    new SelectListItem { Value = "2", Text = "Out" },
                    new SelectListItem { Value = "3", Text = "Early" }
                };
                return items;
            }
        }

        public string[] comboBoxNames = new string[] { "ML", "MD", "TL", "TD", "WL", "WD", "UL", "UD", "FL", "FD" };

        [BindProperty]
        public string? SelectedMealType { get; set; }

        public IActionResult OnPostEditSignUp()
        {
            MemberToShow = GetMember(ID);
            for (int i = 0; i < comboBoxNames.Length; i++)
            {
                string value = Request.Form[comboBoxNames[i]];

                if (value == "1")
                {
                    MemberToShow.TempSignUp[i] = MealStatus.In;
 
                }
                else if (value == "2")
                {
                    MemberToShow.TempSignUp[i] = MealStatus.Out;
                }
                else if (value == "3")
                {
                    MemberToShow.TempSignUp[i] = MealStatus.Early;
                }
                else
                {
                    MemberToShow.TempSignUp[i] = MealStatus.Late;
                }
            }
            House.Save();
            return Page();
        }

        
    }
}
