using System.Collections.Generic;

namespace DesktopClient.Models
{
    public class Exercise
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public List<Question>? Questions { get; set; }
    }
}
