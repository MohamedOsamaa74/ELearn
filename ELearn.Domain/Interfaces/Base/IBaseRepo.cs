using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Domain.Interfaces.Base
{
    public interface IBaseRepo<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdAsync(string id);
        Task<IEnumerable<object>> GetAllAsync(Expression<Func<T, object>> Selected);
        Task<bool> FindIfExistAsync(Expression<Func<T, bool>> Condition);
        Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> expression);
        Task<IEnumerable<object>> GetWhereSelectAsync(Expression<Func<T, bool>> Condition, Expression<Func<T, object>> expression);
        // Get By Department Id
        // Get X from Multiple Groups 1
        // Get x From Group Y
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        //Create x To Multiple Groups 2
        //Task AddToGroupsAsync(ICollection<T> Groups, T Entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(ICollection<T> entities);
        Task SaveChangesAsync();
        void Commit();
        void RollBack();
    }
}
