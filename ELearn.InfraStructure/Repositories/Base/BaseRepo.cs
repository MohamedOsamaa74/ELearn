using ELearn.Data;
using ELearn.Domain.Entities;
using ELearn.Domain.Interfaces.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Repositories.Base
{
    public class BaseRepo<T> : IBaseRepo<T> where T : class
    {
        #region props and constructures
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public BaseRepo(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        #endregion

        #region Methods
        
        public virtual async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteRangeAsync(ICollection<T> entities)
        {
            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Deleted;
            }
            await _context.SaveChangesAsync();
        }

        public virtual async Task<T> GetByIdAsync(int Id) => await _context.Set<T>().FindAsync(Id);
        public virtual async Task<T> GetByIdAsync(string Id) => await _context.Set<T>().FindAsync(Id);

        public async Task<IEnumerable<object>> GetWhereSelectAsync(Expression<Func<T, bool>> Condition, Expression<Func<T, object>> expression)
        => await _context.Set<T>().Where(Condition).Select(expression).ToListAsync();

        public virtual async Task<IEnumerable<object>> GetAllAsync(Expression<Func<T, object>> Selected)
            => await _context.Set<T>().Select(Selected).ToListAsync();
        
        public virtual async Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> Condition)
         => await _context.Set<T>().Where(Condition).ToListAsync();

        
        public virtual async Task<bool> FindIfExistAsync(Expression<Func<T, bool>> Condition) => await _context.Set<T>().AnyAsync(Condition);

        public virtual async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public virtual async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
             await _context.SaveChangesAsync();
        }
       
        /*public async Task<string> UploadFileAsync(IFormFile file, string folderPath)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File not selected or empty.");

            // Create the folder if it doesn't exist
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Define the file path within the folder
            var filePath = Path.Combine(folderPath, file.FileName);

            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }               
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

              
                var filePath = Path.Combine(folderPath, file.FileName);

               
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            return filePath;
        }

      */
        public async Task<string> UploadFileAsync(IFormFile file, string folderPath)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File not selected or empty.");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }


            var filePath = Path.Combine(folderPath, file.FileName);


            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }
        public void Commit() => _context.Database.CommitTransaction();

        public void RollBack() => _context.Database.RollbackTransaction();

        public async Task<ApplicationUser> GetCurrentUserAsync(ClaimsPrincipal User)
        {
            return await _userManager.FindByNameAsync(User.Identity.Name);
        }

        #endregion
    }
}
