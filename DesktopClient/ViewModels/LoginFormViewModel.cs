using Avalonia.Controls.Notifications;
using DesktopClient.Helpers;
using DesktopClient.Models;
using DesktopClient.Views;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;

namespace DesktopClient.ViewModels
{
    public class LoginFormViewModel : ViewModelBase, IRoutableViewModel
    {
        #region Properties
        private string? _email;

        public string? Email
        {
            get => _email;
            set
            {
                value?.IsValid(DataTypes.Email);
                this.RaiseAndSetIfChanged(ref _email, value);
            }
        }

        private string? _password;


        public string? Password
        {
            get => _password;
            set
            {
                value?.IsValid(DataTypes.Password);
                this.RaiseAndSetIfChanged(ref _password, value);
            }
        }

        public IObservable<bool> Executing => LoginCommand.IsExecuting;

        public ReactiveCommand<Unit, Unit> LoginCommand { get; }

        public StepManagerViewModel StepManager { get; }

        public ReactiveCommand<Unit, Unit> OpenBrowser { get; }

        public string? UrlPathSegment => "/LoginForm";

        public IScreen HostScreen { get; }

        #endregion

        public LoginFormViewModel(IScreen screen, StepManagerViewModel stepManager)
        {
            HostScreen = screen;
            StepManager = stepManager;
           
            var canLogin = this.WhenAnyValue(
                x => x.Email, x => x.Password,
                (email, pass) =>
                    !string.IsNullOrEmpty(email) &&
                    !string.IsNullOrEmpty(pass)
                );

            LoginCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                await Task.Run(() => Thread.Sleep(3000));
                HostScreen.Router.Navigate.Execute(new ChooseVerificationMethodViewModel(screen, StepManager));
            }, canLogin);

            // exception handeling
            LoginCommand.ThrownExceptions.Subscribe(x =>
                      MainWindow.WindowNotificationManager?.Show(new Avalonia.Controls.Notifications.Notification("Error",
                      x.Message,
                      NotificationType.Error)));

            OpenBrowser = ReactiveCommand.Create(openBrowser);
        }

        void openBrowser()
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "http://www.google.com",
                UseShellExecute = true
            };
            Process.Start(psi);
        }

    }
}
