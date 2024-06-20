namespace ELearn.Application.DTOs.VotingDTOs
{
    public class UserVotingDTO
    {
        public required string FullName { get; set; }
        public required string Voting { get; set; }
        public required string Option { get; set; }
        public DateTime? Created { get; set; }
    }
}
