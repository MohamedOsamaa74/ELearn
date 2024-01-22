namespace ELearn.Domain.Entities
{
    public class Message
    {
        public int MessageId { get; set; }
        //senderid
        public int SenderId { get; set; }
        public int RecepientId { get; set; }

    }
}