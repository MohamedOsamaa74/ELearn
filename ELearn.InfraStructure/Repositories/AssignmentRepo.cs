using ELearn.Data;
using ELearn.Domain.Entities;
using ELearn.Domain.Interfaces;
using ELearn.InfraStructure.Repositories.Base;
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

        public AssignmentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
