﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearn.Domain.Entities
{
    public class Assignment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Title { get; set; }
        public DateTime Start { get; set; } = DateTime.UtcNow.ToLocalTime();
        public required DateTime End { get; set; }
        public bool IsActivate => DateTime.UtcNow >= Start && DateTime.UtcNow <= End;

        #region ForeignKeys
        public required string UserId { get; set; }
        public int GroupId { get; set; }
        #endregion

        #region NavigationProperties
        public virtual ApplicationUser User { get; set; }
        //many tasks in one group
        public virtual Group Group { get; set; }
        //many to many task 
        public List<UserAssignment> UserAssignment { get; set; }
        public ICollection<ApplicationUser> users { get; set; }
        public ICollection<FileEntity> Files { get; } = new List<FileEntity>();
        #endregion
    }
}