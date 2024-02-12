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
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Repositories.Base
{
    public class BaseRepo<T> : IBaseRepo<T> where T : class
    {
        #region props and constructures
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public BaseRepo(AppDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        
        public virtual async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task AddRangeAsync(ICollection<T> entities)
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

        public virtual async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();
        public virtual async Task<IEnumerable<T>> GetByNameAsync(Expression<Func<T, bool>> expression, string name)
         => await _context.Set<T>().Where(expression).ToListAsync();

        public virtual async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public virtual async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public void Commit() => _context.Database.CommitTransaction();

        public void RollBack() => _context.Database.RollbackTransaction();


        #endregion
    }
}
