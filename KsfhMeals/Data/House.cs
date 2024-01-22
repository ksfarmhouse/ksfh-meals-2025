using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Data
{
    public static class House
    {

        private static List<string> _lunch;
        private static List<string> _dinner;

        private static List<Member> _members;


        static House()
        {
            if (File.Exists("lunch.json"))
            {
                string Ljson = File.ReadAllText("lunch.json");
                string Djson = File.ReadAllText("dinner.json");

                _lunch = JsonConvert.DeserializeObject<List<string>>(Ljson);
                _dinner = JsonConvert.DeserializeObject<List<string>>(Djson);
            }
            else
            {
                _lunch = new List<string>();
                _dinner = new List<string>();
            }

            if (File.Exists("members.json"))
            {
                string membersJson = File.ReadAllText("members.json");
                

                _members = JsonConvert.DeserializeObject<List<Member>>(membersJson);
            }
            else
            {
                _members = new List<Member>();
            }
        }

        public static IEnumerable<Member> AllMembers => _members;
        public static IEnumerable<string> Lunch => _lunch;
        public static IEnumerable<string> Dinner => _dinner;

        public static void AddLunchItem(List<string> s)
        {
            _lunch = s;
            
            File.WriteAllText("lunch.json", JsonConvert.SerializeObject(Lunch));  
        }
        public static void AddDinnerItem(List<string> s)
        {
            _dinner = s;
            File.WriteAllText("dinner.json", JsonConvert.SerializeObject(Dinner));
        }

        public static void AddMember(Member member)
        {
            _members.Add(member);
            _members = _members.OrderBy(m => m.LastName).ToList();
            File.WriteAllText("members.json", JsonConvert.SerializeObject(AllMembers));
        }

        public static void RemoveMember(string id)
        {
            foreach (Member member in _members) 
            {
                if (member.ID == id)
                {
					_members.Remove(member);
					_members = _members.OrderBy(m => m.LastName).ToList();
					File.WriteAllText("members.json", JsonConvert.SerializeObject(AllMembers));
                    break;
				}
            }
            
		}

        public static void Save()
        {
            File.WriteAllText("Members.json", JsonConvert.SerializeObject(AllMembers));
        }

    }
}
