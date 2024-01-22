using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Website.Pages
{
    public class EditMembersModel : PageModel
    {
		public List<SelectListItem> Items { get; set; } = new List<SelectListItem>
			{
				new SelectListItem { Value = "1", Text = "In House" },
				new SelectListItem { Value = "2", Text = "Out Of House" },
				new SelectListItem { Value = "3", Text = "New Member" },
				new SelectListItem { Value = "4", Text = "Alumni" }
			};

		public List<SelectListItem> Statuses { get; set; }

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

		[BindProperty]
        public string? First { get; set; }

		[BindProperty]
		public string? Last { get; set; }

		[BindProperty]
		public string? ID { get; set; }

		[BindProperty]
		public string? SelectedStatus { get; set; }

		[BindProperty]
		public string? SelectedEditStatus { get; set; }

		public List<string> ComboBoxNames { get; set; } = new List<string>();

		public int Index { get; set; }

		public IActionResult OnPostNew() 
        {
			Status status = new Status();
			if (SelectedStatus == "1") status = Status.InHouse;
			if (SelectedStatus == "2") status = Status.OutOfHouse;
			if (SelectedStatus == "3") status = Status.NewMember;
			if (SelectedStatus == "4") status = Status.Alumni;
			Member newMember = new Member(ID, First, Last, status);
			House.AddMember(newMember);

			Items = new List<SelectListItem>
			{
				new SelectListItem { Value = "1", Text = "In House" },
				new SelectListItem { Value = "2", Text = "Out Of House" },
				new SelectListItem { Value = "3", Text = "New Member" },
				new SelectListItem { Value = "4", Text = "Alumni" }
			};
			
			foreach(Member m in House.AllMembers)
			{
				ComboBoxNames.Add("Member" + m.ID);
			}

			Index = 0;

			return Page();
		}

		public IActionResult OnPostEdit()
		{
			int i = 0;

			foreach (Member m in House.AllMembers) 
			{
				string value = Request.Form["Member" + m.ID];
				if (value == "1")
				{
					m.HouseStatus = Status.InHouse;
					m.DefaultSignUp = new MealStatus[] { MealStatus.In, MealStatus.In, MealStatus.In, MealStatus.In, MealStatus.In, MealStatus.In, MealStatus.In, MealStatus.In, MealStatus.In, MealStatus.In, };
				}
				else if (value == "2")
				{
					m.HouseStatus = Status.OutOfHouse;
					m.DefaultSignUp =  new MealStatus[] { MealStatus.Out, MealStatus.Out, MealStatus.Out, MealStatus.Out, MealStatus.Out, MealStatus.Out, MealStatus.Out, MealStatus.Out, MealStatus.Out, MealStatus.Out };
				}
				else if (value == "3")
				{
					m.HouseStatus = Status.NewMember;
					m.DefaultSignUp = new MealStatus[] { MealStatus.In, MealStatus.In, MealStatus.In, MealStatus.In, MealStatus.In, MealStatus.In, MealStatus.In, MealStatus.In, MealStatus.In, MealStatus.In, };
				}
				else
				{
					m.HouseStatus = Status.Alumni;
					m.DefaultSignUp = new MealStatus[] { MealStatus.Out, MealStatus.Out, MealStatus.Out, MealStatus.Out, MealStatus.Out, MealStatus.Out, MealStatus.Out, MealStatus.Out, MealStatus.Out, MealStatus.Out };
				}
				ComboBoxNames.Add("Member" + m.ID);
				i++;
			}

			House.Save();
			return Page();
		}

		public IEnumerable<Member> AllMembers => House.AllMembers;

		public List<SelectListItem> GetOtherStatus (Status status)
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
			return Page();
		}
    }
}
