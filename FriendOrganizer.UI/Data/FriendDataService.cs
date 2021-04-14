using FriendOrganizer.Model;
using System.Collections.Generic;

namespace FriendOrganizer.UI.Data
{
    public class FriendDataService : IFriendDataService
    {
        public IEnumerable<Friend> GetAll()
        {
            yield return new Friend { FirstName = "Carl", LastName = "Villanos", Email = "carl@test.com" };
            yield return new Friend { FirstName = "Henry", LastName = "Long", Email = "henry@test.com" };
            yield return new Friend { FirstName = "Yoh", LastName = "Hoho", Email = "yohhoho@test.com" };
            yield return new Friend { FirstName = "Ru", LastName = "Ffy", Email = "ruffy@test.com" };
        }
    }
}
