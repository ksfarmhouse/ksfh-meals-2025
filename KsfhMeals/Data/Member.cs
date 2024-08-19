namespace Data
{
    public class Member
    {
        public string ID { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }

        public string FullName { get; init; }
        public Status HouseStatus { get; set; }
        public int DinnerCount { get; set; } 
        public int LunchCount { get; set; }
        public MealStatus[] TempSignUp { get; set; }
        public MealStatus[] DefaultSignUp { get; set; }

        public Member(string iD, string firstName, string lastName, Status houseStatus)
        {
            MealStatus[] defaultSignUp;
            if(houseStatus == Status.InHouse || houseStatus == Status.NewMember) 
            {
                defaultSignUp = new MealStatus[] { MealStatus.In, MealStatus.In, MealStatus.In, MealStatus.In, MealStatus.In, MealStatus.In, MealStatus.In, MealStatus.In, MealStatus.In, MealStatus.In, };
            }
            else
            {
                defaultSignUp = new MealStatus[] { MealStatus.Out, MealStatus.Out, MealStatus.Out, MealStatus.Out, MealStatus.Out, MealStatus.Out, MealStatus.Out, MealStatus.Out, MealStatus.Out, MealStatus.Out, };
            }
            ID = iD;
            FirstName = firstName;
            LastName = lastName;
            FullName = firstName + " " + lastName;
            HouseStatus = houseStatus;
            DinnerCount = 0;
            LunchCount = 0;
			DefaultSignUp = defaultSignUp;
			TempSignUp = defaultSignUp;
            
        }
    }
}