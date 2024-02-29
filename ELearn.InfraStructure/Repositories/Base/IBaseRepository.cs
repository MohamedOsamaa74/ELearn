using ELearn.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Repositories.Base
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdAsync(string id);
        Task<IEnumerable<object>> GetAllAsync(Expression<Func<T, object>> Selected);
        Task<bool> FindIfExistAsync(Expression<Func<T, bool>> Condition);
        Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> expression);
        Task<IEnumerable<TResult>> GetWhereSelectAsync<TResult>(Expression<Func<T, bool>> Condition, Expression<Func<T, TResult>> expression);
        // Get By Department Id   
        // Get X from Multiple Groups 1
        // Get x From Group Y
        Task<ApplicationUser> GetCurrentUserAsync(ClaimsPrincipal User);
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(ICollection<T> entities);
        Task<string> UploadFileAsync(IFormFile file, string folderPath);
        Task SaveChangesAsync();
        //Task<string> UploadFile(IFormFile formFile, string x);
        void Commit();
        void RollBack();
    }
}
