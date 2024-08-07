﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Domain.Entities
{
    public class UserAnswerAssignment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? Grade { get; set; }
        public string? Comment { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow.ToLocalTime();

        #region ForeignKeys
        public required string UserId { get; set; }
        public required int AssignmentId { get; set; }
        #endregion
        
        #region NavigationProperties
        public virtual Assignment Assignment { get; set; }
        public virtual ApplicationUser Users { get; set; }
        public virtual ICollection<FileEntity> Files { get; } = new List<FileEntity>();
        #endregion
    }
}
