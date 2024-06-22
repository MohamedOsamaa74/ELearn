namespace ELearn.Application.DTOs.VotingDTOs
{
    public class AddVotingDTO
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime CreateDate => DateTime.UtcNow.ToLocalTime();
        public required DateTime End { get; set; }
        public required ICollection<int> groups { get; set; }
        public required ICollection<string> Options { get; set; }
    }
}