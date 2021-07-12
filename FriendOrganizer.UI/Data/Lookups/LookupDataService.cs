using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Lookups
{
    public class LookupDataService : IFriendLookupDataService, IProgrammingLanguageLookupDataService,
        IMeetingLookupDataService
    {
        private readonly Func<FriendOrganizerDbContext> contextCreator;
        public LookupDataService(Func<FriendOrganizerDbContext> contextCreator)
        {
            this.contextCreator = contextCreator;
        }

        public async Task<IEnumerable<LookupItem>> GetFriendLookupAsync()
        {
            using (var ctx = contextCreator())
            {
                return await ctx.Friends.AsNoTracking()
                    .Select(f => new LookupItem
                    {
                        Id = f.Id,
                        DisplayMember = f.FirstName + " " + f.LastName
                    }).ToListAsync();
            }
        }

        public async Task<List<LookupItem>> GetMeetingLookupAsync()
        {
            using (var ctx = contextCreator())
            {
                var items = await ctx.Meetings.AsNoTracking()
                    .Select(m => new LookupItem
                    {
                        Id = m.Id,
                        DisplayMember = m.Title
                    })
                    .ToListAsync();

                return items;
            }
        }

        public async Task<IEnumerable<LookupItem>> GetProgrammingLanguageLookupAsync()
        {
            using (var ctx = contextCreator())
            {
                return await ctx.ProgrammingLanguages.AsNoTracking()
                    .Select(pl => new LookupItem
                    {
                        Id = pl.Id,
                        DisplayMember = pl.Name
                    }).ToListAsync();
            }
        }

        
    }
}
