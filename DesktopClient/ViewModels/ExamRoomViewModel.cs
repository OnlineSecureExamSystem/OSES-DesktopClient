using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using DesktopClient.Helpers;
using DesktopClient.Models;
using DesktopClient.Services;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
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

        public ReactiveCommand<Unit, Unit> SubmitCommand { get; }

        public ReactiveCommand<Unit, Unit> ExitCommand { get; }

        public ReactiveCommand<Unit, Unit> InfoCommand { get; }

        public ReactiveCommand<Unit, Unit> ZoomInCommand { get; }

        public ReactiveCommand<Unit, Unit> ZoomOutCommand { get; }

        public ReactiveCommand<Unit, Unit> MessagePopupCommand { get; }

        public ReactiveCommand<Unit, Unit> RefreshCommand { get; }

        public Task InitTask { get; private set; }

        public Exam ExamObject { get; private set; }

        public List<Models.Question> QuestionsList { get; private set; }

        public IObservable<bool> Executing => RefreshCommand.IsExecuting;

        bool IsMessageBoxOpen => SystemMonitor.IsMessageBoxOpen;



        Task LoadingControls()
        {
            return Task.Run(() => Init()).ContinueWith(t =>
            {
                Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    try
                    {
                        QuestionDecoder decoder = new QuestionDecoder();

                        for (var i = 0; i < t.Result.Count; i++)
                        {
                            Exercise exercise = t.Result[i];

                            StackPanel stackPanel = new StackPanel();

                            foreach (var question in exercise.Questions)
                            {
                                Question questionControl = decoder.DecodeQuestion(question);
                                stackPanel.Children.Add(questionControl);
                            }

                            CustomControls.Exercise exerciseControl = new CustomControls.Exercise
                            {
                                Title = exercise.Title,
                                Description = exercise.Description,
                                Questions = stackPanel
                            };

                            QuestionsStackPanel.Children.Add(exerciseControl);
                        }
                    }
                    catch (Exception e)
                    {
                        ExceptionNotifier.NotifyError(e.Message);
                    }
                });
            });
        }

        public ExamRoomViewModel(IScreen screen)
        {
            SystemMonitor.IsInExamRoom = true;
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

            InitTask = LoadingControls();

            SubmitCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var messageBox = MessageBoxManager.GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions = new[]
                    {
                        new ButtonDefinition{Name = "Yes", IsDefault = true},
                        new ButtonDefinition{Name = "Cancel", IsCancel = true}
                    },
                    ContentTitle = "Submit",
                    ContentMessage = "Are you sure you want to submit?",
                    Icon = Icon.Warning,
                    Topmost = true,
                    CanResize = false,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                });
                var result = await messageBox.Show();
                if (result == "Yes")
                {
                    QuestionDecoder decoder = new QuestionDecoder();
                    decoder.GetAnswers(ExamObject, QuestionsStackPanel);
                }
            });

            ExitCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var messageBox = MessageBoxManager.GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions = new[]
                    {
                        new ButtonDefinition{Name = "Yes", IsDefault = true},
                        new ButtonDefinition{Name = "Cancel", IsCancel = true}
                    },
                    ContentTitle = "Exit",
                    ContentMessage = "Are you sure you want to Exit the exam?",
                    Icon = Icon.Error,
                    Topmost = true,
                    CanResize = false,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                });
                var result = await messageBox.Show();
                if (result == "Yes")
                {
                    Dispatcher.UIThread.MainLoop(new CancellationToken(true));
                }
            });

            InfoCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var messageBox = MessageBoxManager.GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions = new[]
                    {
                        new ButtonDefinition{Name = "Yes", IsDefault = true},
                        new ButtonDefinition{Name = "Cancel", IsCancel = true}
                    },
                    ContentTitle = "Exit",
                    ContentMessage = "Are you sure you want to Exit the exam?",
                    Icon = Icon.Error,
                    Topmost = true,
                    CanResize = false,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                });
                var result = await messageBox.Show();
                if (result == "Yes")
                {
                    Dispatcher.UIThread.MainLoop(new CancellationToken(true));
                }
            });

            MessagePopupCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var messageBox = MessageBoxManager.GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions = new[]
                    {
                        new ButtonDefinition{Name = "Yes", IsDefault = true},
                        new ButtonDefinition{Name = "Cancel", IsCancel = true}
                    },
                    ContentTitle = "Exit",
                    ContentMessage = "Are you sure you want to Exit the exam?",
                    Icon = Icon.Error,
                    Topmost = true,
                    CanResize = false,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                });
                var result = await messageBox.Show();
                if (result == "Yes")
                {
                    Dispatcher.UIThread.MainLoop(new CancellationToken(true));
                }
            });

            RefreshCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                QuestionsStackPanel.Children.Clear();
                await LoadingControls();
            });
        }
        async Task<List<Exercise>> Init()
        {
            return await GetExercises();
        }

        private async Task<List<Exercise>> GetExercises()
        {
            ExamService examService = new ExamService();
            ExamObject = await examService.GetExamAsync("AAAAAA");

            return ExamObject.Exercises;
        }
    }
}