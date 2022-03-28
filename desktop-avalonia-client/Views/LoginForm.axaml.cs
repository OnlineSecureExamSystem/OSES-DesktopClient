using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using DesktopClient.Helpers;
using DesktopClient.Models;
using ReactiveUI;
using System;
using System.ComponentModel;
using System.Reactive;

namespace DesktopClient.Views
{
    public partial class LoginForm : ReactiveUserControl<LoginForm>
    {


        private string? _email;

        private string? Email
        {
            get => _email;
            set
            {
                value?.IsValid(DataTypes.Email);
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private string? _password;


        private string? Password
        {
            get => _password;
            set
            {
                value?.IsValid(DataTypes.Password);
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }


        private ReactiveCommand<Unit, Unit> LoginCommand { get; }
        public LoginForm()
        {
            InitializeComponent();
            DataContext = this;
            var canLogin = this.WhenAnyValue(
                x => x.Email, x => x.Password,
                (email, pass) =>
                    !string.IsNullOrEmpty(email) &&
                    !string.IsNullOrEmpty(pass)
                );

            LoginCommand = ReactiveCommand.Create(Login, canLogin);
            LoginCommand.ThrownExceptions.Subscribe(x =>
                MainWindow.WindowNotificationManager?.Show(new Avalonia.Controls.Notifications.Notification("Error", x.Message, Avalonia.Controls.Notifications.NotificationType.Error)));

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void Login()
        {

        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
