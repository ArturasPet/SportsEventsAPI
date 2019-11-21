using System.Collections.Generic;
using System.Linq;
using SportsEventsAPI.Models;

namespace SportsEventsAPI.Helpers
{
    public static class ExtensionMethods
    {
        public static List<User> WithoutPasswords(this List<User> users)
        {
            return users.Select(x => x.WithoutPassword()).ToList();
        }
        public static User WithoutPassword(this User user)
        {
            if (user != null)
                user.Password = null;
            return user;
        }
    }
}
