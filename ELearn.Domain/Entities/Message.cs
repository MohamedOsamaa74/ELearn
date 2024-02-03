using System.ComponentModel.DataAnnotations.Schema;

namespace ELearn.Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public required string SenderId { get; set; }
        public required string ReceiverId { get; set; }
        public required string Text { get; set; }
        public required DateTime Date { get; set; }
        public virtual ApplicationUser Sender { get; set; }
        public virtual ApplicationUser Receiver { get; set; }

    }
}