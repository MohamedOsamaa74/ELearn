using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Domain.Entities
{
    public class UserGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string UserId { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow.ToLocalTime();
        public virtual Group Group { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
