using Avalonia.Controls.Notifications;
using Avalonia.Data;
using DesktopClient.Helpers;
using DesktopClient.Models;
using ReactiveUI;
using System;
using System.Reactive;
using System.Text.RegularExpressions;
using DesktopClient.Views;
using static DesktopClient.Views.MainWindow;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;

namespace DesktopClient.ViewModels
{
    public class LoginFormViewModel : ViewModelBase
    {

        private string? _email;
        
        private string? Email
        {
            get => _email;
            set
            {
                value?.IsValid(DataTypes.Email);
                this.RaiseAndSetIfChanged(ref _email, value);
            }
        }

        private string? _password;

        
        private string? Password
        {
            get => _password;
            set
            {
                value?.IsValid(DataTypes.Password);
                this.RaiseAndSetIfChanged(ref _password, value);
            }
        }


        private ReactiveCommand<Unit, Unit> LoginCommand { get; }

        public LoginFormViewModel()
        {
            var canLogin = this.WhenAnyValue(
                x => x.Email, x => x.Password,
                (email, pass) =>
                    !string.IsNullOrEmpty(email) &&
                    !string.IsNullOrEmpty(pass)
                );

            LoginCommand = ReactiveCommand.Create(Login, canLogin);

            // exception handeling
            LoginCommand.ThrownExceptions.Subscribe(x =>
                      MainWindow.WindowNotificationManager?.Show(new Avalonia.Controls.Notifications.Notification("Error",
                      x.Message,
                      NotificationType.Error)));

        }
        
        private void Login()
        {
            StepManagerViewModel stepManager = new StepManagerViewModel();
            stepManager.NavigateCommand.Execute(new EnterCode()).Subscribe();
        }
    }
}
