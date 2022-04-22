using Avalonia.Controls.Notifications;
using DesktopClient.Services;
using DesktopClient.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DesktopClient.ViewModels
{
    public class ChooseVerificationMethodViewModel : ViewModelBase, IRoutableViewModel
    {
        public string? UrlPathSegment => "/ChooseVerificationMethod";

        public IScreen HostScreen { get; }

        public StepManagerViewModel StepManager { get; }

        public ReactiveCommand<string, Unit> SendCodeCommand { get; }

        public ReactiveCommand<Unit, Unit> NavigateBack { get; }

        public IObservable<bool> Executing => SendCodeCommand.IsExecuting;

        public MainWindowViewModel MainWindowp { get; }

        

        public ChooseVerificationMethodViewModel(IScreen screen, StepManagerViewModel stepManager, MainWindowViewModel mainWindow)
        {
            HostScreen = screen;
            StepManager = stepManager;
            MainWindowp = mainWindow;

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
