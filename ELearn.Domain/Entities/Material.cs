namespace ELearn.Domain.Entities
{
    public class Material
    {
        public int MaterialId { get; set; }
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public required string title { get; set; }
        public required string link { get; set; }
    }
}