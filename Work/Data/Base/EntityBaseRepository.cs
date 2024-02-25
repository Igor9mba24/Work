using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Work.Context;

namespace Work.Data.Base
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T> where T : class, IEntityBase, new()
    { 
private readonly ApplictaionDbContext _appDbContext;
    public EntityBaseRepository(ApplictaionDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task AddAsync(T entity)
    {
        await _appDbContext.Set<T>().AddAsync(entity);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _appDbContext.Set<T>().ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _appDbContext.Set<T>();

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        return await query.ToListAsync();
    }

    public async Task<T> GetByIdAsync(long id)
    {
        return await _appDbContext.Set<T>().FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task RemoveAsync(long id)
    {
        var entity = await GetByIdAsync(id);
        EntityEntry entry = _appDbContext.Entry<T>(entity);
        entry.State = EntityState.Deleted;
        await _appDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(long id, T entity)
    {
        EntityEntry entry = _appDbContext.Entry<T>(entity);
        entry.State = EntityState.Modified;
        await _appDbContext.SaveChangesAsync();
    }
}
}
