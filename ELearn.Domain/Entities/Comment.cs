using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ELearn.Domain.Entities
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Text { get; set; }
        public DateTime Date { get; set; }
        //UserId

        //PostId
        public required int PostId { get; set; }
        public virtual required Post Post { get; set; }
        public string UserId { get; set; }//CreatorId

        public virtual required ApplicationUser User { get; set; }

    }

}
