using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.PostDTOs
{
    public class ViewPostDTO
    {
        public string CreatorName { get; set; }
        public DateTime CreationDate { get; set; }
        public string Text { get; set; }
        public ICollection<string> urls { get; set; }

        //public int ReactsCount { get; set; }
        //public int CommentsCount { get; set; }

    }
}
