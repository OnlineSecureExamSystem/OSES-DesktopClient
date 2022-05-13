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
        public List<Question>? Questions { get; set; }
    }
}
