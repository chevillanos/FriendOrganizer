using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Repositories
{
    public class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        protected readonly TContext context;

        protected GenericRepository(TContext context)
        {
            this.context = context;
        }

        public void Add(TEntity model)
        {
            context.Set<TEntity>().Add(model);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await context.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<TEntity> GetIdByAsync(int id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }

        public bool HasChanges()
        {
            return context.ChangeTracker.HasChanges();
        }

        public void Remove(TEntity model)
        {
            context.Set<TEntity>().Remove(model);
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
