﻿using ELearn.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs
{
    public class AssignmentDTO
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required DateTime Date { get; set; }
        public required Duration Duration { get; set; }
    }
}