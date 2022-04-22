using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace DesktopClient.CustomControls;

public class Question : TemplatedControl
{
    public static readonly StyledProperty<string> TextProperty = AvaloniaProperty.Register<Question, string>("Text");

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    
    public static readonly StyledProperty<object> ContentProperty = AvaloniaProperty.Register<Question, object>("Content");
    
    public object Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }
    public Question()
    {
        DataContext = this;
    }

    public Question(string? text, object content)
    {
        Text = text;
        Content = content;
    }
}