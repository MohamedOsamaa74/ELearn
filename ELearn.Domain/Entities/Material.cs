using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearn.Domain.Entities
{
    public class Material
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Link { get; set; }
        public int Week { get; set; }
        public string FilePath { get; set; }
        public required int GroupId { get; set; }
        public required string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Group Group { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
        //add date
    }
}