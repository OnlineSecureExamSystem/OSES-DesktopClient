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

        private Border _border = new Border()
        {
            Background = Brushes.Black,
            IsVisible = false,
            Opacity = 0.5,
            ZIndex = 500
        };

        private readonly DispatcherTimer _timer;

        public Border Border
        {
            get => _border;
            set => this.RaiseAndSetIfChanged(ref _border, value);
        }
        #region Properties

        private TimeSpan _examTimer = new(1, 30, 0);

        public TimeSpan ExamTimer
        {
            get { return _examTimer; }
            set { this.RaiseAndSetIfChanged(ref _examTimer, value); }
        }

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

        private Border _msgBoxBackground;

        public Border MsgBoxBackground
        {
            get => _msgBoxBackground;
            set => this.RaiseAndSetIfChanged(ref _msgBoxBackground, value);
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

        public ReactiveCommand<Unit, Unit> BackgroundOn { get; }

        public ReactiveCommand<Unit, Unit> BackgroundOff { get; }
        public ReactiveCommand<Unit, Unit> MessagePopupCommand { get; }
        public ReactiveCommand<Unit, Unit> RefreshCommand { get; }

        public Task InitTask { get; private set; }


        public Exam ExamObject { get; private set; }

        public List<Models.Question> QuestionsList { get; private set; }

        public IObservable<bool> Executing => RefreshCommand.IsExecuting;

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
            var mainWindow = screen as MainWindowViewModel;
            mainWindow.Monitor.StopMonitoring();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += _timer_Tick;

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

            BackgroundOn = ReactiveCommand.Create(() => { Border.IsVisible = true; });
            BackgroundOff = ReactiveCommand.Create(() => { Border.IsVisible = false; });

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
                    ExamService examService = new ExamService();
                    var examAnswers = await decoder.GetAnswers(ExamObject, QuestionsStackPanel);
                    var results = await examService.SendExamAnswers(examAnswers);
                    if (results)
                        ExceptionNotifier.NotifySuccess("Exam asnwers submitted sucessfully ✅");
                    else
                        ExceptionNotifier.NotifyError("Something went wrong ❌");
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
                    BackgroundOn.Execute().Subscribe();
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

            SystemMonitor monitor = new SystemMonitor(this);
            monitor.StartExamMonitoring();
            _timer.Start();
        }

        private void _timer_Tick(object? sender, EventArgs e)
        {
            ExamTimer = ExamTimer.Subtract(TimeSpan.FromSeconds(1));
            if (ExamTimer.TotalSeconds == 0)
            {
                _timer.Stop();
            }
        }

        async Task<List<Exercise>> Init()
        {
            return await GetExercises();
        }

        public void setBackgroundVisibility(bool visibility)
        {
            Border.IsVisible = visibility;
        }
        private async Task<List<Exercise>> GetExercises()
        {
            ExamService examService = new ExamService();
            ExamObject = await examService.GetExamAsync("AAAAAA");

            return ExamObject.Exercises;
        }
    }
}