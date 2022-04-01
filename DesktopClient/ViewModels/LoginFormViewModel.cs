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
using static DesktopClient.Views.LoginFormView;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using System.Threading.Tasks;
using System.Threading;
using DesktopClient.CustomControls;
using Avalonia.Threading;

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
        #endregion

        public ReactiveCommand<Unit, Unit> LoginCommand { get; }

        public string? UrlPathSegment => "login_form_path";

        public IScreen HostScreen { get; }

        public LoginFormViewModel(IScreen screen)
        {
            HostScreen = screen;
            var canLogin = this.WhenAnyValue(
                x => x.Email, x => x.Password,
                (email, pass) =>
                    !string.IsNullOrEmpty(email) &&
                    !string.IsNullOrEmpty(pass)
                );

            LoginCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                await Task.Run(() => Thread.Sleep(3000));
                HostScreen.Router.Navigate.Execute(new EnterCodeViewModel(screen));
            }, canLogin);


            // exception handeling
            LoginCommand.ThrownExceptions.Subscribe(x =>
                      MainWindow.WindowNotificationManager?.Show(new Avalonia.Controls.Notifications.Notification("Error",
                      x.Message,
                      NotificationType.Error)));
        }

        //private async void Login()
        //{
        //    loginForm.FindControl<ProgressBar>("progressBar").IsVisible = true;
        //    // login logic
        //    await Task.Run(() => Thread.Sleep(3000));
        //    StepManagerViewModel.NavigateCommand.Execute(new EnterCode()).Subscribe();
        //}

    }
}
