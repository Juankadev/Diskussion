using Diskussion.Models;
using Diskussion.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Diskussion.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DiskussionDbContext _context;
        protected DbSet<T> _dbSet;

        protected GenericRepository(DiskussionDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            return query;
        }


        public async Task<T?> GetById(long? id) 
            => await _dbSet.FindAsync(id);

        public async Task<T> Insert(T entity)
        {
            var result = await _dbSet.AddAsync(entity);
            return result.Entity;
        }

        //public async Task<bool> SoftDelete(long id)
        //{
        //    T? entity = await GetById(id);
        //    if (entity == null) return false;

        //    //entity.State = false;
        //    Update(entity);

        //    return true;
        //}

        public async Task<bool> HardDelete(long? id)
        {
            T? entity = await GetById(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            return true;
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
