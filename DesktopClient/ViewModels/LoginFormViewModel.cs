using Avalonia.Controls.Notifications;
using DesktopClient.Helpers;
using DesktopClient.Models;
using DesktopClient.Services;
using DesktopClient.Views;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.Net;
using System.Reactive;

namespace DesktopClient.ViewModels
{
    public class LoginFormViewModel : ViewModelBase, IRoutableViewModel
    {
        #region Properties
        private string? _email = "rd077gamer@gmail.com";

        public string? Email
        {
            get => _email;
            set
            {
                value?.IsValid(DataTypes.Email);
                this.RaiseAndSetIfChanged(ref _email, value);
            }
        }

        private string? _password = "sEUdRylySd";


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

        public IObservable<bool> Executing => LoginCommand.IsExecuting;

        public ReactiveCommand<Unit, Unit> LoginCommand { get; }

        public StepManagerViewModel StepManager { get; }

        public ReactiveCommand<Unit, Unit> OpenBrowser { get; }

        public string? UrlPathSegment => "/LoginForm";

        public IScreen HostScreen { get; }

        public MainWindowViewModel MainWindowP { get; }

        public LoginFormViewModel(IScreen screen, MainWindowViewModel mainWindow)
        {


            HostScreen = screen;
            StepManager = (StepManagerViewModel)screen;
            MainWindowP = mainWindow;

            var canLogin = this.WhenAnyValue(
                x => x.Email, x => x.Password,
                (email, pass) =>
                    !string.IsNullOrEmpty(email) &&
                    !string.IsNullOrEmpty(pass)
                );

            LoginCommand = ReactiveCommand.CreateFromTask(async () =>
            {


                AuthenticationService authenticationService = new AuthenticationService();
                var result = await authenticationService.Login(Email, Password);

                if (result.StatusCode != HttpStatusCode.BadRequest)
                {
                    HostScreen.Router.Navigate.Execute(new ChooseVerificationMethodViewModel(screen, StepManager, MainWindowP));
                }
                else
                {
                    ApiNotifier.Notify(result);
                }
            }, canLogin);

            //LoginCommand = ReactiveCommand.CreateFromTask(async () =>
            //{
            //    StreamingHelper streamingHelper = new StreamingHelper();
            //    //streamingHelper.InitWebsocket();
            //    streamingHelper.InitWebsocket();
            //});

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
