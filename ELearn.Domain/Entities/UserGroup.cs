using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Domain.Entities
{
    public class UserGroup
    {
        [Key]
        public int UserGroupId { get; set; }
        public int GroupId { get; set; }
        public string UserId { get; set; }
        public virtual Group Group { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
