using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
//Hunter was here
namespace Website.Pages
{
    public class AdminModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string? ML { get; set; }


        [BindProperty(SupportsGet = true)]
        public string? MD { get; set; }


        [BindProperty(SupportsGet = true)]
        public string? TL { get; set; }


        [BindProperty(SupportsGet = true)]
        public string? TD { get; set; }


        [BindProperty(SupportsGet = true)]
        public string? WL { get; set; }


        [BindProperty(SupportsGet = true)]
        public string? WD { get; set; }


        [BindProperty(SupportsGet = true)]
        public string? THL { get; set; }


        [BindProperty(SupportsGet = true)]
        public string? THD { get; set; }


        [BindProperty(SupportsGet = true)]
        public string? FL { get; set; }


        [BindProperty(SupportsGet = true)]
        public string? FD { get; set; }


        [BindProperty(SupportsGet = true)]
        public string? SA { get; set; }


        [BindProperty(SupportsGet = true)]
        public string? SU { get; set; }

        public string? SaveScheduleMessage { get; set; }

        public string? SaveRolloverMessage { get; set; }

        public string? SaveResetMessage { get; set; }
        public string? UpdateNMMessage { get; set; }

        public void OnGet()
        {
            ViewData["ActivePage"] = "Admin";
        }

        public void OnPost()
        {
            List<string> lunch = new List<string>();

            lunch.Add(ML!);
            lunch.Add(TL!);
            lunch.Add(WL!);
            lunch.Add(THL!);
            lunch.Add(FL!);
            lunch.Add(SA!);
            lunch.Add(SU!);

            List<string> dinner = new List<string>();

            dinner.Add(MD!);
            dinner.Add(TD!);
            dinner.Add(WD!);
            dinner.Add(THD!);
            dinner.Add(FD!);

            if (!lunch.Contains(null!) || !dinner.Contains(null!))
            {
                House.AddLunchItem(lunch);
                House.AddDinnerItem(dinner);

                SaveScheduleMessage = "Meal Scheudle has been saved successfully!";
            }
        }


        public void OnPostRollover()
        {
            foreach (Member m in House.AllMembers)
            {
                for (int i = 0; i < m.TempSignUp.Length; i++)
                {
                    bool isLunch = (i % 2 == 0) || (i >= 10); // weekday lunch OR weekend lunch
                    bool isDinner = (i % 2 == 1 && i < 10);   // only Mon–Fri dinners

                    if (m.HouseStatus == Status.OutOfHouse && m.TempSignUp[i] != MealStatus.Out )
                    {
                        if (isLunch) m.LunchCount++;
                        if (isDinner) m.DinnerCount++;
                    }

                    m.TempSignUp[i] = m.DefaultSignUp[i];
                }
            }

            House.Save();
            SaveRolloverMessage = "Meal Rollover has been saved successfully!";
        }

        public IActionResult OnPostResetMeals()
        {
            foreach (Member m in House.AllMembers)
            {
                if (m.HouseStatus == Status.InHouse || m.HouseStatus == Status.NewMember)
                    m.DefaultSignUp = Enumerable.Repeat(MealStatus.In, 12).ToArray();

                else
                {
                    m.DefaultSignUp = Enumerable.Repeat(MealStatus.Out, 12).ToArray();
                    m.DinnerCount = 0;
                    m.LunchCount = 0;
                }
            }

            House.Save();
            SaveResetMessage = "Meal Reset has been saved successfully!";

            return Page();
        }

        public void OnPostUpdateNM()
        {
            foreach (Member m in House.AllMembers)
            {
                if (m.HouseStatus == Status.NewMember)
                {
                    m.HouseStatus = Status.InHouse;
                }
            }

            House.Save();
            UpdateNMMessage = "All New Members are now In House Members";
        }
    }
}
