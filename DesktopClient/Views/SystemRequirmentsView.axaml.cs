using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using DesktopClient.Helpers;
using DesktopClient.ViewModels;
using System;

namespace DesktopClient.Views
{
    public partial class SystemRequirmentsView : ReactiveUserControl<SystemRequirmentsViewModel>
    {
        public SystemRequirmentsView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
