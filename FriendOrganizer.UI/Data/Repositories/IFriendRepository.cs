using FriendOrganizer.Model;
using FriendOrganizer.UI.Wrapper;
using System.Collections.Generic;

namespace FriendOrganizer.UI.Data.Repositories
{
    public interface IFriendRepository : IGenericRepository<Friend>
    {
        void RemovePhoneNumber(FriendPhoneNumber selectedPhoneNumber);
    }
}