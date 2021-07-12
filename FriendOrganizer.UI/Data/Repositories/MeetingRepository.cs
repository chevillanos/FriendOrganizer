using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System.Threading.Tasks;
using System.Data.Entity;

namespace FriendOrganizer.UI.Data.Repositories
{
    public class MeetingRepository : GenericRepository<Meeting, FriendOrganizerDbContext>, 
        IMeetingRepository
    {
        public MeetingRepository(FriendOrganizerDbContext context) : base(context)
        {

        }

        public async override Task<Meeting> GetIdByAsync(int id)
        {
            return await context.Meetings.Include(m => m.Friends)
                 .SingleAsync(m => m.Id == id);
        }
    }
}
