using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

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
        public string? UL { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? UD { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? FL { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? FD { get; set; }
        public void OnGet()
        {
        }

        public void OnPost()
        {
            List<string> lunch = new List<string>();

            lunch.Add(ML);
            lunch.Add(TL);
            lunch.Add(WL);
            lunch.Add(UL);
            lunch.Add(FL);

            List<string> dinner = new List<string>();

            dinner.Add(MD);
            dinner.Add(TD);
            dinner.Add(WD);
            dinner.Add(UD);
            dinner.Add(FD);

            if (!lunch.Contains(null) || !dinner.Contains(null))
            {
                House.AddLunchItem(lunch);
                House.AddDinnerItem(dinner);
            }
		}

        public void OnPostRollover()
        {
			foreach (Member m in House.AllMembers)
			{
				for (int i = 0; i < m.TempSignUp.Count(); i++)
				{
                    if (i % 2 == 0)
                    {
						if ((m.HouseStatus == Status.OutOfHouse || m.HouseStatus == Status.Alumni) && m.TempSignUp[i] == MealStatus.In)
                        {
                            m.LunchCount++;
                        }
					}
                    else
                    {
						if ((m.HouseStatus == Status.OutOfHouse || m.HouseStatus == Status.Alumni) && m.TempSignUp[i] == MealStatus.In)
						{
							m.DinnerCount++;
						}
					}
                    
					m.TempSignUp[i] = m.DefaultSignUp[i];
				}
			}
            House.Save();
		}
    }
}
