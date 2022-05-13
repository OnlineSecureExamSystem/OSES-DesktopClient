using Avalonia;
using Avalonia.Controls.Primitives;

namespace DesktopClient.CustomControls;

public class Question : TemplatedControl
{
    public static readonly StyledProperty<object> DescriptionProperty = AvaloniaProperty.Register<Question, object>("Description");

    public object Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public static readonly StyledProperty<object> AnswerProperty = AvaloniaProperty.Register<Question, object>("Answer");

    public object Answer
    {
        get => GetValue(AnswerProperty);
        set => SetValue(AnswerProperty, value);
    }
    public Question()
    {
        DataContext = this;
    }

    public Question(object description, object answer)
    {
        Description = description;
        Answer = answer;
    }
}