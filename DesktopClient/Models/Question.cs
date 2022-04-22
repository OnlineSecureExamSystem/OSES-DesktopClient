using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Models
{
    public class Question
    {
        public string Type { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }


        // constructor with parameters for json deserialization
        public Question(int type, string? description, string? content)
        {
            Type = type.ToString();
            Description = description;
            Content = content;
        }

        public Question()
        {
                
        }
    }
}
    