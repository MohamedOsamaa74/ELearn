using ELearn.Domain.Entities;
using ELearn.Domain.Interfaces.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Domain.Interfaces
{
    public interface IApplicationUserRepo : IBaseRepo<ApplicationUser>
    {
        public Task<IEnumerable<ApplicationUser>> UploadCSV(IFormFile file);
    }
}
