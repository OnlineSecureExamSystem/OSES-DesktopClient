using Avalonia;
using Avalonia.Controls.Primitives;

namespace DesktopClient.CustomControls
{
    public class Exercise : TemplatedControl
    {
        public static readonly StyledProperty<string> TitleProperty = AvaloniaProperty.Register<Question, string>("Title");

        public string Title
        {
            get => GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly StyledProperty<string> DescriptionProperty = AvaloniaProperty.Register<Question, string>("Description");

        public string Description
        {
            get => GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public static readonly StyledProperty<object> QuestionsProperty = AvaloniaProperty.Register<Question, object>("Questions");

        public object Questions
        {
            get => GetValue(QuestionsProperty);
            set => SetValue(QuestionsProperty, value);
        }

        public Exercise()
        {
            DataContext = this;
        }

        public Exercise(string title, string description, object questions)
        {
            Title = title;
            Description = description;
            Questions = questions;
        }
    }
}
