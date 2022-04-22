using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using DesktopClient.CustomControls;

namespace DesktopClient.Helpers;

public static class QuestionDecoder
{
    public static int QuestionNumber = 0;
    public static Question DecodeQuestion(Models.Question question)
    {
        Question q = new Question();
        int type = Convert.ToInt32(question.Type);
        QuestionNumber++;
        switch (type)
        {
            // multiple choice
            case 1:
            {
                q.Text = question.Description;
                
                string[]? answers = question.Content?.Split(',');

                Grid grid = new Grid();
                
                for (int i = 0; i < 2; i++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                }
                
                if (answers != null)
                    for (int i = 0; i < answers.Length / 2 + 1; i++)
                    {
                        grid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
                    }

                if (answers != null)
                    for (int i = 0; i < answers.Length; i++)
                    {
                        CheckBox checkBox = new CheckBox()
                        {
                            Content = answers[i],
                            Margin = new Thickness(0, 0, 0, 10),
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Center,
                        };

                        grid.Children.Add(checkBox);

                        Grid.SetColumn(checkBox, i % 2);
                        Grid.SetRow(checkBox, i / 2);

                        if (answers.Length % 2 != 0 && i == answers.Length - 1)
                        {
                            Grid.SetColumnSpan(checkBox, 2);
                        }
                        //checkBox.Checked += CheckBox_Checked;
                        //checkBox.Unchecked += CheckBox_Unchecked;
                    }

                q.Content = grid;
                
                return q;
            }
            // single choice
            case 2:
            {
                q.Text = question.Description;
                
                string[]? answers = question.Content?.Split(',');

                Grid grid = new Grid();
                
                // creating two star sized columns
                for (int i = 0; i < 2; i++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                }
                
                if (answers != null)
                {
                    for (int i = 0; i < answers.Length / 2 + 1; i++)
                    {
                        grid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
                    }
                }

                if (answers != null)
                {
                    for (int i = 0; i < answers.Length; i++)
                    {
                        RadioButton checkBox = new RadioButton()
                        {
                            Content = answers[i],
                            Margin = new Thickness(0, 0, 0, 10),
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Center,
                        };

                        grid.Children.Add(checkBox);

                        Grid.SetColumn(checkBox, i % 2);
                        Grid.SetRow(checkBox, i / 2);

                        if (answers.Length % 2 != 0 && i == answers.Length - 1)
                        {
                            Grid.SetColumnSpan(checkBox, 2);
                        }
                        //checkBox.Checked += CheckBox_Checked;
                        //checkBox.Unchecked += CheckBox_Unchecked;
                    }
                }

                q.Content = grid;
                
                return q;
            }
            // true or false
            case 3:
            {
                q.Text = question.Description;
                string[]? answers = question.Content?.Split(',');
                
                Grid grid = new Grid();
                
                // creating 3 star sized columns
                for (int i = 0; i < 3; i++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                }
                
                if (answers != null)
                {
                    for (int i = 0; i < answers.Length; i++)
                    {
                        grid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
                    }
                }

                if (answers != null)
                {
                    for (int i = 0; i < answers.Length; i++)
                    {
                        TextBlock textBlock = new TextBlock()
                        {
                            Text = answers[i],
                            Margin = new Thickness(20, 0, 0, 0),
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Left,
                        };

                        for (int j = 0; j < 2; j++)
                        {
                            RadioButton checkBox = new RadioButton()
                            {
                                Content = j == 0 ? "True" : "False",
                                GroupName = i.ToString() + QuestionNumber,
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = j == 0 ? HorizontalAlignment.Right : HorizontalAlignment.Center
                            };
                            Grid.SetRow(checkBox, i);
                            Grid.SetColumn(checkBox, j + 1);
                            grid.Children.Add(checkBox);
                        }

                        textBlock.TextWrapping = TextWrapping.Wrap;
                        grid.Children.Add(textBlock);
                        Grid.SetColumnSpan(textBlock, 2);
                        Grid.SetRow(textBlock, i);
                    }
                }

                q.Content = grid;
                return q;
            }
            // short answer
            case 4:
            {
                q.Text = question.Description;
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
                q.Content = textBox;
                return q;
            }
            // long answer
            case 5:
            {
                q.Text = question.Description;
          
                TextBox textBox = new TextBox()
                {
                    Margin = new Thickness(10, 10, 10, 10),
                    AcceptsReturn = true,
                    MinHeight = 100,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    TextWrapping = TextWrapping.Wrap,
                    Watermark = "Enter your answer here",
                    MaxLength = Convert.ToInt32(question.Content),
                };
                q.Content = textBox;
                return q;
            }
            default: return new Question("Error", "Unsupported question type");
        }
    }
}