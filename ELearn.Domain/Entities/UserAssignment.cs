using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Domain.Entities
{
    public class UserAssignment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int AssignmentId { get; set; }
        public DateTime DateAnswered {get; set; }
        public virtual Assignment Assignment { get; set; }
        public virtual ApplicationUser Users { get; set; }
    }
}
