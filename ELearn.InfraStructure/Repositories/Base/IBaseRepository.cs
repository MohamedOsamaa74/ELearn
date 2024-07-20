using System.Linq.Expressions;

namespace ELearn.InfraStructure.Repositories.Base
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdAsync(string id);
        Task<List<T>> GetAllAsync();
        Task<IEnumerable<object>> GetAllAsync(Expression<Func<T, object>> Selected);
        Task<bool> FindIfExistAsync(Expression<Func<T, bool>> Condition);
        Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> expression);
        Task<T> GetWhereSingleAsync(Expression<Func<T, bool>> expression);
        Task<List<TResult>> GetWhereSelectAsync<TResult>(Expression<Func<T, bool>> Condition, Expression<Func<T, TResult>> expression);
        Task<int> CountAsync(Expression<Func<T, bool>> expression);
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(ICollection<T> entities);
        Task SaveChangesAsync();
        void Commit();
        void RollBack();
    }
}
