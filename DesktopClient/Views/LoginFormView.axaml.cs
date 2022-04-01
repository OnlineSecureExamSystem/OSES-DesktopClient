using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using DesktopClient.ViewModels;
using ReactiveUI;
using System;

namespace DesktopClient.Views
{
    public partial class LoginFormView : ReactiveUserControl<LoginFormViewModel>
    {

        public LoginFormView()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

    }
}
