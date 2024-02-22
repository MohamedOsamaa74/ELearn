using ELearn.Data;
using ELearn.Domain.Entities;
using ELearn.Domain.Interfaces;
using ELearn.InfraStructure.Repositories.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Repositories
{
    public class AssignmentRepository : BaseRepo<Assignment>, IAssignmentRepo
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public AssignmentRepository(AppDbContext context, UserManager<ApplicationUser> userManager) : base(context, userManager)
        {
        }
    }
}
