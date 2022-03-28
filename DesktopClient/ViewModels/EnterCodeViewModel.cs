using Avalonia.Controls.Notifications;
using DesktopClient.Helpers;
using DesktopClient.Models;
using DesktopClient.Views;
using ReactiveUI;
using System;
using System.Reactive;

namespace DesktopClient.ViewModels
{
    internal class EnterCodeViewModel : ViewModelBase
    {

        private string? _code;

        public string? Code
        {
            get => _code;
            set
            {
                value?.IsValid(DataTypes.Code);
                this.RaiseAndSetIfChanged(ref _code, value);
            }
        }

        private ReactiveCommand<Unit, Unit> EnterCommand { get; }


        public EnterCodeViewModel()
        {
            var canEnter = this.WhenAnyValue(
                x => x.Code,
                (code) =>
                    !string.IsNullOrEmpty(code)
                );

            EnterCommand = ReactiveCommand.Create(Enter, canEnter);

            EnterCommand.ThrownExceptions.Subscribe(x =>
                    MainWindow.WindowNotificationManager?
                        .Show(new Avalonia.Controls.Notifications.Notification(
                            "Error",
                            x.Message,
                            NotificationType.Error)));
        }

        private void Enter()
        {
            throw new NotImplementedException();
        }
    }
}
