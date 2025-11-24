using Arohan.TMS.Application.Interfaces;
using Arohan.TMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Arohan.TMS.Infrastructure.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _db;
        public EfRepository(ApplicationDbContext db) => _db = db;

        public async Task AddAsync(T entity, CancellationToken ct = default) => await _db.Set<T>().AddAsync(entity, ct);

        public async Task<T?> GetByIdAsync(object id, CancellationToken ct = default) => await _db.Set<T>().FindAsync(new[] { id }, ct);

        public void Remove(T entity) => _db.Set<T>().Remove(entity);

        public void Update(T entity) => _db.Set<T>().Update(entity);

        public async Task<List<T>> ListAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default)
        {
            IQueryable<T> q = _db.Set<T>();
            if (predicate != null) q = q.Where(predicate);
            return await q.ToListAsync(ct);
        }

        public async Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        {
            return await _db.Set<T>().Where(predicate).SingleOrDefaultAsync(ct);
        }
    }
}
