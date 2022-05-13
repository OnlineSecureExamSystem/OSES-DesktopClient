using System.Collections.Generic;

namespace DesktopClient.Models
{
    public class QuestionAnswer
    {
        public int Type { get; set; }
        public List<AnswerItem>? Content { get; set; }
    }
}
