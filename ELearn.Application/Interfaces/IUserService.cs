using ELearn.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.Interfaces
{
    public interface IUserService
    {
        public Task<IEnumerable<ApplicationUser>> UploadCSV(IFormFile file);
        public Task<bool> CreateNewUser(object user);
    }
}
