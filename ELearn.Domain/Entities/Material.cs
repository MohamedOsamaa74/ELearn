namespace ELearn.Domain.Entities
{
    public class Material
    {
        public int Id { get; set; }
        public required string title { get; set; }
        public string? link { get; set; }
        public int Week { get; set; }
        public required int GroupId { get; set; }
        public required string UserId { get; set; }
        public required virtual ApplicationUser User { get; set; }
        public required virtual Group Group { get; set; }
    }
}