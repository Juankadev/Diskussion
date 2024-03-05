using System.Linq.Expressions;

namespace Diskussion.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Insert(T entity);
        Task<T?> GetById(long? id);
        IQueryable<T> GetAll(Expression<Func<T, bool>> filter = null);
        void Update(T entity);
        //Task<bool> SoftDelete(long id);
        Task<bool> HardDelete(long? id);
        Task Save();
    }
}
