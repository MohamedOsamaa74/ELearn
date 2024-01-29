namespace ELearn.Domain.Entities
{
    public class Material
    {
        public int MaterialId { get; set; }
        public required int GroupId { get; set; }
        public required int UserId { get; set; }
        public required string title { get; set; }
        public required string link { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Group Group { get; set; }
    }
}