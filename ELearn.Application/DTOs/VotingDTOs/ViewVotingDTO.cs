using ELearn.Application.DTOs.OptionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.VotingDTOs
{
    public class ViewVotingDTO
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public required string CreatorName { get; set; }
        public bool IsActive { get; set; }
        public required ICollection<int> Groups { get; set; }
        public required ICollection<OptionPercentageDTO> OptionPercentages { get; set; }
        //public required string Option1 { get; set; }
        //public required string Option2 { get; set; }
        //public string? Option3 { get; set; }
        //public string? Option4 { get; set; }
        //public string? Option5 { get; set; }
    }
}