using System.ComponentModel.DataAnnotations;

namespace ELearn.Domain.Entities
{
    public class Department
    {
        public int Department_id { get; set; }

        public required string title { get; set; }
    }
}