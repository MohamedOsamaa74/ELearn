using ELearn.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs
{
    public  class VotingDTO
    {
        public required string Text { get; set; }
        public DateTime CreateDate { get; set; }
        public required Duration Duration { get; set; }
        public ICollection<Group> groups { get; set; }
    }
}
