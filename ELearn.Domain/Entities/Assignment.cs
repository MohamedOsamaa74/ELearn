using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearn.Domain.Entities
{
    public class Assignment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public  string Title { get; set; }
        public  DateTime CreationDate { get; set; }
        public required DateTime Start { get; set; }
        public required DateTime End { get; set; }
        public string FilePath { get; set; }
        public  required string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        //many tasks in one group
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
        //many to many task 
        public List<UserAssignment> UserAssignment { get; set; }

        public ICollection<ApplicationUser> users { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }

    }
}