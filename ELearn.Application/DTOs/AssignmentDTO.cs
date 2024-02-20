using ELearn.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs
{
    public class AssignmentDTO
    {
        public int Id { get; set; }
        public  string Title { get; set; }
        public  DateTime Date { get; set; }
        public  Duration Duration { get; set; }
       
        public required IFormFile File { get; set; }

       

       
    }
}
