using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using DesktopClient.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                        Address = @"C:\Users\rd07g\Desktop\OSES\desktop-avalonia-client\DesktopClient\Views\HtmlViews\EmptyHtml.html"
                    };
                    content.ExecuteScript("document.body.innerHTML = '" + description.Content + "';");
                    control.Children.Add(label);
                    control.Children.Add(content);

                    return control;
                }
            default:
                return new TextBlock { Text = "Unknown description type" };
        }
    }
    private Control DecodeAnswer(QuestionAnswer answer)
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

                    for (int i = 0; i < 3; i++)
                    {
                        grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                    }

                    if (answer.Content != null)
                    {
                        for (int i = 0; i < answer.Content.Count; i++)
                        {
                            grid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
                        }

                        for (int i = 0; i < answer.Content.Count; i++)
                        {
                            TextBlock textBlock = new TextBlock()
                            {
                                Text = answer.Content[i].Content,
                                Margin = new Thickness(20, 0, 0, 0),
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = HorizontalAlignment.Left,
                            };

                            textBlock.TextWrapping = TextWrapping.Wrap;
                            grid.Children.Add(textBlock);
                            Grid.SetColumnSpan(textBlock, 2);
                            Grid.SetRow(textBlock, i);

                            for (int j = 0; j < 2; j++)
                            {
                                RadioButton radioButton = new RadioButton()
                                {
                                    Content = j == 0 ? "True" : "False",
                                    GroupName = answer.Content[i].Content,
                                    VerticalAlignment = VerticalAlignment.Center,
                                    HorizontalAlignment = j == 0 ? HorizontalAlignment.Right : HorizontalAlignment.Center
                                };
                                Grid.SetRow(radioButton, i);
                                grid.Children.Add(radioButton);
                                Grid.SetColumn(radioButton, j + 1);
                            }
                        }
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
                        Height = 100,
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
                    StackPanel stackPanel = new StackPanel()
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(10, 10, 10, 10),
                    };

                    for (int i = 0; i < answer.Content.Count; i++)
                    {
                        TextBox textBox = new TextBox()
                        {
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            MinWidth = 300,
                            Margin = new Thickness(5, 5, 5, 5),
                            TextWrapping = TextWrapping.Wrap,
                            Watermark = (i + 1).ToString(),
                            MaxLength = 20,
                        };
                        stackPanel.Children.Add(textBox);
                    }

                    return stackPanel;
                }
            // rich box
            case 7:
                {
                    WebView webView = new WebView()
                    {
                        Margin = new Thickness(5, 5, 5, 5),
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        MinHeight = 150
                       ,
                        Address = @"C:\Users\rd07g\Desktop\OSES\desktop-avalonia-client\DesktopClient\Views\HtmlViews\RichTextBox.html"
                    };
                    return webView;
                }
            // script
            case 8:
                {
                    return new TextBlock { Text = "Script" };

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
    public CustomControls.Question DecodeQuestion(Models.Question question)
    {
        CustomControls.Question q = new CustomControls.Question();

        try
        {
            q.Description = DecodeDescription(question.Description);
            q.Answer = DecodeAnswer(question.Answer);
            q.QuestionType = question.Answer.Type;
        }
        catch (Exception e)
        {
            ExceptionNotifier.NotifyError(e.Message);
        }

        return q;
    }

    public async Task<Exam> GetAnswers(Exam exam, StackPanel questionsPanel)
    {
        List<CustomControls.Exercise> exercisesControls = new List<CustomControls.Exercise>();
        for (int i = 1; i < questionsPanel.Children.Count; i++)
        {
            exercisesControls.Add(questionsPanel.Children[i] as CustomControls.Exercise);
        }

        for (int i1 = 0; i1 < exercisesControls.Count; i1++)
        {

            Panel? exercise = exercisesControls[i1].Questions as Panel;

            int foreachIndex = 0;
            foreach (var item in exercise.Children)
            {

                if (item is TextBlock)
                {
                    continue;
                }
                CustomControls.Question? question = exercise.Children[foreachIndex] as CustomControls.Question;

                switch (question.QuestionType)
                {
                    // multiple choice
                    case 1:
                        {
                            Grid? grid = question.Answer as Grid;
                            for (int j = 0; j < grid.Children.Count; j++)
                            {
                                var item1 = grid.Children[j];
                                if (item1 is CheckBox checkBox)
                                {
                                    exam.Exercises[i1].Questions[foreachIndex].Answer.Content[j].IsChecked = checkBox.IsChecked;
                                }
                            }

                        }
                        break;
                    // single choice
                    case 2:
                        {
                            Grid? grid = question.Answer as Grid;
                            for (int j = 0; j < grid.Children.Count; j++)
                            {
                                var item1 = grid.Children[j];
                                if (item1 is RadioButton radioButton)
                                {
                                    exam.Exercises[i1].Questions[foreachIndex].Answer.Content[j].IsChecked = radioButton.IsChecked;
                                }
                            }
                        }
                        break;
                    // true or false
                    case 3:
                        {
                            Grid? grid = question.Answer as Grid;

                            for (int m = 0; m < grid.RowDefinitions.Count; m++)
                            {
                                for (int j = 0; j < grid.Children.Count; j++)
                                {
                                    if (Grid.GetRow(grid.Children[j] as Control) == m)
                                    {
                                        var control = grid.Children[j];
                                        if (control is RadioButton radioButton)
                                        {
                                            RadioButton? radioButton1 = grid.Children[j + 1] as RadioButton;
                                            if (radioButton.IsChecked == true)
                                            {
                                                exam.Exercises[i1].Questions[foreachIndex].Answer.Content[m].IsChecked = true;
                                            }
                                            else if (radioButton1.IsChecked == true)
                                            {
                                                exam.Exercises[i1].Questions[foreachIndex].Answer.Content[m].IsChecked = false;
                                            }
                                            else
                                            {
                                                exam.Exercises[i1].Questions[foreachIndex].Answer.Content[m].IsChecked = null;
                                            }
                                            j++;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    // short text, long text
                    case 4:
                    case 5:
                        {
                            TextBox? textBox = question.Answer as TextBox;
                            List<AnswerItem> answer = new List<AnswerItem>();
                            AnswerItem answerItem = new AnswerItem() { Content = textBox.Text };
                            answer.Add(answerItem);
                            exam.Exercises[i1].Questions[foreachIndex].Answer.Content = answer;
                        }
                        break;
                    // fill in the blanks
                    case 6:
                        {
                            StackPanel? stackpanel = question.Answer as StackPanel;

                            for (int j = 0; j < stackpanel.Children.Count; j++)
                            {
                                TextBox? item1 = stackpanel.Children[j] as TextBox;
                                exam.Exercises[i1].Questions[foreachIndex].Answer.Content[j].Content = item1.Text;
                            }
                        }
                        break;
                    // rich box 
                    case 7:
                        {
                            WebView? webView = question.Answer as WebView;
                            string? data = await webView.EvaluateScriptFunction<string>("submit");
                            List<AnswerItem> answer = new List<AnswerItem>();
                            AnswerItem answerItem = new AnswerItem() { Content = data };
                            answer.Add(answerItem);
                            exam.Exercises[i1].Questions[foreachIndex].Answer.Content = answer;
                        }
                        break;
                    // script
                    case 8:
                        {

                        }
                        break;
                    // diagram
                    case 9:
                        {

                        }
                        break;
                    default:
                        ExceptionNotifier.NotifyError($"Couldnt get the answer number : {foreachIndex - 1}");
                        break;
                }
                foreachIndex++;
            }
        }
        return exam;
    }
}