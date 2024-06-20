using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.MessageDTOs
{
    public class SendMessageDTO
    {
        public string Text { get; set; }
        public IFormFile? File { get; set; }
        public required string ReceiverId { get; set; }

    }
}
