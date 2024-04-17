using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearn.Domain.Entities
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Text { get; set; }
        public DateTime Date => DateTime.UtcNow.ToLocalTime();

        #region ForeignKeys
        public required string SenderId { get; set; }
        public required string ReceiverId { get; set; }
        #endregion

        #region NavigationProperties
        public virtual ApplicationUser Sender { get; set; }
        public virtual ApplicationUser Receiver { get; set; }
        public FileEntity? File { get; set; }
        #endregion

    }
}