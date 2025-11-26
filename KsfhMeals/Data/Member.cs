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
        public MealStatus[] TempMealSignUp { get; set; } = new MealStatus[12];
        
        public MealStatus[] DefaultSignUp { get; set; } = new MealStatus[12];

        public Member(string iD, string firstName, string lastName, Status houseStatus)
        {
            MealStatus[] defaultSignUp;
            if(houseStatus == Status.InHouse || houseStatus == Status.NewMember) 
            {
                defaultSignUp = Enumerable.Repeat(MealStatus.In, 12).ToArray();
            }
            else
            {
                defaultSignUp = Enumerable.Repeat(MealStatus.Out, 12).ToArray();
            }
            ID = iD;
            FirstName = firstName;
            LastName = lastName;
            FullName = firstName + " " + lastName;
            HouseStatus = houseStatus;
            DinnerCount = 0;
            LunchCount = 0;
			DefaultSignUp = defaultSignUp;
            TempMealSignUp = defaultSignUp;
            
        }
    }
}