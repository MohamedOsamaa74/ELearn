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
        public DateTime CreationDate { get; set; } = DateTime.UtcNow.ToLocalTime();
        //public required ReactType Type { get; set; }
        
        #region ForeignKey
        public int? PostID { get; set; }
        public int? CommentId { get; set; }
        public required string UserID { get; set; }
        #endregion

        #region Navigation Property
        public virtual ApplicationUser User { get; set; }
        public virtual Post Post { get; set; }
        public virtual Comment Comment { get; set; }
        #endregion
    }
}