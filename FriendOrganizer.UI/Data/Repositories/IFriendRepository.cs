using FriendOrganizer.Model;
using FriendOrganizer.UI.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Repositories
{
    public interface IFriendRepository
    {
        Task<Friend> GetIdByAsync(int friendId);
        Task SaveAsync();
        bool HasChanges();
        void Add(Friend friend);
        void Remove(Friend friend);
        void RemovePhoneNumber(FriendPhoneNumber selectedPhoneNumber);
    }
}