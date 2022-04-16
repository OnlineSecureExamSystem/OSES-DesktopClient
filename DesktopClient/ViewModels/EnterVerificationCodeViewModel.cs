using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using DesktopClient.CustomControls.StepCircle;
using DesktopClient.Helpers;
using DesktopClient.Models;
using DesktopClient.Services;
using DesktopClient.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.ViewModels
{
    public class EnterVerificationCodeViewModel : ViewModelBase, IRoutableViewModel
    {
        #region Properties
        private string _code;

        public string Code
        {
            get { return _code; }
            set
            {
                value.IsValid(DataTypes.VerificationCode);
                this.RaiseAndSetIfChanged(ref _code, value);
            }
        }

        private string _bottomText;

        public string BottomText
        {
            get { return _bottomText; }
            set { this.RaiseAndSetIfChanged(ref _bottomText, value); }
        }

        private TimeSpan _codeTimer = new(0, 1, 25);

        public TimeSpan CodeTimer
        {
            get { return _codeTimer; }
            set { this.RaiseAndSetIfChanged(ref _codeTimer, value); }
        }
        #endregion


        private DispatcherTimer _timer { get; set; }


        public ReactiveCommand<Unit, Unit> ConfirmCommand { get; }

        public ReactiveCommand<Unit, Unit> NavigateBack { get; }

        public IObservable<bool> Executing => ConfirmCommand.IsExecuting;


        public string? UrlPathSegment => "/EnterVerificationCode";

        public IScreen HostScreen { get; }

        public StepManagerViewModel StepManager { get; }

        public EnterVerificationCodeViewModel(IScreen screen, StepManagerViewModel stepManager)
        {
            HostScreen = screen;
            StepManager = stepManager;


            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();

            BottomText = "code expires in " + CodeTimer.ToString(@"mm\:ss");

            var canConfirm = this.WhenAnyValue(x =>
                x.Code, x => !string.IsNullOrEmpty(x));
            

            ConfirmCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                AuthenticationService authenticationService = new AuthenticationService();
                bool result = await authenticationService.VerifyCode(Code);
                if (result)
                {
                    MainWindow.WindowNotificationManager?.Show(new Avalonia.Controls.Notifications.Notification("Success",
                         "You are logged in successfully!",
                         NotificationType.Success));
                    
                    HostScreen.Router.Navigate.Execute(new EnterCodeViewModel(HostScreen, StepManager));
                    StepManager.LoginCtrl = new Done();
                    StepManager.ExamCodeCtrl = new Running();
                }
                else
                {
                    MainWindow.WindowNotificationManager?.Show(new Avalonia.Controls.Notifications.Notification("Error",
                      "Wrong verification code, try again",
                      NotificationType.Error));
                }
                
            }, canConfirm);

            NavigateBack = ReactiveCommand.Create(() =>
            {
                HostScreen.Router.NavigateBack.Execute();
            });
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            CodeTimer = CodeTimer.Subtract(TimeSpan.FromSeconds(1));
            BottomText = "code expires in " + CodeTimer.ToString(@"mm\:ss");
            if (CodeTimer.TotalSeconds == 0)
            {
                _timer.Stop();
                BottomText = "verification code expired";
                
                if (HostScreen.Router.GetCurrentViewModel().UrlPathSegment == "/EnterVerificationCode")
                {
                    MainWindow.WindowNotificationManager?.Show(new Avalonia.Controls.Notifications.Notification("Info",
                      "verification code expired, please go back to the previous page and try again",
                      NotificationType.Information));
                }
            }
        }
    }
}
