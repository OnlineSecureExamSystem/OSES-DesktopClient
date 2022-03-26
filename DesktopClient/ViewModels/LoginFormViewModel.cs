using Avalonia.Controls.Notifications;
using Avalonia.Data;
using DesktopClient.Helpers;
using DesktopClient.Models;
using ReactiveUI;
using System;
using System.Reactive;
using System.Text.RegularExpressions;
using static DesktopClient.Views.MainWindow;

namespace DesktopClient.ViewModels
{
    public class LoginFormViewModel : ViewModelBase
    {

        string email;
        
        public string Email
        {
            get => email;
            set
            {
                value.isValid(DataTypes.Email);
                this.RaiseAndSetIfChanged(ref email, value);
            }
        }

        string password;

        
        public string Password
        {
            get => password;
            set
            {
                value.isValid(DataTypes.Password);
                this.RaiseAndSetIfChanged(ref password, value);
            }
        }


        public ReactiveCommand<Unit, Unit> LoginCommand { get; }

        public LoginFormViewModel()
        {
            var canLogin = this.WhenAnyValue(
                x => x.Email, x => x.Password,
                (email, pass) =>
                    !string.IsNullOrEmpty(email) &&
                    !string.IsNullOrEmpty(pass)
                );

            LoginCommand = ReactiveCommand.Create(Login, canLogin);
            LoginCommand.ThrownExceptions.Subscribe(x =>
                       windowNotificationManager?.Show(new Avalonia.Controls.Notifications.Notification("Error", x.Message, NotificationType.Error)));
        }


        public void Login()
        {
            throw new AccessViolationException();
        }

    }
}
