using ELearn.Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace ELearn.Domain.Entities
{
    public class React
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int PostID { get; set; }
        public string UserID { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow.ToLocalTime();
        public required ReactType Type { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Post Post { get; set; }
        

    }
}