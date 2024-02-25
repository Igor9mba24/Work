using System.Linq.Expressions;

namespace Work.Data.Base
{
    public interface IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetByIdAsync(long id);
        Task AddAsync(T entity);
        Task RemoveAsync(long id);
        Task UpdateAsync(long id, T entity);
    }
}
