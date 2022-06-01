using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using DesktopClient.ViewModels;
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
        public void OpenBrowser(object sender, PointerPressedEventArgs args) => throw new NotImplementedException();
    }
}
