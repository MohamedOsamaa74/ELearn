using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.MessageDTOs
{
    public class ViewMessageDTO
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public DateTime CreationDate { get; set; }
        public string? url { get; set; }
        public string SenderId { get; set; }


    }
}
