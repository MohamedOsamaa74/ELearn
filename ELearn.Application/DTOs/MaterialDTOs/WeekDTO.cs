using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.MaterialDTOs
{
    public class WeekDTO
    {
        public int Id { get; set; }
        public int WeekNumber { get; set; }
        public List<MaterialDTO> Lectures { get; set; } = [];
        public List<MaterialDTO> Sections { get; set; } = [];
    }
}
