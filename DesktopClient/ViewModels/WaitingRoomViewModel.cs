using Avalonia.Threading;
using DesktopClient.Helpers;
using DesktopClient.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.ViewModels
{
    public class WaitingRoomViewModel : ViewModelBase, IRoutableViewModel
    {
        #region Properties

        private TimeSpan _examTimer = new(0, 5, 5);
        
        public TimeSpan ExamTimer
        {
            get { return _examTimer; }
            set { this.RaiseAndSetIfChanged(ref _examTimer, value); }
        }

        #endregion

        public string? UrlPathSegment => "/WaitingRoom";

        private DispatcherTimer _timer { get; set; }
        public IScreen HostScreen { get; }

        public WaitingRoomViewModel(IScreen screen)
        {
            HostScreen = screen;
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();

        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            ExamTimer = ExamTimer.Subtract(TimeSpan.FromSeconds(1));
        }
    }
}
