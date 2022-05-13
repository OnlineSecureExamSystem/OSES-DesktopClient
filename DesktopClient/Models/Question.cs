namespace DesktopClient.Models
{
    public class Question
    {
        public string? Id { get; set; }
        public QuestionDescription? Description { get; set; }
        public QuestionAnswer? Answer { get; set; }
    }
}
