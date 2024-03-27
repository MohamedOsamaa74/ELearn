using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Domain.Entities
{
    public class FileEntity
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string FilePath { get; set; }
        public required string Url { get; set; }
        public required string Type { get; set; }
        public DateTime Createion { get; set; } = DateTime.Now.ToLocalTime();

    }
}