using Avalonia.Controls.Notifications;
using DesktopClient.Helpers;
using DesktopClient.Models;
using DesktopClient.Views;
using ReactiveUI;
using System;
using System.Reactive;

namespace DesktopClient.ViewModels
{
    public class EnterCodeViewModel : ViewModelBase, IRoutableViewModel
    {
        #region Properties
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
        #endregion


        private ReactiveCommand<Unit, IRoutableViewModel> EnterCommand { get; }

        public string? UrlPathSegment => "enter_code_path";

        public IScreen HostScreen { get; }

        public EnterCodeViewModel()
        {

        }

        public EnterCodeViewModel(IScreen screen)
        {
            HostScreen = screen;
            
            var canEnter = this.WhenAnyValue(
                x => x.Code,
                (code) =>
                    !string.IsNullOrEmpty(code)
                );

            EnterCommand = ReactiveCommand.CreateFromObservable(() => 
                screen.Router.Navigate.Execute(new LoginFormViewModel(screen))
            );

            EnterCommand.ThrownExceptions.Subscribe(x =>
                    MainWindow.WindowNotificationManager?
                        .Show(new Avalonia.Controls.Notifications.Notification(
                            "Error",
                            x.Message,
                            NotificationType.Error)));
        }

        
    }
}
