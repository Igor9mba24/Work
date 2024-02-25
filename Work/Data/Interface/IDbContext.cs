using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using Work.Models;

namespace Work.Data.Interface
{
    public interface IDbContext
    {
        DbSet<Person> Persons { get; set; }
        DbSet<Skill_Person> Skill_Person { get; set; }
        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        Task AddAsync<T>(T entity) where T : class;
        DatabaseFacade Database { get; } // Добавленный метод для доступа к базе данных
    }
}
