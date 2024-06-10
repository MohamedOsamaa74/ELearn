using ELearn.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.MaterialDTOs
{
    public class MaterialDTO
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required int Week { get; set; }
        public required MaterialType Type { get; set; }
        public required string DownloadUrl { get; set; }
        public required string ViewUrl { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
