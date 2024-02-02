namespace ELearn.Domain.Entities
{
    public class UserVoting
    {
    
        public string userId { get; set; }
        public ApplicationUser User { get; set; }
        public int VotingId { get; set; }
        public Voting Voting { get; set; }
        public int OptionsId { get; set; }
    }
}