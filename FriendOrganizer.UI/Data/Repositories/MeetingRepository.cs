using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;

namespace FriendOrganizer.UI.Data.Repositories
{
    public class MeetingRepository : GenericRepository<Meeting, FriendOrganizerDbContext>, 
        IMeetingRepository
    {
        public MeetingRepository(FriendOrganizerDbContext context) : base(context)
        {

        }

        public async Task<List<Friend>> GetAllFriendsAsync()
        {
            return await context.Set<Friend>()
                .ToListAsync();
        }

        public async override Task<Meeting> GetIdByAsync(int id)
        {
            return await context.Meetings.Include(m => m.Friends)
                 .SingleAsync(m => m.Id == id);
        }

        public async Task ReloadFriendAsync(int friendId)
        {
            var dbEntityEntry = context.ChangeTracker.Entries<Friend>()
                .SingleOrDefault(db => db.Entity.Id == friendId);

            if (dbEntityEntry != null)
                await dbEntityEntry.ReloadAsync();
        }
    }
}
