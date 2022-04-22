using Avalonia.Controls;
using DesktopClient.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Layout;
using Avalonia.Media;
using System.Text.Json;
using static DesktopClient.Helpers.QuestionDecoder;
using Question = DesktopClient.CustomControls.Question;

namespace DesktopClient.ViewModels
{
    public class ExamRoomViewModel : ViewModelBase, IRoutableViewModel
    {
        #region Properties

        private StackPanel _questionsStackPanel;

        public StackPanel QuestionsStackPanel
        {
            get => _questionsStackPanel;
            set => this.RaiseAndSetIfChanged(ref _questionsStackPanel, value);
        }


        private List<Question> _questions;

        public List<Question> Questions
        {
            get => _questions;
            set => this.RaiseAndSetIfChanged(ref _questions, value);
        }

        #endregion

        public string? UrlPathSegment => "/ExamRoom";

        public IScreen HostScreen { get; }

        public ReactiveCommand<Unit, Unit> TestCommand { get; }


        public ExamRoomViewModel(IScreen screen)
        {
            HostScreen = screen;
            QuestionsStackPanel = new StackPanel
            {
                DataContext = this
            };

            
            var examNameTextBox = new TextBlock
            {
                Width = 200,
                Height = 30,
                Margin = new Thickness(10, 30, 10, 30),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontSize = 30,
                FontWeight = FontWeight.Bold,
                Text = "Exam Name"
            };
            
            QuestionsStackPanel.Children.Add(examNameTextBox);
            
            // getting questions from a json file
            var questions = GetQuestions();

            for (var i = 0; i < questions.Count; i++)
            {
                var question = questions[i];
                
                // specifying the question number before adding it to the stack panel
                if (question.Description != null && !char.IsNumber(question.Description[0]))
                {
                    string questionDescription = $"{i + 1}. {question.Description}";
                    question.Description =  questionDescription;
                }
                QuestionsStackPanel.Children.Add(DecodeQuestion(question));
            }

            TestCommand = ReactiveCommand.Create(() =>
            {
                QuestionsStackPanel.Children.Add(new Question("Question Text", new StackPanel(){Orientation = Orientation.Horizontal,Children = { new CheckBox(), new CheckBox() }}));
            });
        }

        private List<Models.Question> GetQuestions()
        {
            var questions = new List<Models.Question>();
            var json = System.IO.File.ReadAllText("../Question.json");
            questions = JsonSerializer.Deserialize<List<Models.Question>>(json);
            return questions;
        }
    }
}