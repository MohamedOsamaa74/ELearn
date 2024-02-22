using ELearn.Data;
using ELearn.Domain.Entities;
using ELearn.Domain.Interfaces.Base;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _webRootPath;
        public BaseRepo(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _webRootPath = _webHostEnvironment.WebRootPath;
        }
           
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

       



        #region
        public async Task<string> UploadFile(IFormFile formFile, string folderName)
        {
            if (formFile == null || formFile.Length == 0)
                throw new ArgumentNullException(nameof(formFile), "Form file is null or empty.");

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
            string uploadDirectory = Path.Combine(_webRootPath, folderName);

            Directory.CreateDirectory(uploadDirectory);

            string filePath = Path.Combine(uploadDirectory, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            string url = $"/{folderName}/{fileName}";

            return url;
        }

        #endregion
        public void Commit() => _context.Database.CommitTransaction();

            public void RollBack() => _context.Database.RollbackTransaction();

            #endregion
        } 
}
