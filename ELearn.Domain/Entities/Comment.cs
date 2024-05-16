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
        public DateTime CreationDate { get; set; } = DateTime.UtcNow.ToLocalTime();

        #region ForeignKeys
        public required int PostId { get; set; }
        public required string UserId { get; set; }//CreatorId
        #endregion

        #region NavigationProperties
        public virtual Post Post { get; set; }
        public virtual ApplicationUser User { get; set; }
        public FileEntity? File { get; set; }
        #endregion
    }
}