using Avalonia.Threading;
using ReactiveUI;
using System;
using DesktopClient.CustomControls.StepCircle;


namespace DesktopClient.ViewModels
{
    public class WaitingRoomViewModel : ViewModelBase, IRoutableViewModel
    {
        #region Properties

        private TimeSpan _examTimer = new(0, 0, 5);
        
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


        public WaitingRoomViewModel(IScreen screen, StepManagerViewModel stepManager, MainWindowViewModel mainWindowp)
        {
            HostScreen = mainWindowp;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
            StepManager = stepManager;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            ExamTimer = ExamTimer.Subtract(TimeSpan.FromSeconds(1));
            if (ExamTimer.TotalSeconds == 0)
            {
                _timer.Stop();
                HostScreen.Router.Navigate.Execute(new ExamRoomViewModel(HostScreen));
            }
            
        }
    }
}
