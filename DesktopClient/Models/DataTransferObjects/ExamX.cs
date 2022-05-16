using System;
using System.Collections.Generic;

namespace DesktopClient.Models.DataTransferObjects
{
    public class ExamX
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? ExamCode { get; set; }
        public TimeSpan Duration { get; set; }
        public string? Description { get; set; }
        public List<Question>? Questions { get; set; }

        public ExamX(Exam exam)
        {
            Id = exam.Id;
            Name = exam.Name;
            ExamCode = exam.ExamCode;
            Duration = exam.Duration;
            Description = exam.Description;
            Questions = GetQuestions(exam);
        }

        private List<Question> GetQuestions(Exam exam)
        {
            List<Question> questions = new List<Question>();
            foreach (var exercise in exam.Exercises)
            {
                foreach (var question in exercise.Questions)
                {
                    questions.Add(question);
                }
            }
            return questions;
        }
    }
}
