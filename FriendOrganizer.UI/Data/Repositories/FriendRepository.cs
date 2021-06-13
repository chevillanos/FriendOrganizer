using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Repositories
{
    public class FriendRepository : IFriendRepository
    {
        private readonly FriendOrganizerDbContext context;

        public FriendRepository(FriendOrganizerDbContext context)
        {
            this.context = context;
        }

        public void Add(Friend friend)
        {
            context.Friends.Add(friend);
        }

        public async Task<Friend> GetIdByAsync(int friendId)
        {
            return await context.Friends.SingleAsync(f => f.Id == friendId);

        }

        public bool HasChanges()
        {
            return context.ChangeTracker.HasChanges();
        }

        public void Remove(Friend friend)
        {
            context.Friends.Remove(friend);
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
