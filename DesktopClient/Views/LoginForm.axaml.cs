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
    public partial class LoginForm : UserControl
    {
        public LoginForm()
        {
            InitializeComponent();
            DataContext = new LoginFormViewModel();
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void Loaded(object sender, VisualTreeAttachmentEventArgs e)
        {
            Control border = this.FindControl<Border>("BorderToAnimate");
            border.Classes.Add("Fade");
        }
    }
}
