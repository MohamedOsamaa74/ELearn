using System.ComponentModel.DataAnnotations.Schema;

namespace ELearn.Domain.Entities
{
    public class Message
    {
        public int MessageId { get; set; }
        //senderid
        //[ForeignKey("Sender")]
        public required string SenderId { get; set; }
        public required string ReceiverId { get; set; }
        public required string Text { get; set; }
        public required DateTime Date { get; set; }=DateTime.Now;
        public virtual ApplicationUser Sender { get; set; }
        public virtual ApplicationUser Receiver { get; set; }

    }
}