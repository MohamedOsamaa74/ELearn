using ELearn.Data;
using ELearn.Domain.Entities;
using ELearn.Domain.Interfaces;
using ELearn.Domain.Interfaces.Base;
using ELearn.InfraStructure.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Repositories
{
    public class AnnouncmentRepository : BaseRepo<Announcement>, IAnnouncmentRepo
    {
        private readonly AppDbContext _context;

        public AnnouncmentRepository(AppDbContext context) : base(context)
        {
        }
        
    }
}
