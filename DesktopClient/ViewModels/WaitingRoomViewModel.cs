using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using DesktopClient.Views;
using ReactiveUI;
using System;
using System.Reactive;

namespace DesktopClient.ViewModels
{
    public class WaitingRoomViewModel : ViewModelBase, IRoutableViewModel
    {
        #region Properties

        private TimeSpan _examTimer = new(0, 0, 1);

        public TimeSpan ExamTimer
        {
            get { return _examTimer; }
            set { this.RaiseAndSetIfChanged(ref _examTimer, value); }
        }

        #endregion

        public string? UrlPathSegment => "/WaitingRoom";

        private DispatcherTimer _timer { get; set; }

        public IScreen HostScreen { get; }

        public StepManagerViewModel StepManager { get; }

        public ReactiveCommand<Unit, Unit> Navigate { get; }

        public IObservable<bool> Executing => Navigate.IsExecuting;


        public WaitingRoomViewModel(IScreen screen, StepManagerViewModel stepManager, MainWindowViewModel mainWindowp)
        {
            HostScreen = mainWindowp;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
            StepManager = stepManager;

            Navigate = ReactiveCommand.CreateFromTask(async () =>
            {
                ExamRoomViewModel examRoomViewModel = new ExamRoomViewModel(HostScreen);
                await examRoomViewModel.InitTask;
                HostScreen.Router.Navigate.Execute(new ExamRoomViewModel(HostScreen));
            });

            // exception handeling
            Navigate.ThrownExceptions.Subscribe(x =>
                      MainWindow.WindowNotificationManager?.Show(new Avalonia.Controls.Notifications.Notification("Error",
                      x.Message,
                      NotificationType.Error)));
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            ExamTimer = ExamTimer.Subtract(TimeSpan.FromSeconds(1));
            if (ExamTimer.TotalSeconds == 0)
            {
                _timer.Stop();
                Navigate.Execute().Subscribe();
            }

        }
    }
}
