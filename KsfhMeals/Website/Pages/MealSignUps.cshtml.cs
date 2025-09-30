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
        public string? MemberID { get; set; }

        public void OnGet()
        {
            ViewData["ActivePage"] = "Sign Up/ View Meals";
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

        public List<SelectListItem> GetLunchItems(MealStatus status)
        {
            List<SelectListItem> items;
            if (status == MealStatus.In)
            {
                items = new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "In" },
                    new SelectListItem { Value = "2", Text = "Out" },
                    new SelectListItem { Value = "3", Text = "Early" },
                    new SelectListItem { Value = "4", Text = "Late" },
                    new SelectListItem { Value = "5", Text = "Tardy" }

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
                    new SelectListItem { Value = "4", Text = "Late" },
                    new SelectListItem { Value = "5", Text = "Tardy" }
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
                    new SelectListItem { Value = "4", Text = "Late" },
                    new SelectListItem { Value = "5", Text = "Tardy" }
                };
                return items;
            }
            else if (status == MealStatus.Late)
            {
                items = new List<SelectListItem>
                {
                    new SelectListItem { Value = "4", Text = "Late" },
                    new SelectListItem { Value = "1", Text = "In" },
                    new SelectListItem { Value = "2", Text = "Out" },
                    new SelectListItem { Value = "3", Text = "Early" },
                    new SelectListItem { Value = "5", Text = "Tardy" }
                };
                return items;
            }
            else
            {
                items = new List<SelectListItem>
                {
                    new SelectListItem { Value = "5", Text = "Tardy" },
                    new SelectListItem { Value = "4", Text = "Late" },
                    new SelectListItem { Value = "1", Text = "In" },
                    new SelectListItem { Value = "2", Text = "Out" },
                    new SelectListItem { Value = "3", Text = "Early" }
                };
                return items;
            }
        }

        public List<SelectListItem> GetDinnerItems(MealStatus status)
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
                else if (value == "4")
                {
                    MemberToShow.TempSignUp[i] = MealStatus.Late;
                }
                else
                {
                    MemberToShow.TempSignUp[i] = MealStatus.Tardy;
                }
  
            }
            House.Save();
            SaveConfirmationMessage = "Your meal sign-up has been saved successfully!";
            return Page();
        }

        
    }
}
