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
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetByNameAsync(Expression<Func<T, bool>> expression, string name);
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(ICollection<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(ICollection<T> entities);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(ICollection<T> entities);
        Task SaveChangesAsync();
        void Commit();
        void RollBack();
    }
}
