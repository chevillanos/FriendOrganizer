using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System.Data.Entity;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Repositories
{
    public class FriendRepository : GenericRepository<Friend, FriendOrganizerDbContext>, IFriendRepository
    {
        public FriendRepository(FriendOrganizerDbContext context)
            : base(context)
        {
        }

        public override async Task<Friend> GetIdByAsync(int friendId)
        {
            return await context.Friends
                .Include(f => f.PhoneNumbers)
                .SingleAsync(f => f.Id == friendId);
        }

        public void RemovePhoneNumber(FriendPhoneNumber selectedPhoneNumber)
        {
            context.FriendPhoneNumbers.Remove(selectedPhoneNumber);
        }
    }
}
