using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using DesktopClient.CustomControls;
using System;
using WebViewControl;

namespace DesktopClient.Helpers;

public class QuestionDecoder
{
    private Control DecodeDescription(Models.QuestionDescription description)
    {
        switch (description.Type)
        {
            // normal text
            case 1:
                {
                    StackPanel control = new StackPanel();

                    TextBlock label = new TextBlock
                    {
                        Text = description.Label,
                        FontWeight = FontWeight.Bold,
                        Margin = new Thickness(5, 5, 5, 10)
                    };

                    TextBlock content = new TextBlock
                    {
                        Text = description.Content,
                        Margin = new Thickness(5, 5, 5, 5)
                    };

                    control.Children.Add(label);
                    control.Children.Add(content);

                    return control;
                }
            // rich text
            case 2:
                {
                    StackPanel control = new StackPanel();

                    TextBlock label = new TextBlock
                    {
                        Text = description.Label,
                        FontWeight = FontWeight.Bold,
                        Margin = new Thickness(5, 5, 5, 10)
                    };


                    WebView content = new WebView()
                    {
                        Margin = new Thickness(5, 5, 5, 5),
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        MinHeight = 80,
                        Address = @"C:\Users\rd07g\Desktop\OSES\desktop-avalonia-client\DesktopClient\Views\HtmlViews\RichTextBox.html"
                    };

                    control.Children.Add(label);
                    control.Children.Add(content);

                    return control;
                }
            default:
                return new TextBlock { Text = "Unknown description type" };
        }
    }
    private Control DecodeAnswer(Models.QuestionAnswer answer)
    {
        switch (answer.Type)
        {
            // multiple choice
            case 1:
                {
                    Grid grid = new Grid();

                    for (int i = 0; i < 2; i++)
                    {
                        grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                    }

                    if (answer.Content != null)
                    {
                        for (int i = 0; i < answer.Content.Count / 2 + 1; i++)
                        {
                            grid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
                        }

                        for (int i = 0; i < answer.Content.Count; i++)
                        {

                            CheckBox checkBox = new CheckBox()
                            {
                                Content = answer.Content[i].Content,
                                IsChecked = answer.Content[i].IsChecked,
                                Margin = new Thickness(0, 0, 0, 10),
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = HorizontalAlignment.Center,
                            };

                            grid.Children.Add(checkBox);

                            Grid.SetColumn(checkBox, i % 2);
                            Grid.SetRow(checkBox, i / 2);

                            if (answer.Content.Count % 2 != 0 && i == answer.Content.Count - 1)
                            {
                                Grid.SetColumnSpan(checkBox, 2);
                            }
                            //checkBox.Checked += CheckBox_Checked;
                            //checkBox.Unchecked += CheckBox_Unchecked;
                        }
                    }
                    return grid;
                }
            // single choice
            case 2:
                {
                    Grid grid = new Grid();
                    for (int i = 0; i < 2; i++)
                    {
                        grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                    }

                    if (answer.Content != null)
                    {
                        for (int i = 0; i < answer.Content.Count / 2 + 1; i++)
                        {
                            grid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
                        }

                        for (int i = 0; i < answer.Content.Count; i++)
                        {

                            RadioButton radioButton = new RadioButton()
                            {
                                Content = answer.Content[i].Content,
                                IsChecked = answer.Content[i].IsChecked,
                                Margin = new Thickness(0, 0, 0, 10),
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = HorizontalAlignment.Center,
                            };

                            grid.Children.Add(radioButton);

                            Grid.SetColumn(radioButton, i % 2);
                            Grid.SetRow(radioButton, i / 2);

                            if (answer.Content.Count % 2 != 0 && i == answer.Content.Count - 1)
                            {
                                Grid.SetColumnSpan(radioButton, 2);
                            }
                            //checkBox.Checked += CheckBox_Checked;
                            //checkBox.Unchecked += CheckBox_Unchecked;
                        }
                    }
                    return grid;
                }
            // true or false
            case 3:
                {
                    Grid grid = new Grid();

                    for (int i = 0; i < 2; i++)
                    {
                        grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                    }

                    if (answer.Content != null)
                        for (int i = 0; i < answer.Content.Count / 2 + 1; i++)
                        {
                            grid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
                        }

                    for (int i = 0; i < answer.Content.Count; i++)
                    {
                        CheckBox checkBox = new CheckBox()
                        {
                            Content = answer.Content[i].Content,
                            IsChecked = answer.Content[i].IsChecked,
                            Margin = new Thickness(0, 0, 0, 10),
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Center,
                        };

                        grid.Children.Add(checkBox);

                        Grid.SetColumn(checkBox, i % 2);
                        Grid.SetRow(checkBox, i / 2);

                        if (answer.Content.Count % 2 != 0 && i == answer.Content.Count - 1)
                        {
                            Grid.SetColumnSpan(checkBox, 2);
                        }
                        //checkBox.Checked += CheckBox_Checked;
                        //checkBox.Unchecked += CheckBox_Unchecked;
                    }
                    return grid;
                }
            // short text
            case 4:
                {
                    TextBox textBox = new TextBox()
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        MinWidth = 400,
                        Margin = new Thickness(10, 10, 10, 10),
                        TextWrapping = TextWrapping.Wrap,
                        Watermark = "Enter your answer here",
                        MaxLength = 20,
                    };
                    return textBox;
                }
            // long text
            case 5:
                {
                    TextBox textBox = new TextBox()
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        MinWidth = 400,
                        Margin = new Thickness(10, 10, 10, 10),
                        TextWrapping = TextWrapping.Wrap,
                        Watermark = "Enter your answer here",
                        MaxLength = 600,
                    };
                    return textBox;
                }
            // fill in the blanks
            case 6:
                {
                    return new TextBlock { Text = "Fill in the blanks" };
                }
            // script
            case 7:
                {
                    return new TextBlock { Text = "Script" };
                }
            // rich box
            case 8:
                {
                    WebView webView = new WebView()
                    {
                        Margin = new Thickness(5, 5, 5, 5),
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        MinHeight = 80,
                        Address = @"C:\Users\rd07g\Desktop\OSES\desktop-avalonia-client\DesktopClient\Views\HtmlViews\RichTextBox.html"
                    };
                    return webView;
                }
            // diagram
            case 9:
                {
                    return new TextBlock { Text = "Diagram" };
                }
            default:
                return new TextBlock { Text = "Unknown answer type" };
        }
    }
    public Question DecodeQuestion(Models.Question question)
    {
        Question q = new Question();

        try
        {
            q.Description = DecodeDescription(question.Description);
            q.Answer = DecodeAnswer(question.Answer);
        }
        catch (Exception e)
        {
            ExceptionNotifier.NotifyError(e.Message);
        }

        return q;
    }
}