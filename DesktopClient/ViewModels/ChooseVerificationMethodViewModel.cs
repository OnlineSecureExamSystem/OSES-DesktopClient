using Avalonia.Controls.Notifications;
using DesktopClient.CustomControls.StepCircle;
using DesktopClient.Services;
using DesktopClient.Views;
using ReactiveUI;
using System;
using System.Reactive;

namespace DesktopClient.ViewModels
{
    public class ChooseVerificationMethodViewModel : ViewModelBase, IRoutableViewModel
    {
        public string? UrlPathSegment => "/ChooseVerificationMethod";

        public IScreen HostScreen { get; }

        public StepManagerViewModel StepManager { get; }

        public ReactiveCommand<string, Unit> SendCodeCommand { get; }

        public ReactiveCommand<Unit, Unit> NavigateBack { get; }

        public ReactiveCommand<Unit, Unit> SkipCommand { get; }

        public IObservable<bool> Executing => SendCodeCommand.IsExecuting;

        public MainWindowViewModel MainWindowp { get; }



        public ChooseVerificationMethodViewModel(IScreen screen, StepManagerViewModel stepManager, MainWindowViewModel mainWindow)
        {
            HostScreen = screen;
            StepManager = stepManager;
            MainWindowp = mainWindow;


            SkipCommand = ReactiveCommand.Create(() =>
            {
                StepManager.LoginCtrl = new Done();
                StepManager.ExamCodeCtrl = new Running();
                HostScreen.Router.Navigate.Execute(new EnterCodeViewModel(screen, StepManager, MainWindowp));
            });

            SendCodeCommand = ReactiveCommand.CreateFromTask<string, Unit>(async (p) =>
            {
                AuthenticationService authenticationService = new AuthenticationService();
                if (p == "email")
                {
                    await authenticationService.SendCodeToEmail();
                    MainWindow.WindowNotificationManager?.Show(new Avalonia.Controls.Notifications.Notification("Info",
                      "Code sent to your email",
                      NotificationType.Information));
                }
                else
                {
                    await authenticationService.SendCodeToSMS();
                    MainWindow.WindowNotificationManager?.Show(new Avalonia.Controls.Notifications.Notification("Info",
                      "Code sent to your phone",
                      NotificationType.Information));
                }
                HostScreen.Router.Navigate.Execute(new EnterVerificationCodeViewModel(screen, StepManager, MainWindowp));
                return Unit.Default;
            });

            NavigateBack = ReactiveCommand.Create(() =>
            {
                HostScreen.Router.NavigateBack.Execute();
            });
        }
    }
}
