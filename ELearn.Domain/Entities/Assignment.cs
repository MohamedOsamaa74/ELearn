using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearn.Domain.Entities
{
    public class Assignment
    {
        public int Id { get; set; }
        public  string Title { get; set; }
        public  DateTime Date { get; set; }
        public  Duration Duration { get; set; }
      
        public string FilePath { get; set; }
        //UserId
        [Required]
        public  string UserId { get; set; }//CreatorId

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