using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Domain.Entities
{
    public class UserAssignment
    {
        public int AssignmentId { get; set; }
        public string UserId { get; set; }
        public virtual Assignment Assignment { get; set; }
        public virtual ApplicationUser Users { get; set; }
    }
}
