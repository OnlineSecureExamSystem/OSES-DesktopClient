using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Data;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Text;
using System.Text.RegularExpressions;
using static examClientMVVM.Views.MainWindow;

namespace examClientMVVM.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        string email;

        public string Email
        {
            get => email;
            set
            {
                if (!Regex.IsMatch(value, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"))
                {
                    throw new DataValidationException("invalid email");
                }
                this.RaiseAndSetIfChanged(ref email, value);
            }
        }

        string password;

        public string Password
        {
            get => password;
            set => this.RaiseAndSetIfChanged(ref password, value);
        }


        public ReactiveCommand<Unit, Unit> LoginCommand { get; }

        public MainWindowViewModel()
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
