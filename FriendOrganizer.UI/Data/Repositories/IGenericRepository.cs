using System.Collections.Generic;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Repositories
{
    public interface IGenericRepository<T>
    {
        Task<T> GetIdByAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task SaveAsync();
        bool HasChanges();
        void Add(T model);
        void Remove(T model);
    }
}