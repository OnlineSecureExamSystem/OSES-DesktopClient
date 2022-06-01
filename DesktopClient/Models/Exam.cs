using System;
using System.Collections.Generic;

namespace DesktopClient.Models
{
    public class Exam
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? ExamCode { get; set; }
        public TimeSpan Duration { get; set; }
        public string? Description { get; set; }
        public List<Exercise>? Exercises { get; set; }


    }
}
