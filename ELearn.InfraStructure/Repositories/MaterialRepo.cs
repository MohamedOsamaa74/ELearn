using ELearn.Data;
using ELearn.Domain.Entities;
using ELearn.Domain.Interfaces;
using ELearn.Domain.Interfaces.Base;
using ELearn.InfraStructure.Repositories.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Repositories
{
    public class MaterialRepository : BaseRepo<Material>, IMaterialRepo
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MaterialRepository(AppDbContext context, UserManager<ApplicationUser> userManager) : base(context, userManager)
        {
        }

    }
}
