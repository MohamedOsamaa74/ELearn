using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.CommentDTOs
{
    public class ViewCommentDTO
    {
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatorName { get; set; }
    }
}
